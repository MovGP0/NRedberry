using System.Collections;

namespace NRedberry.Core.Utils;

public static class BitArrayExtensions
{
    public static BitArray Empty => new(0);

    public static T[] CopyOfRange<T>(this T[] source, int start, int end)
    {
        if (end < start)
            throw new ArgumentException("End index must be greater than or equal to the start index.");

        int length = end - start;
        T[] result = new T[length];
        Array.Copy(source, start, result, 0, length);
        return result;
    }

    public static BitArray Append(this BitArray first, BitArray second)
    {
        BitArray result = new BitArray(first.Count + second.Count);
        int i = 0;

        // Copy bits from the first array
        foreach (bool bit in first)
        {
            result[i] = bit;
            i++;
        }

        // Copy bits from the second array
        foreach (bool bit in second)
        {
            result[i] = bit;
            i++;
        }

        return result;
    }
}
