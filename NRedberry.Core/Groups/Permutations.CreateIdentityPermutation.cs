using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static readonly int DefaultIdentityLength = 10;

    private static readonly Permutation[] CachedIdentities = new Permutation[128];

    public static int[] CreateIdentityArray(int length)
    {
        int[] array = new int[length];
        for (int i = 0; i < length; ++i)
        {
            array[i] = i;
        }

        return array;
    }

    public static Permutation CreateIdentityPermutation(int degree)
    {
        if (degree < 0)
            throw new ArgumentException("Degree cannot be negative.", nameof(degree));

        if (degree < CachedIdentities.Length)
        {
            if (CachedIdentities[degree] == null)
            {
                CachedIdentities[degree] = CreatePermutation(CreateIdentityArray(degree));
            }

            return CachedIdentities[degree];
        }

        return CreatePermutation(CreateIdentityArray(degree));
    }

    public static Permutation GetIdentityPermutation()
    {
        return CreateIdentityPermutation(DefaultIdentityLength);
    }
}
