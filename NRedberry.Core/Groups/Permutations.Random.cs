using NRedberry.Contexts;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

public static partial class Permutations
{
    public static int[] RandomPermutation(int n, Random random)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Dimension cannot be negative.");

        int[] p = new int[n];
        for (int i = 0; i < n; ++i)
            p[i] = i;
        for (int i = n; i > 1; --i)
            ArraysUtils.Swap(p, i - 1, random.Next(i));
        return p;
    }

    public static int[] RandomPermutation(int n)
    {
        return RandomPermutation(n, GetRandomSource());
    }

    public static void Shuffle(int[] a)
    {
        Shuffle(a, GetRandomSource());
    }

    public static void Shuffle(int[] a, Random random)
    {
        for (int i = a.Length; i > 1; --i)
            ArraysUtils.Swap(a, i - 1, random.Next(i));
    }

    public static void Shuffle(object[] a, Random random)
    {
        for (int i = a.Length; i > 1; --i)
            ArraysUtils.Swap(a, i - 1, random.Next(i));
    }

    public static void Shuffle(object[] a)
    {
        Shuffle(a, GetRandomSource());
    }

    private static Random GetRandomSource()
    {
        return Context.Get().NameManager.GetRandomGenerator();
    }
}
