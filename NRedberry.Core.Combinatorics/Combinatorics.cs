namespace NRedberry.Core.Combinatorics;

public static class Combinatorics
{
    public static IIntCombinatorialGenerator CreateIntGenerator(int n, int k)
    {
        if (n < k)
        {
            throw new ArgumentException();
        }

        return n == k
            ? new IntPermutationsGenerator(n)
            : new IntCombinationPermutationGenerator(n, k);
    }

    public static bool IsIdentity(int[] permutation)
    {
        return permutation.Select((t, i) => t == i).All(isTrue => isTrue);
    }

    public static bool IsIdentity(Permutation permutation)
    {
        return IsIdentity(permutation.GetPermutation());
    }

    public static bool IsIdentity(Symmetry symmetry)
    {
        return !symmetry.IsAntiSymmetry() && IsIdentity(symmetry.GetPermutation());
    }

    public static int[] CreateIdentity(int dimension)
    {
        return Enumerable.Range(0, dimension).ToArray();
    }

    public static int[] CreateTransposition(int dimension)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Dimension is negative.");
        }

        if (dimension <= 1)
        {
            return new int[dimension];
        }

        return CreateTransposition(dimension, 0, 1);
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

        var transposition = Enumerable.Range(0, dimension).ToArray();
        (transposition[position1], transposition[position2]) = (transposition[position2], transposition[position1]);

        return transposition;
    }

    public static int[] CreateCycle(int dimension)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Negative dimension");
        }

        var cycle = Enumerable.Range(0, dimension).ToArray();
        Array.Copy(cycle, 0, cycle, 1, dimension - 1);
        cycle[0] = dimension - 1;

        return cycle;
    }

    public static long[] Inverse(long[] permutation)
    {
        var inverse = new long[permutation.Length];
        for (var i = 0; i < permutation.Length; i++)
        {
            inverse[permutation[i]] = i;
        }

        return inverse;
    }

    public static T[] Shuffle<T>(T[] array, long[] permutation)
    {
        if (array.Length != permutation.Length)
        {
            throw new ArgumentException();
        }

        if (!TestPermutationCorrectness(permutation))
        {
            throw new ArgumentException();
        }

        var newArray = new T[array.Length];
        for (var i = 0; i < permutation.Length; i++)
        {
            newArray[i] = array[permutation[i]];
        }

        return newArray;
    }

    public static long[] Reorder(long[] array, long[] permutation)
    {
        if (array.Length != permutation.Length)
        {
            throw new ArgumentException();
        }

        if (!TestPermutationCorrectness(permutation))
        {
            throw new ArgumentException();
        }

        var newArray = new long[array.Length];
        for (var i = 0; i < permutation.Length; i++)
        {
            newArray[i] = array[permutation[i]];
        }
        return newArray;
    }

    public static bool TestPermutationCorrectness(long[] permutation)
    {
        var clone = (long[])permutation.Clone();
        Array.Sort(clone);

        for (var i = 0; i < clone.Length; i++)
        {
            if (clone[i] != i)
            {
                return false;
            }
        }

        return true;
    }

    public static bool TestPermutationCorrectness(int[] permutation)
    {
        var clone = (long[])permutation.Clone();
        Array.Sort(clone);

        for (var i = 0; i < clone.Length; i++)
        {
            if (clone[i] != i)
            {
                return false;
            }
        }

        return true;
    }

    private static void RangeCheck(int arrayLen, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex)
        {
            throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})");
        }

        if (fromIndex < 0)
        {
            throw new IndexOutOfRangeException(fromIndex.ToString());
        }

        if (toIndex > arrayLen)
        {
            throw new IndexOutOfRangeException(toIndex.ToString());
        }
    }

    private static void RangeCheck1(int dimension, params int[] positions)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Dimension is negative.");
        }

        foreach (var i in positions)
        {
            if (i < 0)
            {
                throw new ArgumentException($"Negative index {i}.");
            }

            if (i >= dimension)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}