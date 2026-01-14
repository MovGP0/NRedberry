using System.Collections;
using System.Diagnostics;
using System.Text;
using NRedberry.Contexts;
using NRedberry.Core.Utils;

namespace NRedberry.Indices;

/// <summary>
/// The unique identification information about indices objects. This class contains
/// information about types of indices (number of indices of each type) and about
/// states of non-metric indices (if there are any).
/// </summary>
public sealed class StructureOfIndices
{
    public int[] TypesCounts { get; } = new int[IndexTypeMethods.TypesCount];

    private readonly BitArray[] states = new BitArray[IndexTypeMethods.TypesCount];

    public int Size { get; }

    public int Count { get; }

    #region Constructors

    //for empty instance
    private StructureOfIndices()
    {
        Size = 0;
        Count = 0;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (!CC.IsMetric((byte)i))
                states[i] = BitArrayExtensions.Empty;
        }
    }

    private StructureOfIndices(int size)
    {
        Size = size;
        Count = size;
    }

    private StructureOfIndices(byte type, int count, params bool[] states)
    {
        TypesCounts[type] = count;
        Size = count;
        Count = count;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (!CC.IsMetric((byte)i))
                this.states[i] = i == type ? new BitArray(states) : BitArrayExtensions.Empty;
        }
    }

    private StructureOfIndices(byte type, int count)
    {
        if (!CC.IsMetric(type))
            throw new ArgumentException("No states information provided for non metric type.");
        TypesCounts[type] = count;
        Size = count;
        Count = count;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (!CC.IsMetric((byte)i))
                states[i] = BitArrayExtensions.Empty;
        }
    }

    private StructureOfIndices(byte[] types, int[] count)
    {
        for (int i = 0; i < types.Length; ++i)
        {
            if (count[i] != 0 && !CC.IsMetric(types[i]))
                throw new ArgumentException("No states information provided for non metric type.");
        }

        int size = 0;
        for (int i = 0; i < types.Length; ++i)
        {
            TypesCounts[types[i]] = count[i];
            size += count[i];
        }

        Size = size;
        Count = size;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (!CC.IsMetric((byte)i))
                states[i] = BitArrayExtensions.Empty;
        }
    }

    private StructureOfIndices(int[] allCount, BitArray[] allStates)
    {
        if (allCount.Length != IndexTypeMethods.TypesCount || allStates.Length != IndexTypeMethods.TypesCount)
            throw new ArgumentException();
        int i;
        int size = 0;
        for (i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if ((allStates[i] != null && CC.IsMetric((byte)i))
                || (allStates[i] == null && !CC.IsMetric((byte)i)))
            {
                throw new ArgumentException();
            }

            states[i] = allStates[i] == null ? null : (BitArray)allStates[i].Clone();
            size += allCount[i];
        }

        Array.Copy(allCount, 0, TypesCounts, 0, allCount.Length);
        Size = size;
        Count = size;
    }

    internal StructureOfIndices(SimpleIndices indices)
    {
        Size = indices.Size();
        Count = Size;
        int i;
        for (i = 0; i < Size; ++i)
            ++TypesCounts[IndicesUtils.GetType(indices[i])];
        int[] pointers = new int[IndexTypeMethods.TypesCount];
        for (i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (!CC.IsMetric((byte)i))
            {
                states[i] = CreateBBBA(TypesCounts[i]);
            }
            else
            {
                pointers[i] = -1;
            }
        }

        for (i = 0; i < Size; ++i)
        {
            var type = IndicesUtils.GetType(indices[i]);
            if (pointers[type] != -1)
            {
                if (IndicesUtils.GetState(indices[i]))
                    states[type].Set(pointers[type], true);
                ++pointers[type];
            }
        }
    }

    #endregion

    #region Factories

    public static StructureOfIndices Create(byte type, int count, params bool[] states)
    {
        if (count != states.Length)
            throw new ArgumentException();
        if (count == 0)
            return Empty;
        return new StructureOfIndices(type, count, states);
    }

    public static StructureOfIndices Create(byte type, int count)
    {
        if (count == 0)
            return Empty;
        return new StructureOfIndices(type, count);
    }

    public static StructureOfIndices Create(IndexType type, int count)
    {
        return Create(type.GetType_(), count);
    }

    public static StructureOfIndices Create(byte[] types, int[] count)
    {
        int total = 0;
        for (int i = 0; i < count.Length; ++i)
            total += count[i];
        if (total == 0)
            return Empty;
        return new StructureOfIndices(types, count);
    }

    public static StructureOfIndices Create(int[] allCount, BitArray[] allStates)
    {
        int total = 0;
        for (int i = 0; i < allCount.Length; ++i)
        {
            if (allStates[i] != null && allCount[i] != allStates[i].Count)
                throw new ArgumentException("Count differs from states size.");
            total += allCount[i];
        }

        if (total == 0)
            return Empty;
        return new StructureOfIndices(allCount, allStates);
    }

    public static StructureOfIndices Create(SimpleIndices indices)
    {
        if (indices.Size() == 0)
            return Empty;
        return new StructureOfIndices(indices);
    }

    #endregion

    public BitArray[] GetStates()
    {
        BitArray[] statesCopy = new BitArray[states.Length];
        for (int i = 0; i < states.Length; ++i)
            statesCopy[i] = states[i] == null ? null : (BitArray) states[i].Clone();
        return statesCopy;
    }

    public BitArray GetStates(IndexType type)
    {
        return (BitArray) states[type.GetType_()].Clone();
    }

    public bool FixedStates(IndexType type)
    {
        return states[type.GetType_()] != null;
    }

    public int[] GetTypesCounts()
    {
        return (int[])TypesCounts.Clone();
    }

    StructureOfIndices(int[] indices)
    {
        Size = indices.Length;
        Count = Size;
        int i;
        for (i = 0; i < Size; ++i)
            ++TypesCounts[IndicesUtils.GetType_(indices[i])];
        int[] pointers = new int[IndexTypeMethods.TypesCount];
        for (i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (!CC.IsMetric((byte)i))
            {
                states[i] = CreateBBBA(TypesCounts[i]);
            }
            else
            {
                pointers[i] = -1;
            }
        }

        byte type;
        for (i = 0; i < Size; ++i)
        {
            type = IndicesUtils.GetType_(indices[i]);
            if (pointers[type] != -1)
            {
                if (IndicesUtils.GetState(indices[i]))
                    states[type].Set(pointers[type], true);
                ++pointers[type];
            }
        }
    }

    private static BitArray CreateBBBA(int size)
    {
        if (size == 0)
            return BitArrayExtensions.Empty; // Check if there is an Empty property for your BitArray implementation
        return new BitArray(size);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        StructureOfIndices other = (StructureOfIndices) obj;
        if (Size != other.Size)
            return false;
        if (Size == 0)
            return true;

        return TypesCounts.SequenceEqual(other.GetTypesCounts())
            && states.SequenceEqual(other.GetStates(), new BitArrayEqualityComparer());
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 469;
            foreach (var item in TypesCounts)
            {
                hash = (hash * 23) + item.GetHashCode();
            }

            foreach (var item in states)
            {
                hash = (hash * 23) + (item == null ? 0 : item.GetHashCode());
            }

            return hash;
        }
    }

    public TypeData GetTypeData(byte type)
    {
        int from = 0;
        for (int i = 0; i < type; ++i)
            from += TypesCounts[i];
        return new TypeData(from, TypesCounts[type], states[type]);
    }

    public int TypeCount(byte type)
    {
        return TypesCounts[type];
    }

    public bool IsStructureOf(SimpleIndices indices)
    {
        if (Size != indices.Size())
            return false;
        return Equals(indices.StructureOfIndices);
    }

    public StructureOfIndices GetInverted()
    {
        if (Size == 0)
            return this;
        StructureOfIndices r = new StructureOfIndices(Size);
        Array.Copy(TypesCounts, r.TypesCounts, TypesCounts.Length);
        for (int i = r.states.Length - 1; i >= 0; --i)
        {
            if (states[i] == null)
            {
                continue;
            }

            if (states[i].Count == 0)
            {
                r.states[i] = BitArrayExtensions.Empty;
                continue;
            }

            r.states[i] = (BitArray)states[i].Clone();
            r.states[i].Not();
        }

        return r;
    }

    public StructureOfIndices Append(StructureOfIndices oth)
    {
        if (Size == 0)
            return oth;
        if (oth.Size == 0)
            return this;
        int newSize = Size + oth.Size;
        StructureOfIndices r = new StructureOfIndices(newSize);
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            r.TypesCounts[i] = TypesCounts[i] + oth.TypesCounts[i];
            if (states[i] == null)
                continue;
            r.states[i] = states[i].Append(oth.states[i]); // Assuming BitArray.Append exists
        }

        return r;
    }

    public StructureOfIndices Pow(int count)
    {
        if (Size == 0 || count == 0)
        {
            return Empty;
        }

        if (count == 1)
        {
            return this;
        }

        int newSize = Size * count;
        StructureOfIndices r = new StructureOfIndices(newSize);
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            r.TypesCounts[i] = count * TypesCounts[i];
            if (states[i] == null)
            {
                continue;
            }

            r.states[i] = Times(states[i], count);
        }

        return r;
    }

    public StructureOfIndices Subtract(StructureOfIndices other)
    {
        ArgumentNullException.ThrowIfNull(other);
        int newSize = Size - other.Size;
        if (newSize < 0)
        {
            throw new ArgumentException();
        }

        if (other.Size == 0)
        {
            return this;
        }

        StructureOfIndices r = new StructureOfIndices(newSize);
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            int count = TypesCounts[i] - other.TypesCounts[i];
            if (count < 0)
            {
                throw new ArgumentException("Other is larger then this.");
            }

            r.TypesCounts[i] = count;
            if (states[i] == null)
            {
                continue;
            }

            if (other.states[i] == null)
            {
                throw new ArgumentException($"Inconsistent structures: {this} and {other}");
            }

            int otherCount = other.states[i].Count;
            if (!BitArrayEquals(CopyRange(states[i], states[i].Count - otherCount, otherCount), other.states[i]))
            {
                throw new ArgumentException("Nonmetric states are different");
            }

            r.states[i] = CopyRange(states[i], 0, states[i].Count - otherCount);
        }

        return newSize == 0 ? Empty : r;
    }

    public int[][] GetPartitionMappings(params StructureOfIndices[] partition)
    {
        int c;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            c = 0;
            foreach (StructureOfIndices str in partition)
                c += str.TypesCounts[i];
            if (c != TypesCounts[i])
                throw new ArgumentException("Not a partition.");
        }

        {
            int[][] mappings = new int[partition.Length][];
            int i;
            int j;
            int k;

            int[] pointers = new int[IndexTypeMethods.TypesCount];
            for (j = 0; j < IndexTypeMethods.TypesCount; ++j)
                pointers[j] = GetTypeData((byte)j).From;

            for (c = 0; c < partition.Length; ++c)
            {
                mappings[c] = new int[partition[c].Size];
                i = 0;
                for (j = 0; j < IndexTypeMethods.TypesCount; ++j)
                {
                    for (k = partition[c].TypesCounts[j] - 1; k >= 0; --k)
                        mappings[c][i++] = pointers[j]++;
                }

                Debug.Assert(i == partition[c].Size);
            }

            return mappings;
        }
    }

    public override string ToString()
    {
        if (Size == 0)
            return "[]";
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            if (TypesCounts[i] != 0)
            {
                sb.Append('{');
                if (states[i] == null)
                {
                    for (int t = 0; ; ++t)
                    {
                        sb.Append(IndexTypeMethods.Values[i].GetShortString()); // Assuming GetShortString() exists
                        if (t == TypesCounts[i] - 1)
                            break;
                        sb.Append(',');
                    }
                }
                else
                {
                    for (int t = 0; t < TypesCounts[i]; ++t)
                    {
                        sb.Append('(');
                        sb.Append(IndexTypeMethods.Values[i].GetShortString()); // Assuming GetShortString() exists
                        sb.Append(',');
                        sb.Append(states[i].Get(t) ? 1 : 0); // Assuming Get() exists
                        sb.Append(')');
                        if (t == TypesCounts[i] - 1)
                            break;
                        sb.Append(',');
                    }
                }

                sb.Append("},");
            }
        }

        sb.Remove(sb.Length - 1, 1);
        sb.Append(']');
        return sb.ToString();
    }

    private static bool BitArrayEquals(BitArray left, BitArray right)
    {
        return new BitArrayEqualityComparer().Equals(left, right);
    }

    private static BitArray CopyRange(BitArray source, int start, int length)
    {
        if (length <= 0)
        {
            return BitArrayExtensions.Empty;
        }

        BitArray result = new BitArray(length);
        for (int i = 0; i < length; ++i)
        {
            result[i] = source[start + i];
        }

        return result;
    }

    private static BitArray Times(BitArray source, int count)
    {
        if (count <= 0 || source.Count == 0)
        {
            return BitArrayExtensions.Empty;
        }

        BitArray result = new BitArray(source.Count * count);
        int offset = 0;
        for (int i = 0; i < count; ++i)
        {
            for (int j = 0; j < source.Count; ++j)
            {
                result[offset + j] = source[j];
            }

            offset += source.Count;
        }

        return result;
    }

    public static StructureOfIndices Empty { get; } = new();
}
