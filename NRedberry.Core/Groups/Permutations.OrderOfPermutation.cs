using System.Collections;
using System.Numerics;

namespace NRedberry.Groups;

public static partial class Permutations
{
    public static BigInteger OrderOfPermutation(int[] permutation)
    {
        var used = new BitArray(permutation.Length);
        BigInteger lcm = BigInteger.One;

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

            counter += currentSize;
            var temp = new BigInteger(currentSize);
            lcm = (lcm / BigInteger.GreatestCommonDivisor(lcm, temp)) * temp;
        }

        return lcm;
    }

    public static BigInteger OrderOfPermutation(short[] permutation)
    {
        var used = new BitArray(permutation.Length);
        BigInteger lcm = BigInteger.One;

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

            counter += currentSize;
            var temp = new BigInteger(currentSize);
            lcm = (lcm / BigInteger.GreatestCommonDivisor(lcm, temp)) * temp;
        }

        return lcm;
    }

    public static BigInteger OrderOfPermutation(sbyte[] permutation)
    {
        var used = new BitArray(permutation.Length);
        BigInteger lcm = BigInteger.One;

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

            counter += currentSize;
            var temp = new BigInteger(currentSize);
            lcm = (lcm / BigInteger.GreatestCommonDivisor(lcm, temp)) * temp;
        }

        return lcm;
    }
}
