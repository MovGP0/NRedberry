using System.Collections;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static bool OrderOfPermutationIsOdd(int[] permutation)
    {
        var used = new BitArray(permutation.Length);
        int counter = 0;
        while (counter < permutation.Length)
        {
            int pointer = NextZeroBit(used);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used.Set(pointer, true);
                pointer = permutation[pointer];
                ++currentSize;
            } while (pointer != start);
            if (currentSize % 2 == 0)
                return false;
            counter += currentSize;
        }

        return true;
    }

    public static bool OrderOfPermutationIsOdd(short[] permutation)
    {
        var used = new BitArray(permutation.Length);
        int counter = 0;
        while (counter < permutation.Length)
        {
            int pointer = NextZeroBit(used);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used.Set(pointer, true);
                pointer = permutation[pointer];
                ++currentSize;
            } while (pointer != start);
            if (currentSize % 2 == 0)
                return false;
            counter += currentSize;
        }

        return true;
    }

    public static bool OrderOfPermutationIsOdd(sbyte[] permutation)
    {
        var used = new BitArray(permutation.Length);
        int counter = 0;
        while (counter < permutation.Length)
        {
            int pointer = NextZeroBit(used);
            int start = pointer;
            int currentSize = 0;
            do
            {
                used.Set(pointer, true);
                pointer = permutation[pointer];
                ++currentSize;
            } while (pointer != start);
            if (currentSize % 2 == 0)
                return false;
            counter += currentSize;
        }

        return true;
    }
}
