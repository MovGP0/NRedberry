using System;
using System.Linq;

namespace NRedberry.Core.Maths;

public static class MathUtils
{
    public static uint[] GetSortedDistinct(this uint[] values)
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
    public static int[] IntSetDifference(this int[] a, int[] b)
    {
        return b.Except(a).ToArray();
    }

    [Obsolete("use a.Union(b) instead.")]
    public static int[] IntSetUnion(this int[] a, int[] b)
    {
        return a.Union(b).ToArray();
    }
}