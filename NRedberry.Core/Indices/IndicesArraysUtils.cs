using System.Collections.Immutable;

namespace NRedberry.Indices;

public static class IndicesArraysUtils
{
    public static void ArrayCopy(ImmutableArray<int> source, int srcPos, int[] dest, int destPos, int length)
    {
        Array.Copy(source.ToArray(), srcPos, dest, destPos, length);
    }
}
