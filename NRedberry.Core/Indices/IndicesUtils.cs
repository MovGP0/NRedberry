using NRedberry.Contexts;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Maths;
using CC = NRedberry.Core.Tensors.CC;

namespace NRedberry.Core.Indices;

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
    public static int CreateIndex(int name, IndexType type, bool state)
    {
        return CreateIndex(name, type.GetType_(), state);
    }

    public static int CreateIndex(int name, byte type, bool state)
    {
        return (int)((name & 0xFFFF) | ((0x7F & type) << 24) | (state ? 0x80000000 : 0));
    }

    public static int GetRawStateInt(int index)
    {
        return (int)(index & 0x80000000);
    }

    public static long GetStateInt(long index)
    {
        return (int)((index & 0x80000000) >> 31);
    }

    public static bool GetState(long index)
    {
        return (index & 0x80000000) == 0x80000000;
    }

    public static int InverseIndexState(int index)
    {
        return (int)(0x80000000 ^ index);
    }

    public static int GetNameWithType(int index)
    {
        return index & 0x7FFFFFFF;
    }

    public static int SetType(byte type, int index)
    {
        return (int)(0x80FFFFFF & index) | ((0x7F & type) << 24);
    }

    public static int SetType(IndexType type, int index)
    {
        return SetType(type.GetType_(), index);
    }

    public static int SetRawState(int rawState, int index)
    {
        return rawState | index;
    }

    public static int GetNameWithoutType(int index)
    {
        return index & 0xFFFF;
    }

    public static byte GetType(int index)
    {
        return (byte)((index & 0x7FFFFFFF) >> 24);
    }

    public static IndexType GetTypeEnum(int index)
    {
        foreach (IndexType type in Enum.GetValues(typeof(IndexType)))
        {
            if (type.GetType_() == GetType_(index))
                return type;
        }

        throw new Exception("Unknown type");
    }

    public static long GetTypeInt(long index)
    {
        return (index & 0x7FFFFFFF) >> 24;
    }

    public static long GetRawTypeInt(long index)
    {
        return index & 0x7F000000;
    }

    public static byte GetTypeWithState(long index)
    {
        return (byte)(index >> 24);
    }

    public static bool HasEqualTypeAndName(long index0, long index1)
    {
        return (index0 & 0x7FFFFFFF) == (index1 & 0x7FFFFFFF);
    }

    public static bool HasEqualTypes(long index0, long index1)
    {
        return (index0 & 0x7F000000) == (index1 & 0x7F000000);
    }

    public static bool HasEqualTypesAndStates(long index0, long index1)
    {
        return (index0 & 0xFF000000) == (index1 & 0xFF000000);
    }

    public static bool AreContracted(long index0, long index1)
    {
        return (index0 ^ index1) == 0x80000000;
    }

    public static int[] GetSortedDistinctIndicesNames(Indices indices)
    {
        int[] indsArray = indices.GetAllIndices().ToArray(); // Assuming copy() is equivalent to ToArray()
        for (int i = 0; i < indsArray.Length; ++i)
            indsArray[i] = GetNameWithType(indsArray[i]);
        return indsArray.GetSortedDistinct();
    }

    public static string ToString(long index, OutputFormat mode)
    {
        return (GetState(index) ? "^{" : "_{") + Context.Get().GetIndexConverterManager().GetSymbol(index, mode) + "}";
    }

    public static string ToString(long index)
    {
        return ToString(index, Context.Get().GetDefaultOutputFormat());
    }

    public static string ToString(int[] indices, OutputFormat mode)
    {
        return IndicesFactory.CreateSimple(null, indices).ToString(mode);
    }

    public static string ToString(int[] indices)
    {
        return ToString(indices, CC.GetDefaultOutputFormat());
    }

    public static int ParseIndex(string @string)
    {
        bool state = @string[0] == '^';
        var nameWithType = Context.Get()
            .GetIndexConverterManager()
            .GetCode(@string[1] == '{' ? @string.Substring(2, @string.Length - 1) : @string[1..]);

        return state ? (int)(0x80000000 ^ nameWithType) : nameWithType;
    }

    public static int[] GetIndicesNames(Indices indices)
    {
        var a = new int[indices.Size()];
        for (int i = indices.Size() - 1; i >= 0; --i)
            a[i] = GetNameWithType(indices[i]);
        return a;
    }

    public static int[] GetIndicesNames(int[] indices)
    {
        int[] a = new int[indices.Length];
        for (int i = a.Length - 1; i >= 0; --i)
            a[i] = GetNameWithType(indices[i]);
        return a;
    }

    public static int[] GetFree(int[] indices)
    {
        return IndicesFactory.CreateSimple(null, indices).GetFree().GetAllIndices().ToArray();
    }

    public static bool HaveEqualStates(int index1, int index2)
    {
        return GetRawStateInt(index1) == GetRawStateInt(index2);
    }

    public static bool IsPermutationConsistentWithIndices(int[] indices, int[] permutation)
    {
        if (indices.Length != permutation.Length)
            return false;
        for (int i = 0; i < permutation.Length; ++i)
        {
            if (GetRawTypeInt(indices[i]) != GetRawTypeInt(indices[permutation[i]]))
                return false;
        }

        return true;
    }

    public static bool IsPermutationConsistentWithIndices(int[] indices, Permutation permutation)
    {
        if (indices.Length != permutation.Length)
            return false;
        for (int i = 0; i < permutation.Length; ++i)
        {
            if (GetRawTypeInt(indices[i]) != GetRawTypeInt(indices[permutation.NewIndexOf(i)]))
                return false;
        }

        return true;
    }

    public static bool EqualsRegardlessOrder(Indices indices1, int[] indices2)
    {
        if (indices1 is EmptyIndices)
            return indices2.Length == 0;
        if (indices1.Size() != indices2.Length)
            return false;
        int[] temp = (int[])indices2.Clone();
        Array.Sort(temp);
        return ((AbstractIndices)indices1).GetSortedData().SequenceEqual(temp);
    }

    public static bool EqualsRegardlessOrder(int[] indices1, int[] indices2)
    {
        if (indices1.Length != indices2.Length)
            return false;
        int[] temp1 = (int[])indices1.Clone();
        int[] temp2 = (int[])indices2.Clone();
        Array.Sort(temp1);
        Array.Sort(temp2);
        return temp1.SequenceEqual(temp2);
    }

    public static bool HaveIntersections(Indices u, Indices v)
    {
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

    public static byte GetType_(int index) {
        return (byte) ((index & 0x7FFFFFFF) >>> 24);
    }

    public static int[] GetIntersections(int[] freeIndices1, int[] freeIndices2)
    {
        //micro optimization
        if (freeIndices1.Length > freeIndices2.Length)
        {
            (freeIndices1, freeIndices2) = (freeIndices2, freeIndices1);
        }

        List<int> contracted = [];
        foreach (var t in freeIndices1)
        {
            foreach (var t1 in freeIndices2)
            {
                if (t1 == InverseIndexState(t))
                {
                    contracted.Add(GetNameWithType(t1));
                }
            }
        }

        return contracted.ToArray();
    }

    public static int[] GetIntersections(Indices u, Indices v)
    {
        if (u.Size() == 0 || v.Size() == 0)
            return [];

        Indices freeU = u.GetFree();
        Indices freeV = v.GetFree();
        if (freeU.Size() == 0 || freeV.Size() == 0)
            return [];

        return GetIntersections(((AbstractIndices)freeU).Data, ((AbstractIndices)freeV).Data);
    }
}
