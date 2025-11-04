using NRedberry.Core.Utils;

namespace NRedberry.Core.Indices;

public static class IndicesArraysUtils
{
    public static void ArrayCopy(IntArray source, int srcPos, int[] dest, int destPos, int length)
    {
        Array.Copy(source.InnerArray, srcPos, dest, destPos, length);
    }
}
