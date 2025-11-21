using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

public static partial class Permutations
{
    public static Permutation CreatePermutation(bool antisymmetry, int[][] cycles)
    {
        return CreatePermutation(antisymmetry, ConvertCyclesToOneLine(cycles));
    }

    public static Permutation CreatePermutation(int[][] cycles)
    {
        return CreatePermutation(false, ConvertCyclesToOneLine(cycles));
    }

    public static Permutation CreatePermutation(params int[] oneLine)
    {
        return CreatePermutation(false, oneLine);
    }

    public static Permutation CreatePermutation(bool antisymmetry, params int[] oneLine)
    {
        bool useByte = true;
        bool useShort = true;
        foreach (int i in oneLine)
        {
            if (i > short.MaxValue - 1)
            {
                useShort = false;
                useByte = false;
                break;
            }

            if (i > byte.MaxValue - 1)
            {
                useByte = false;
            }
        }

        if (useByte)
            return new PermutationOneLineByte(antisymmetry, ArraysUtils.Int2byte(oneLine));

        if (useShort)
            return new PermutationOneLineShort(antisymmetry, ArraysUtils.Int2short(oneLine));

        return new PermutationOneLineInt(antisymmetry, oneLine);
    }
}
