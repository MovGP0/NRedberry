using System.Collections;

namespace NRedberry.Core.Combinatorics.Extensions;

internal static class BitArrayExtensions
{
    public static int NextTrailingBit(this BitArray bitArray, int fromIndex)
    {
        if (fromIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex), "Index cannot be negative.");
        }

        for (int i = fromIndex; i >= 0; i--)
        {
            if (bitArray[i])
            {
                return i;
            }
        }

        return -1; // Return -1 if no trailing bit is set to true
    }
}
