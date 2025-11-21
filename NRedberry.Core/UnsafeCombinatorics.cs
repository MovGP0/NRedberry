using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

namespace NRedberry;

public static class UnsafeCombinatorics
{
    public static Permutation CreateUnsafe(int[] permutation)
    {
        ArgumentNullException.ThrowIfNull(permutation);

        bool isIdentity = true;
        int internalDegree = -1;
        for (int i = 0; i < permutation.Length; ++i)
        {
            isIdentity &= permutation[i] == i;
            internalDegree = Math.Max(internalDegree, Math.Max(i, permutation[i]));
        }

        return new PermutationOneLineInt(isIdentity, false, internalDegree + 1, permutation, true);
    }

    public static Symmetry CreateUnsafe(int[] permutation, bool sign)
    {
        return new Symmetry(permutation, sign, true);
    }
}
