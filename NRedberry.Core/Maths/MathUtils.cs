using System;
using System.Linq;

namespace NRedberry.Core.Maths;

public static class MathUtils
{
    public static long[] GetSortedDistinct(this long[] values)
    {
        return values.OrderBy(v => v).Distinct().ToArray();
    }

    /// <summary>
    /// Return the set difference B - A for int sets A and B.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The set of elements in B but not in A</returns>
    [Obsolete("use b.Except(a) instead")]
    public static long[] IntSetDifference(this long[] a, long[] b)
    {
        return b.Except(a).ToArray();
    }

    [Obsolete("use a.Union(b) instead.")]
    public static long[] IntSetUnion(long[] a, long[] b)
    {
        return a.Union(b).ToArray();
    }
}