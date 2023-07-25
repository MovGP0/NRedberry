using System.Collections;

namespace NRedberry.Core;

public static class BitArrayExtensions
{
    /// <summary>
    /// Finds the index of the next 'true' bit in the <see cref="System.Collections.BitArray"/> starting from the provided start index.
    /// </summary>
    /// <param name="bitArray">The <see cref="System.Collections.BitArray"/> to search.</param>
    /// <param name="startIndex">The index from which to start the search.</param>
    /// <returns>The index of the next 'true' bit found, or -1 if no 'true' bits are found after the start index.</returns>
    public static int NextTrailingBit(this BitArray bitArray, int startIndex)
    {
        for (int i = startIndex; i < bitArray.Count; i++)
        {
            if (bitArray[i])
            {
                return i;
            }
        }
        return -1;  // Return -1 if no 'true' bits found
    }
}