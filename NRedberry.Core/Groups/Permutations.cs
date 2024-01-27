using System;
using System.Linq;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public static class Permutations
{
    /// <summary>
    /// Converts cycles to one-line notation.
    /// </summary>
    /// <param name="cycles">Disjoint cycles.</param>
    /// <returns>Permutation written in one-line notation.</returns>
    /// <exception cref="ArgumentException">Thrown if there is illegal use of cycle notation.</exception>
    public static int[] ConvertCyclesToOneLine(int[][] cycles)
    {
        int degree = -1;
        foreach (int[] cycle in cycles) {
            degree = Math.Max(degree, cycle.Max());
        }
        ++degree;

        int[] permutation = new int[degree];
        for (int i = 1; i < degree; ++i) {
            permutation[i] = i;
        }

        foreach (int[] cycle in cycles) {
            if (cycle.Length == 0) {
                continue;
            }
            if (cycle.Length == 1) {
                throw new ArgumentException($"Illegal use of cycle notation: {string.Join(", ", cycle)}");
            }

            for (int k = 0, s = cycle.Length - 1; k < s; ++k) {
                permutation[cycle[k]] = cycle[k + 1];
            }
            permutation[cycle[cycle.Length - 1]] = cycle[0];
        }

        return permutation;
    }

    /// <summary>
    /// Creates permutation instance from a given array that represents permutation in disjoint cycle notation.
    /// This method will automatically choose an appropriate underlying implementation of Permutation depending on
    /// the permutation length.
    /// If order of specified permutation is odd and antisymmetry is specified, then an exception will be thrown, since
    /// such antisymmetry is impossible from the mathematical point of view.
    /// </summary>
    /// <param name="antisymmetry">If true, then antisymmetry will be created.</param>
    /// <param name="cycles">Array of disjoint cycles.</param>
    /// <returns>An instance of <see cref="Permutation"/>.</returns>
    /// <exception cref="ArgumentException">If specified array is inconsistent with disjoint cycle notation or if antisymmetry is true and permutation order is odd.</exception>
    public static Permutation CreatePermutation(bool antisymmetry, int[][] cycles) {
        return CreatePermutation(antisymmetry, ConvertCyclesToOneLine(cycles));
    }

    /// <summary>
    /// Creates permutation instance from a given array that represents permutation in disjoint cycle notation.
    /// This method will automatically choose an appropriate underlying implementation of Permutation depending on
    /// the permutation length.
    /// </summary>
    /// <param name="cycles">Array of disjoint cycles.</param>
    /// <returns>An instance of <see cref="Permutation"/>.</returns>
    /// <exception cref="ArgumentException">If specified array is inconsistent with disjoint cycle notation.</exception>
    public static Permutation CreatePermutation(int[][] cycles) {
        return CreatePermutation(false, ConvertCyclesToOneLine(cycles));
    }

    /// <summary>
    /// Creates permutation instance from a given array that represents permutation in one-line notation.
    /// This method will automatically choose an appropriate underlying implementation of Permutation depending on
    /// the permutation length.
    /// </summary>
    /// <param name="oneLine">Array that represents permutation in one line notation.</param>
    /// <returns>An instance of <see cref="Permutation"/>.</returns>
    /// <exception cref="ArgumentException">If specified array is inconsistent with one-line notation.</exception>
    public static Permutation CreatePermutation(params int[] oneLine) {
        return CreatePermutation(false, oneLine);
    }

    /// <summary>
    /// Creates permutation instance from a given array that represents permutation in one-line notation.
    /// This method will automatically choose an appropriate underlying implementation of Permutation depending on
    /// the permutation length.
    /// If order of specified permutation is odd and antisymmetry is specified, then an exception will be thrown, since
    /// such antisymmetry is impossible from the mathematical point of view.
    /// </summary>
    /// <param name="antisymmetry">If true, then antisymmetry will be created.</param>
    /// <param name="oneLine">Array that represents permutation in one line notation.</param>
    /// <returns>An instance of <see cref="Permutation"/>.</returns>
    /// <exception cref="ArgumentException">If specified array is inconsistent with one-line notation or if antisymmetry is true and permutation order is odd.</exception>
    public static Permutation CreatePermutation(bool antisymmetry, params int[] oneLine) {
        bool _byte = true, _short = true;
        foreach (int i in oneLine) {
            if (i > short.MaxValue - 1) {  // -1 is because internalDegree calculated as largest moved point + 1
                _short = false;
                _byte = false;
            } else if (i > byte.MaxValue - 1) _byte = false;
        }
        if (_byte)
            return new PermutationOneLineByte(antisymmetry, ArraysUtils.Int2byte(oneLine));
        if (_short)
            return new PermutationOneLineShort(antisymmetry, ArraysUtils.Int2short(oneLine));
        return new PermutationOneLineInt(antisymmetry, oneLine);
    }

    /// <summary>
    /// Cached identities
    /// </summary>
    private static readonly Permutation[] CachedIdentities = new Permutation[128];

    /// <summary>
    /// Default (optimal for average problem) value of identity permutation length
    /// </summary>
    public static readonly int DefaultIdentityLength = 10;

    /// <summary>
    /// Creates identity permutation with the specified degree.
    /// </summary>
    /// <param name="degree">Degree of permutation.</param>
    /// <returns>Identity permutation.</returns>
    public static Permutation CreateIdentityPermutation(int degree) {
        if (degree < CachedIdentities.Length) {
            if (CachedIdentities[degree] == null)
                CachedIdentities[degree] = Permutations.CreatePermutation(CreateIdentityArray(degree));
            return CachedIdentities[degree];
        }
        return Permutations.CreatePermutation(CreateIdentityArray(degree));
    }

    public static int[] CreateIdentityArray(int length) {
        int[] array = new int[length];
        for (int i = 0; i < length; ++i) {
            array[i] = i;
        }
        return array;
    }

    /// <summary>
    /// Returns identity permutation.
    /// </summary>
    /// <returns>identity permutation</returns>
    public static Permutation GetIdentityPermutation() {
        return CreateIdentityPermutation(DefaultIdentityLength);
    }

    public static int[] CreateTransposition(int dimension)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Dimension is negative.");
        }

        if (dimension > 1)
        {
            return CreateTransposition(dimension, 0, 1);
        }

        return new int[dimension];
    }

    public static int[] CreateTransposition(int dimension, int position1, int position2)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Dimension is negative.");
        }

        if (position1 < 0 || position2 < 0)
        {
            throw new ArgumentException("Negative index.");
        }

        if (position1 >= dimension || position2 >= dimension)
        {
            throw new IndexOutOfRangeException();
        }

        var transposition = new int[dimension];
        for (var i = 0; i < dimension; ++i)
        {
            transposition[i] = i;
        }

        (transposition[position1], transposition[position2]) = (transposition[position2], transposition[position1]);

        return transposition;
    }

    public static int[] CreateCycle(int dimension)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Negative degree");
        }

        var cycle = new int[dimension];
        for (var i = 0; i < dimension - 1; ++i)
        {
            cycle[i + 1] = i;
        }

        cycle[0] = dimension - 1;
        return cycle;
    }

    public static int[] CreateBlockCycle(int blockSize, int numberOfBlocks)
    {
        int[] cycle = new int[blockSize * numberOfBlocks];

        int i = blockSize * (numberOfBlocks - 1) - 1;
        for (; i >= 0; --i) {
            cycle[i] = i + blockSize;
        }
        i = blockSize * (numberOfBlocks - 1);
        int k = 0;
        for (; i < cycle.Length; ++i) {
            cycle[i] = k++;
        }

        return cycle;
    }

    public static int[] CreateBlockTransposition(int length1, int length2)
    {
        var r = new int[length1 + length2];
        var i = 0;
        for (; i < length2; ++i)
        {
            r[i] = length1 + i;
        }

        for (; i < r.Length; ++i)
        {
            r[i] = i - length2;
        }

        return r;
    }

    public static int[] Inverse(int[] permutation)
    {
        var inverse = new int[permutation.Length];
        for (var i = 0; i < permutation.Length; ++i)
        {
            inverse[permutation[i]] = i;
        }
        return inverse;
    }
}