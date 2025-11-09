using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int InternalDegree(int[] permutation)
    {
        int result = -1;
        foreach (int i in permutation)
        {
            result = Math.Max(result, i);
        }

        return result + 1;
    }

    public static short InternalDegree(short[] permutation)
    {
        short result = -1;
        foreach (short i in permutation)
        {
            if (i > result)
                result = i;
        }

        return (short)(result + 1);
    }

    public static sbyte InternalDegree(sbyte[] permutation)
    {
        sbyte result = -1;
        foreach (sbyte i in permutation)
        {
            if (i > result)
                result = i;
        }

        return (sbyte)(result + 1);
    }

    public static int InternalDegree<T>(List<T> permutations) where T : Permutation
    {
        int max = 0;
        foreach (T permutation in permutations)
            max = Math.Max(max, permutation.Degree);
        return max;
    }

    public static int InternalDegree<T>(IReadOnlyCollection<T> permutations) where T : Permutation
    {
        int max = 0;
        foreach (T permutation in permutations)
            max = Math.Max(max, permutation.Degree);
        return max;
    }
}
