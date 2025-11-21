using System.Collections;

namespace NRedberry.Groups;

public static partial class Permutations
{
    private static int NextZeroBit(BitArray bitArray)
    {
        for (int i = 0; i < bitArray.Length; ++i)
        {
            if (!bitArray.Get(i))
            {
                return i;
            }
        }

        return -1;
    }

    private static int NextZeroBit(BitArray bitArray, int startIndex)
    {
        for (int i = startIndex; i < bitArray.Length; i++)
        {
            if (!bitArray.Get(i))
            {
                return i;
            }
        }

        return -1; // This should never happen if the function is called correctly
    }
}
