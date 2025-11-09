using System.Collections;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int Parity(int[] permutation)
    {
        var used = new BitArray(permutation.Length);
        int counter = 0;
        int numOfTranspositions = 0;
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
            counter += currentSize;
            numOfTranspositions += currentSize - 1;
        }

        return numOfTranspositions % 2;
    }

    public static int Parity(short[] permutation)
    {
        var used = new BitArray(permutation.Length);
        int counter = 0;
        int numOfTranspositions = 0;
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
            counter += currentSize;
            numOfTranspositions += currentSize - 1;
        }

        return numOfTranspositions % 2;
    }

    public static int Parity(sbyte[] permutation)
    {
        var used = new BitArray(permutation.Length);
        int counter = 0;
        int numOfTranspositions = 0;
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
            counter += currentSize;
            numOfTranspositions += currentSize - 1;
        }

        return numOfTranspositions % 2;
    }
}
