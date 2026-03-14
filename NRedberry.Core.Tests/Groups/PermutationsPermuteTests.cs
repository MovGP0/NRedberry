using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsPermuteTests
{
    [Fact(DisplayName = "Permute int[] should reorder according to one-line permutation")]
    public void PermuteIntArrayShouldReorderAccordingToOneLinePermutation()
    {
        int[] values = [10, 20, 30, 40];
        int[] permutation = [2, 0, 3, 1];

        int[] permuted = GroupPermutations.Permute(values, permutation);

        permuted.ShouldBe([30, 10, 40, 20]);
    }

    [Fact(DisplayName = "Permute T[] should reorder according to one-line permutation")]
    public void PermuteGenericArrayShouldReorderAccordingToOneLinePermutation()
    {
        string[] values = ["a", "b", "c", "d"];
        int[] permutation = [3, 1, 0, 2];

        string[] permuted = GroupPermutations.Permute(values, permutation);

        permuted.ShouldBe(["d", "b", "a", "c"]);
    }

    [Fact(DisplayName = "Permute List<T> should return reordered list and keep input unchanged")]
    public void PermuteListShouldReturnReorderedListAndKeepInputUnchanged()
    {
        List<int> values = [10, 20, 30, 40];
        int[] permutation = [1, 3, 0, 2];
        List<int> snapshot = [.. values];

        List<int> permuted = GroupPermutations.Permute(values, permutation);

        permuted.ShouldBe([20, 40, 10, 30]);
        values.ShouldBe(snapshot);
        permuted.ShouldNotBeSameAs(values);
    }

    [Fact(DisplayName = "Permute int[] should throw for mismatched lengths")]
    public void PermuteIntArrayShouldThrowForMismatchedLengths()
    {
        int[] values = [10, 20, 30];
        int[] permutation = [0, 1];

        Should.Throw<ArgumentException>(() => GroupPermutations.Permute(values, permutation));
    }

    [Fact(DisplayName = "Permute T[] should throw for mismatched lengths")]
    public void PermuteGenericArrayShouldThrowForMismatchedLengths()
    {
        string[] values = ["a", "b", "c"];
        int[] permutation = [0, 1];

        Should.Throw<ArgumentException>(() => GroupPermutations.Permute(values, permutation));
    }

    [Fact(DisplayName = "Permute List<T> should throw for mismatched lengths")]
    public void PermuteListShouldThrowForMismatchedLengths()
    {
        List<int> values = [10, 20, 30];
        int[] permutation = [0, 1];

        Should.Throw<ArgumentException>(() => GroupPermutations.Permute(values, permutation));
    }

    [Fact(DisplayName = "Permute int[] should throw for invalid one-line permutation")]
    public void PermuteIntArrayShouldThrowForInvalidOneLinePermutation()
    {
        int[] values = [10, 20, 30];
        int[] permutation = [0, 1, 1];

        Should.Throw<ArgumentException>(() => GroupPermutations.Permute(values, permutation));
    }

    [Fact(DisplayName = "Permute T[] should throw for invalid one-line permutation")]
    public void PermuteGenericArrayShouldThrowForInvalidOneLinePermutation()
    {
        string[] values = ["a", "b", "c"];
        int[] permutation = [0, 3, 1];

        Should.Throw<ArgumentException>(() => GroupPermutations.Permute(values, permutation));
    }
}
