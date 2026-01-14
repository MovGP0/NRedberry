using System.Collections;

namespace NRedberry.Groups;

public static partial class Permutations
{
    /// <summary>
    /// Returns an array of cycles lengths.
    /// </summary>
    /// <param name="permutation">Permutation written in one-line notation.</param>
    /// <returns>An array of cycles lengths.</returns>
    public static int[] LengthsOfCycles(sbyte[] permutation)
    {
        List<int> sizes = [];
        BitArray seen = new BitArray(permutation.Length);
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = NextZeroBit(seen);
            if (permutation[start] == start)
            {
                counter++;
                seen.Set(start, true);
                continue;
            }

            int size = 0;
            while (!seen.Get(start))
            {
                seen.Set(start, true);
                counter++;
                size++;
                start = permutation[start];
            }

            sizes.Add(size);
        }

        return sizes.ToArray();
    }

    /// <summary>
    /// Returns an array of cycles lengths.
    /// </summary>
    /// <param name="permutation">Permutation written in one-line notation.</param>
    /// <returns>An array of cycles lengths.</returns>
    public static int[] LengthsOfCycles(int[] permutation)
    {
        List<int> sizes = [];
        BitArray seen = new BitArray(permutation.Length);
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = NextZeroBit(seen);
            if (permutation[start] == start)
            {
                counter++;
                seen.Set(start, true);
                continue;
            }

            int size = 0;
            while (!seen.Get(start))
            {
                seen.Set(start, true);
                counter++;
                size++;
                start = permutation[start];
            }

            sizes.Add(size);
        }

        return sizes.ToArray();
    }

    /// <summary>
    /// Returns an array of cycles lengths.
    /// </summary>
    /// <param name="permutation">Permutation written in one-line notation.</param>
    /// <returns>An array of cycles lengths.</returns>
    public static int[] LengthsOfCycles(short[] permutation)
    {
        List<int> sizes = [];
        BitArray seen = new BitArray(permutation.Length);
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = NextZeroBit(seen);
            if (permutation[start] == start)
            {
                counter++;
                seen.Set(start, true);
                continue;
            }

            int size = 0;
            while (!seen.Get(start))
            {
                seen.Set(start, true);
                counter++;
                size++;
                start = permutation[start];
            }

            sizes.Add(size);
        }

        return sizes.ToArray();
    }
}
