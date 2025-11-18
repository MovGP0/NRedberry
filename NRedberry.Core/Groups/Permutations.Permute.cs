namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int[] Permute(int[] array, int[] permutation)
    {
        if (array.Length != permutation.Length)
            throw new ArgumentException("Array and permutation must have equal lengths.");

        if (!TestPermutationCorrectness(permutation))
            throw new ArgumentException("Permutation must be valid one-line notation.");

        int[] result = new int[array.Length];
        for (int i = 0; i < permutation.Length; ++i)
            result[i] = array[permutation[i]];
        return result;
    }

    public static T[] Permute<T>(T[] array, int[] permutation)
    {
        if (array.Length != permutation.Length)
            throw new ArgumentException("Array and permutation must have equal lengths.");

        if (!TestPermutationCorrectness(permutation))
            throw new ArgumentException("Permutation must be valid one-line notation.");

        T[] result = new T[array.Length];
        for (int i = 0; i < permutation.Length; ++i)
            result[i] = array[permutation[i]];
        return result;
    }

    public static List<T> Permute<T>(List<T> array, int[] permutation)
    {
        if (array.Count != permutation.Length)
            throw new ArgumentException("Array and permutation must have equal lengths.");

        var result = new List<T>(array.Count);
        for (int i = 0; i < permutation.Length; ++i)
            result.Add(array[permutation[i]]);
        return result;
    }
}
