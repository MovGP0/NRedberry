namespace NRedberry.Core.Combinatorics;

public static class UnsafeCombinatorics
{
    public static Permutation CreateUnsafe(int[] permutation)
    {
        return new Permutation(permutation, true);
    }

    public static Symmetry CreateUnsafe(int[] permutation, bool sign)
    {
        return new Symmetry(permutation, sign, true);
    }
}
