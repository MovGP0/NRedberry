using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreatePermutationTests
{
    [Fact(DisplayName = "Should default to symmetric permutation for params overload")]
    public void ShouldDefaultToSymmetricPermutationForParamsOverload()
    {
        Permutation permutation = GroupPermutations.CreatePermutation(1, 0, 2);

        permutation.IsAntisymmetry.ShouldBeFalse();
        permutation.OneLine().ShouldBe([1, 0, 2]);
    }

    [Fact(DisplayName = "Should preserve antisymmetry flag for bool params overload")]
    public void ShouldPreserveAntisymmetryFlagForBoolParamsOverload()
    {
        Permutation permutation = GroupPermutations.CreatePermutation(true, 1, 0, 2);

        permutation.IsAntisymmetry.ShouldBeTrue();
        permutation.OneLine().ShouldBe([1, 0, 2]);
    }

    [Fact(DisplayName = "Should create permutation from cycles overloads")]
    public void ShouldCreatePermutationFromCyclesOverloads()
    {
        int[][] cycles = [[0, 2, 1], [3, 4]];

        Permutation symmetric = GroupPermutations.CreatePermutation(cycles);
        Permutation antisymmetric = GroupPermutations.CreatePermutation(true, cycles);

        symmetric.OneLine().ShouldBe([2, 0, 1, 4, 3]);
        symmetric.IsAntisymmetry.ShouldBeFalse();
        antisymmetric.OneLine().ShouldBe(symmetric.OneLine());
        antisymmetric.IsAntisymmetry.ShouldBeTrue();
    }

    [Fact(DisplayName = "Should use byte representation within signed-byte range")]
    public void ShouldUseByteRepresentationWithinSignedByteRange()
    {
        int[] oneLine = CreateTranspositionOneLine(128);

        Permutation permutation = GroupPermutations.CreatePermutation(oneLine);

        permutation.ShouldBeOfType<PermutationOneLineByte>();
    }

    [Fact(DisplayName = "Should throw when byte-path conversion overflows signed-byte range")]
    public void ShouldThrowWhenBytePathConversionOverflowsSignedByteRange()
    {
        int[] oneLine = CreateTranspositionOneLine(255);

        Should.Throw<ArgumentException>(() => GroupPermutations.CreatePermutation(oneLine));
    }

    [Fact(DisplayName = "Should use short representation at byte threshold")]
    public void ShouldUseShortRepresentationAtByteThreshold()
    {
        int[] oneLine = CreateTranspositionOneLine(256);

        Permutation permutation = GroupPermutations.CreatePermutation(oneLine);

        permutation.ShouldBeOfType<PermutationOneLineShort>();
    }

    [Fact(DisplayName = "Should use int representation at short threshold")]
    public void ShouldUseIntRepresentationAtShortThreshold()
    {
        int[] oneLine = CreateTranspositionOneLine(short.MaxValue + 1);

        Permutation permutation = GroupPermutations.CreatePermutation(oneLine);

        permutation.ShouldBeOfType<PermutationOneLineInt>();
    }

    private static int[] CreateTranspositionOneLine(int degree)
    {
        int[] oneLine = new int[degree];
        for (int i = 0; i < degree; i++)
        {
            oneLine[i] = i;
        }

        oneLine[0] = degree - 1;
        oneLine[degree - 1] = 0;
        return oneLine;
    }
}
