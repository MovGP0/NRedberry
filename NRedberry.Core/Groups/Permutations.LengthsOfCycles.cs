using System;
using System.Collections.Generic;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    /// <summary>
    /// Returns an array of cycles lengths.
    /// </summary>
    /// <param name="permutation">Permutation written in one-line notation.</param>
    /// <returns>An array of cycles lengths.</returns>
    public static int[] LengthsOfCycles(sbyte[] permutation)
    {
        var sizes = new List<int>();
        var seen = new bool[permutation.Length];
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (permutation[start] == start)
            {
                counter++;
                seen[start] = true;
                continue;
            }

            int size = 0;
            while (!seen[start])
            {
                seen[start] = true;
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
        var sizes = new List<int>();
        var seen = new bool[permutation.Length];
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (permutation[start] == start)
            {
                counter++;
                seen[start] = true;
                continue;
            }

            int size = 0;
            while (!seen[start])
            {
                seen[start] = true;
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
        var sizes = new List<int>();
        var seen = new bool[permutation.Length];
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (permutation[start] == start)
            {
                counter++;
                seen[start] = true;
                continue;
            }

            int size = 0;
            while (!seen[start])
            {
                seen[start] = true;
                counter++;
                size++;
                start = permutation[start];
            }
            sizes.Add(size);
        }

        return sizes.ToArray();
    }
}