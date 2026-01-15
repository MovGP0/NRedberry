namespace NRedberry.Indices;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indices/ShortArrayFactory.java
 */

internal static class ShortArrayFactory
{
    private const int Size = 128;

    private static readonly short[][] s_filledWithZeros = new short[Size][];

    public static short[] GetZeroFilledShortArray(int length)
    {
        if (length >= Size)
        {
            return new short[length];
        }

        return s_filledWithZeros[length] ??= new short[length];
    }
}
