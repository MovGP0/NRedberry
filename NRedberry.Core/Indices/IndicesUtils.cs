using System.Collections.Immutable;
using NRedberry.Contexts;
using NRedberry.Core.Combinatorics;
using NRedberry.Maths;
using CC = NRedberry.Tensors.CC;

namespace NRedberry.Indices;

/// <summary>
/// This class provides static methods to work with individual index and indices objects.
/// </summary>
/// <remarks>
/// <h5>Index representation</h5>
/// All information about single index is enclosed in 32-bit word (int). The following bit structure is used:
/// <code>
/// Index: stttttttXXXXXXXXcccccccccccccccc - per-bit representation
/// | | | | |
/// 31 23 15 7 0 - bit index
/// </code>
/// s - one bit representing index state (0 - lower; 1 - upper)
/// t - 7-bits representing index type (lower latin, upper latin, etc...) [for concrete codes see below]
/// c - code of concrete index (a - 0, b - 1, c - 2, etc...) [index name]
/// X - reserved (always 0)
/// <h5>Index types</h5>
/// By default there are four different index types:
/// <table cellspacing="0" cellpadding="5">
/// <caption></caption>
/// <thead>
/// <tr>
/// <th>HexCode</th>
/// <th>BitCode</th>
/// <th>Description</th>
/// </tr>
/// </thead>
/// <tbody>
/// <tr>
/// <td>0x00</td>
/// <td>00000000</td>
/// <td>Latin lower case symbols</td>
/// </tr>
/// <tr>
/// <td>0x01</td>
/// <td>00000001</td>
/// <td>Latin upper case symbols</td>
/// </tr>
/// <tr>
/// <td>0x02</td>
/// <td>00000010</td>
/// <td>Greek lower case symbols</td>
/// </tr>
/// <tr>
/// <td>0x03</td>
/// <td>00000011</td>
/// <td>Greek upper case symbols</td>
/// </tr>
/// </tbody>
/// </table>
/// <h5>Examples</h5>
/// Here are some examples of how concrete indices are presented in Redberry.
/// <table cellspacing="0" cellpadding="5">
/// <caption></caption>
/// <thead>
/// <tr>
/// <th>Index</th>
/// <th>Hex</th>
/// </tr>
/// </thead>
/// <tbody>
/// <tr>
/// <td>_a</td>
/// <td>0x00000000</td>
/// </tr>
/// <tr>
/// <td>_C</td>
/// <td>0x01000002</td>
/// </tr>
/// <tr>
/// <td>^{\beta}</td>
/// <td>0x82000001</td>
/// </tr>
/// <tr>
/// <td>^{\Chi}</td>
/// <td>0x83000015</td>
/// </tr>
/// </tbody>
/// </table>
/// </remarks>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/indices/IndicesUtils.java</remarks>
public sealed class IndicesUtils
{
    public const int UpperRawStateInt = unchecked((int)0x80000000);
    public const int LowerRawStateInt = unchecked((int)0x80000000);

    private const int NameWithTypeMask = 0x7FFFFFFF;
    private const int TypeMask = 0x7F000000;
    private const int TypeStateMask = unchecked((int)0xFF000000);
    private const int TypeUpdateMask = unchecked((int)0x80FFFFFF);

    private IndicesUtils()
    {
    }

    public static int CreateIndex(int name, IndexType type, bool state)
    {
        return CreateIndex(name, type.GetType_(), state);
    }

    public static int CreateIndex(int name, byte type, bool state)
    {
        return (name & 0xFFFF) | ((0x7F & type) << 24) | (state ? UpperRawStateInt : 0);
    }

    public static int GetRawStateInt(int index)
    {
        return index & UpperRawStateInt;
    }

    public static int GetStateInt(int index)
    {
        return (index & UpperRawStateInt) >>> 31;
    }

    public static bool GetState(int index)
    {
        return (index & UpperRawStateInt) == UpperRawStateInt;
    }

    public static int InverseIndexState(int index)
    {
        return UpperRawStateInt ^ index;
    }

    public static int GetNameWithType(int index)
    {
        return index & NameWithTypeMask;
    }

    public static int SetType(byte type, int index)
    {
        return (TypeUpdateMask & index) | ((0x7F & type) << 24);
    }

    public static int SetType(IndexType type, int index)
    {
        return SetType(type.GetType_(), index);
    }

    public static int SetRawState(int rawState, int index)
    {
        return rawState | (index & NameWithTypeMask);
    }

    public static int SetState(bool state, int index)
    {
        return SetRawState(state ? UpperRawStateInt : 0, index);
    }

    public static int Raise(int index)
    {
        return SetRawState(UpperRawStateInt, index);
    }

    public static int Lower(int index)
    {
        return SetRawState(0, index);
    }

    public static int GetNameWithoutType(int index)
    {
        return index & 0xFFFF;
    }

    public static byte GetType(int index)
    {
        return (byte)((index & NameWithTypeMask) >>> 24);
    }

    public static IndexType GetTypeEnum(int index)
    {
        return IndexTypeMethods.GetType(GetType(index));
    }

    public static int GetTypeInt(int index)
    {
        return (index & NameWithTypeMask) >>> 24;
    }

    public static int GetRawTypeInt(int index)
    {
        return index & TypeMask;
    }

    public static byte GetTypeWithState(int index)
    {
        return (byte)(index >>> 24);
    }

    public static bool HasEqualTypeAndName(int index0, int index1)
    {
        return (index0 & NameWithTypeMask) == (index1 & NameWithTypeMask);
    }

    public static bool HasEqualTypes(int index0, int index1)
    {
        return (index0 & TypeMask) == (index1 & TypeMask);
    }

    public static bool HasEqualTypesAndStates(int index0, int index1)
    {
        return (index0 & TypeStateMask) == (index1 & TypeStateMask);
    }

    public static bool AreContracted(int index0, int index1)
    {
        return (index0 ^ index1) == UpperRawStateInt;
    }

    public static int[] GetSortedDistinctIndicesNames(Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        int[] indsArray = indices.AllIndices.ToArray();
        for (int i = 0; i < indsArray.Length; ++i)
        {
            indsArray[i] = GetNameWithType(indsArray[i]);
        }

        return indsArray.GetSortedDistinct();
    }

    public static string ToString(int index, OutputFormat mode)
    {
        return (GetState(index) ? "^" : "_") + Context.Get().ConverterManager.GetSymbol(index, mode);
    }

    public static string ToString(int index)
    {
        return ToString(index, Context.Get().DefaultOutputFormat);
    }

    public static string ToString(int[] indices, OutputFormat mode)
    {
        ArgumentNullException.ThrowIfNull(indices);
        return IndicesFactory.CreateSimple(null, indices).ToString(mode);
    }

    public static string ToString(int[] indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        return ToString(indices, CC.GetDefaultOutputFormat());
    }

    public static int ParseIndex(string @string)
    {
        ArgumentNullException.ThrowIfNull(@string);
        @string = @string.Trim();
        bool state = @string[0] == '^';
        int start = 0;
        if (@string[0] == '^' || @string[0] == '_')
        {
            start = 1;
        }

        int nameWithType;
        if (@string[start] == '{')
        {
            nameWithType = Context.Get().ConverterManager
                .GetCode(@string.Substring(start + 1, @string.Length - start - 2));
        }
        else
        {
            nameWithType = Context.Get().ConverterManager.GetCode(@string.Substring(start));
        }

        return state ? (UpperRawStateInt ^ nameWithType) : nameWithType;
    }

    public static int[] GetIndicesNames(Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        int[] a = new int[indices.Size()];
        for (int i = indices.Size() - 1; i >= 0; --i)
        {
            a[i] = GetNameWithType(indices[i]);
        }

        return a;
    }

    public static int[] GetIndicesNames(int[] indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        int[] a = new int[indices.Length];
        for (int i = a.Length - 1; i >= 0; --i)
        {
            a[i] = GetNameWithType(indices[i]);
        }

        return a;
    }

    public static int[] GetIndicesNames(ImmutableArray<int> indices)
    {
        int[] a = new int[indices.Length];
        for (int i = a.Length - 1; i >= 0; --i)
        {
            a[i] = GetNameWithType(indices[i]);
        }

        return a;
    }

    public static int[] GetFree(int[] indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        return IndicesFactory.CreateSimple(null, indices).GetFree().AllIndices.ToArray();
    }

    public static bool HaveEqualStates(int index1, int index2)
    {
        return GetRawStateInt(index1) == GetRawStateInt(index2);
    }

    public static bool IsPermutationConsistentWithIndices(int[] indices, int[] permutation)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(permutation);
        if (indices.Length != permutation.Length)
        {
            return false;
        }

        for (int i = 0; i < permutation.Length; ++i)
        {
            if (GetRawTypeInt(indices[i]) != GetRawTypeInt(indices[permutation[i]]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsPermutationConsistentWithIndices(int[] indices, Permutation permutation)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(permutation);
        if (indices.Length < permutation.Degree)
        {
            return false;
        }

        int degree = permutation.Degree;
        for (int i = 0; i < degree; ++i)
        {
            if (GetRawTypeInt(indices[i]) != GetRawTypeInt(indices[permutation.NewIndexOf(i)]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool EqualsRegardlessOrder(Indices indices1, int[] indices2)
    {
        ArgumentNullException.ThrowIfNull(indices1);
        ArgumentNullException.ThrowIfNull(indices2);
        if (indices1 is EmptyIndices)
        {
            return indices2.Length == 0;
        }

        if (indices1.Size() != indices2.Length)
        {
            return false;
        }

        int[] temp = (int[])indices2.Clone();
        Array.Sort(temp);
        return ((AbstractIndices)indices1).GetSortedData().SequenceEqual(temp);
    }

    public static bool EqualsRegardlessOrder(int[] indices1, int[] indices2)
    {
        ArgumentNullException.ThrowIfNull(indices1);
        ArgumentNullException.ThrowIfNull(indices2);
        if (indices1.Length != indices2.Length)
        {
            return false;
        }

        int[] temp1 = (int[])indices1.Clone();
        int[] temp2 = (int[])indices2.Clone();
        Array.Sort(temp1);
        Array.Sort(temp2);
        return temp1.SequenceEqual(temp2);
    }

    public static bool HaveIntersections(Indices u, Indices v)
    {
        ArgumentNullException.ThrowIfNull(u);
        ArgumentNullException.ThrowIfNull(v);
        Indices uFree = u.GetFree();
        Indices vFree = v.GetFree();
        if (uFree.Size() > vFree.Size())
        {
            (uFree, vFree) = (vFree, uFree);
        }

        for (int i = 0; i < uFree.Size(); ++i)
        {
            for (int j = 0; j < vFree.Size(); ++j)
            {
                if (vFree[j] == InverseIndexState(uFree[i]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static int[] GetIntersections(int[] freeIndices1, int[] freeIndices2)
    {
        ArgumentNullException.ThrowIfNull(freeIndices1);
        ArgumentNullException.ThrowIfNull(freeIndices2);
        //micro optimization
        if (freeIndices1.Length > freeIndices2.Length)
        {
            (freeIndices1, freeIndices2) = (freeIndices2, freeIndices1);
        }

        List<int> contracted = [];
        for (int i = 0; i < freeIndices1.Length; ++i)
        {
            for (int j = 0; j < freeIndices2.Length; ++j)
            {
                if (freeIndices2[j] == InverseIndexState(freeIndices1[i]))
                {
                    contracted.Add(GetNameWithType(freeIndices2[j]));
                }
            }
        }

        return contracted.ToArray();
    }

    public static int[] GetIntersections(Indices u, Indices v)
    {
        ArgumentNullException.ThrowIfNull(u);
        ArgumentNullException.ThrowIfNull(v);
        if (u.Size() == 0 || v.Size() == 0)
        {
            return [];
        }

        Indices freeU = u.GetFree();
        Indices freeV = v.GetFree();
        if (freeU.Size() == 0 || freeV.Size() == 0)
        {
            return [];
        }

        return GetIntersections(((AbstractIndices)freeU).Data, ((AbstractIndices)freeV).Data);
    }

    public static bool ContainsNonMetric(Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        for (int i = 0; i < indices.Size(); ++i)
        {
            if (!CC.IsMetric(GetType(indices[i])))
            {
                return true;
            }
        }

        return false;
    }

    public static HashSet<IndexType> NonMetricTypes(Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        HashSet<IndexType> types = [];
        for (int i = 0; i < indices.Size(); ++i)
        {
            int index = indices[i];
            if (!CC.IsMetric(GetType(index)))
            {
                types.Add(GetTypeEnum(index));
            }
        }

        return types;
    }

    public static byte GetType_(int index)
    {
        return GetType(index);
    }
}
