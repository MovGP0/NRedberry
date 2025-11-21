namespace NRedberry.Groups;

public static partial class Permutations
{
    public static int[] Inverse(int[] permutation)
    {
        var inverse = new int[permutation.Length];
        for (int i = 0; i < permutation.Length; ++i)
            inverse[permutation[i]] = i;
        return inverse;
    }
}
