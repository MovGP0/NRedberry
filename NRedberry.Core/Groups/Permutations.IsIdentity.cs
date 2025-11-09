namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static bool IsIdentity(int[] permutation)
    {
        for (int i = 0; i < permutation.Length; ++i)
        {
            if (permutation[i] != i)
                return false;
        }

        return true;
    }

    public static bool IsIdentity(short[] permutation)
    {
        for (int i = 0; i < permutation.Length; ++i)
        {
            if (permutation[i] != i)
                return false;
        }

        return true;
    }

    public static bool IsIdentity(sbyte[] permutation)
    {
        for (int i = 0; i < permutation.Length; ++i)
        {
            if (permutation[i] != i)
                return false;
        }

        return true;
    }
}
