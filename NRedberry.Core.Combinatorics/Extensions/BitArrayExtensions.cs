using System.Collections;

namespace NRedberry.Core.Combinatorics.Extensions;

public static class BitArrayExtensions
{
    public static int NextTrailingBit(this BitArray bitArray, int fromIndex)
    {
        ArgumentNullException.ThrowIfNull(bitArray);

        if (fromIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex), "Index cannot be negative.");
        }

        for (int i = fromIndex; i < bitArray.Length; i++)
        {
            if (bitArray[i])
            {
                return i;
            }
        }

        return -1;
    }
}
