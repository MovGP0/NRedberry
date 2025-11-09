using System.Collections;
using System.Linq;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static bool TestPermutationCorrectness(int[] permutation, bool sign)
    {
        return TestPermutationCorrectness(permutation) && (!sign || !OrderOfPermutationIsOdd(permutation));
    }

    public static bool TestPermutationCorrectness(short[] permutation, bool sign)
    {
        return TestPermutationCorrectness(permutation) && (!sign || !OrderOfPermutationIsOdd(permutation));
    }

    public static bool TestPermutationCorrectness(sbyte[] permutation, bool sign)
    {
        return TestPermutationCorrectness(permutation) && (!sign || !OrderOfPermutationIsOdd(permutation));
    }

    public static bool TestPermutationCorrectness(int[] permutation)
    {
        int length = permutation.Length;
        var checkedBits = new BitArray(length);
        for (int i = 0; i < length; ++i)
        {
            int value = permutation[i];
            if (value < 0 || value >= length)
                return false;
            checkedBits.Set(value, true);
        }

        return checkedBits.Cast<bool>().All(b => b);
    }

    public static bool TestPermutationCorrectness(short[] permutation)
    {
        int length = permutation.Length;
        var checkedBits = new BitArray(length);
        for (int i = 0; i < length; ++i)
        {
            int value = permutation[i];
            if (value < 0 || value >= length)
                return false;
            checkedBits.Set(value, true);
        }

        return checkedBits.Cast<bool>().All(b => b);
    }

    public static bool TestPermutationCorrectness(sbyte[] permutation)
    {
        int length = permutation.Length;
        var checkedBits = new BitArray(length);
        for (int i = 0; i < length; ++i)
        {
            int value = permutation[i];
            if (value < 0 || value >= length)
                return false;
            checkedBits.Set(value, true);
        }

        return checkedBits.Cast<bool>().All(b => b);
    }
}
