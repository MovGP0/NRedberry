using System.Diagnostics;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;
using NRedberry.Groups;

namespace NRedberry.Indices;

/// <remarks>
/// original source file: AbstractSimpleIndices.java
/// </remarks>
public abstract class SimpleIndicesAbstract : AbstractIndices, SimpleIndices
{
    protected IndicesSymmetries? symmetries;

    protected SimpleIndicesAbstract(int[] data, IndicesSymmetries symmetries)
        : base(data)
    {
        Debug.Assert(data.Length != 0);

        int[] toSort = new int[data.Length];
        for (int i = 0; i < data.Length; ++i)
            toSort[i] = data[i] & 0x7F000000;
        ArraysUtils.StableSort(toSort, data);
        this.symmetries = symmetries;
        TestConsistentWithException();
    }

    protected SimpleIndicesAbstract(bool notResort, int[] data, IndicesSymmetries? symmetries)
        : base(data)
    {
        Debug.Assert(data.Length != 0);
        this.symmetries = symmetries;
    }

    protected override UpperLowerIndices CalculateUpperLower()
    {
        int upperCount = 0;
        for (int i = 0; i < Data.Length; ++i)
        {
            if ((Data[i] & unchecked((int)0x80000000)) == unchecked((int)0x80000000))
                upperCount++;
        }

        int[] lower = new int[Data.Length - upperCount];
        int[] upper = new int[upperCount];
        int ui = 0;
        int li = 0;
        foreach (int index in Data)
        {
            if ((index & unchecked((int)0x80000000)) == unchecked((int)0x80000000))
            {
                upper[ui++] = index;
            }
            else
            {
                lower[li++] = index;
            }
        }

        return new UpperLowerIndices(upper, lower);
    }

    public override int[] GetSortedData()
    {
        int[] sorted = (int[])Data.Clone();
        Array.Sort(sorted);
        return sorted;
    }

    public override Indices GetOfType(IndexType type)
    {
        int typeMask = type.GetType_() << 24;
        int i = 0;
        for (; i < Data.Length && (Data[i] & 0x7F000000) != typeMask; ++i)
        {
        }

        int start = i;
        for (; i < Data.Length && (Data[i] & 0x7F000000) == typeMask; ++i)
        {
        }

        int[] newData;
        if (start == 0 && i == Data.Length)
        {
            newData = Data;
        }
        else
        {
            newData = new int[i - start];
            Array.Copy(Data, start, newData, 0, i - start);
        }

        return Create(newData, null);
    }

    public override void TestConsistentWithException()
    {
        for (int i = 0; i < Data.Length - 1; ++i)
        {
            for (int j = i + 1; j < Data.Length; ++j)
            {
                if (Data[i] == Data[j])
                    throw new InconsistentIndicesException(Data[i]);
            }
        }
    }

    public override Indices ApplyIndexMapping(IIndexMapping mapping)
    {
        bool changed = false;
        int[] dataCopy = (int[])Data.Clone();
        for (int i = 0; i < dataCopy.Length; ++i)
        {
            int mapped = mapping.Map(dataCopy[i]);
            if (mapped != dataCopy[i])
            {
                dataCopy[i] = mapped;
                changed = true;
            }
        }

        if (!changed)
            return this;

        SimpleIndices result = Create(dataCopy, symmetries);
        result.TestConsistentWithException();
        return result;
    }

    public override int Size(IndexType type)
    {
        int typeMask = type.GetType_() << 24;
        int i = 0;
        for (; i < Data.Length && (Data[i] & 0x7F000000) != typeMask; ++i)
        {
        }

        int size = 0;
        for (; i + size < Data.Length && (Data[i + size] & 0x7F000000) == typeMask; ++size)
        {
        }

        return size;
    }

    public override int this[IndexType type, int position]
    {
        get
        {
            int typeMask = type.GetType_() << 24;
            int i = 0;
            for (; i < Data.Length && (Data[i] & 0x7F000000) != typeMask; ++i)
            {
            }

            int index = Data[i + position];
            if ((index & 0x7F000000) != typeMask)
                throw new IndexOutOfRangeException();
            return index;
        }
    }

    public override Indices GetFree()
    {
        List<int> free = new();
        for (int i = 0; i < Data.Length; i++)
        {
            bool isFree = true;
            for (int j = 0; j < Data.Length; j++)
            {
                if (i != j && (Data[i] ^ Data[j]) == unchecked((int)0x80000000))
                {
                    isFree = false;
                    break;
                }
            }

            if (isFree)
                free.Add(Data[i]);
        }

        return IndicesFactory.CreateSimple(null, free.ToArray());
    }

    public override Indices GetInverted()
    {
        int[] inverted = new int[Data.Length];
        for (int i = 0; i < Data.Length; ++i)
            inverted[i] = Data[i] ^ unchecked((int)0x80000000);
        return Create(inverted, symmetries);
    }

    public override short[] GetDiffIds()
    {
        return symmetries?.DiffIds ?? new short[Data.Length];
    }

    protected abstract SimpleIndices Create(int[] data, IndicesSymmetries? symmetries);

    public bool EqualsWithSymmetries(SimpleIndices indices) => EqualsWithSymmetriesDetailed(indices) ?? false;

    public bool? EqualsWithSymmetriesDetailed(SimpleIndices indices)
    {
        if (indices.GetType() != GetType())
            return false;

        if (Data.Length != indices.Size())
            return false;

        int[] permutation = new int[Data.Length];
        for (int i = 0; i < Data.Length; ++i)
        {
            int from = Data[i];
            bool found = false;
            for (int j = 0; j < Data.Length; ++j)
            {
                if (indices[j] == from)
                {
                    permutation[j] = i;
                    found = true;
                    break;
                }
            }

            if (!found)
                return false;
        }

        if (!Permutations.TestPermutationCorrectness(permutation))
            return false;

        if (symmetries is null)
            return Data.SequenceEqual(((AbstractIndices)indices).Data);

        return Symmetries.PermutationGroup.MembershipTest(Permutations.CreatePermutation(permutation));
    }

    public IndicesSymmetries Symmetries
    {
        get => symmetries ?? throw new InvalidOperationException("Symmetries are not set.");
        set => symmetries = value ?? throw new ArgumentNullException(nameof(value));
    }

    public short[]? DiffIds => symmetries?.DiffIds;
    public StructureOfIndices StructureOfIndices => StructureOfIndices.Create((SimpleIndices)this);
}
