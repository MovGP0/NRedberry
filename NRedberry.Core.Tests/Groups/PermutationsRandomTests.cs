using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsRandomTests
{
    [Fact(DisplayName = "RandomPermutation should be deterministic for equal seeds")]
    public void RandomPermutationShouldBeDeterministicForEqualSeeds()
    {
        var leftRandom = new Random(12345);
        var rightRandom = new Random(12345);

        int[] left = GroupPermutations.RandomPermutation(16, leftRandom);
        int[] right = GroupPermutations.RandomPermutation(16, rightRandom);

        Assert.Equal(left, right);
    }

    [Fact(DisplayName = "RandomPermutation should throw for negative n")]
    public void RandomPermutationShouldThrowForNegativeN()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupPermutations.RandomPermutation(-1, new Random(1)));
    }

    [Fact(DisplayName = "RandomPermutation should return valid one-line notation")]
    public void RandomPermutationShouldReturnValidOneLineNotation()
    {
        int[] permutation = GroupPermutations.RandomPermutation(32, new Random(7));

        Assert.Equal(32, permutation.Length);
        Assert.True(GroupPermutations.TestPermutationCorrectness(permutation));
    }

    [Fact(DisplayName = "Shuffle int[] should be deterministic and preserve multiset")]
    public void ShuffleIntArrayShouldBeDeterministicAndPreserveMultiset()
    {
        int[] left = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        int[] right = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        int[] original = [.. left];

        GroupPermutations.Shuffle(left, new Random(1337));
        GroupPermutations.Shuffle(right, new Random(1337));

        Assert.Equal(left, right);

        Array.Sort(left);
        Array.Sort(original);
        Assert.Equal(original, left);
    }

    [Fact(DisplayName = "Shuffle object[] should be deterministic and preserve multiset")]
    public void ShuffleObjectArrayShouldBeDeterministicAndPreserveMultiset()
    {
        object[] left = ["a", "b", "c", "d", "e", "f"];
        object[] right = ["a", "b", "c", "d", "e", "f"];
        object[] original = [.. left];

        GroupPermutations.Shuffle(left, new Random(9001));
        GroupPermutations.Shuffle(right, new Random(9001));

        Assert.Equal(left, right);

        Array.Sort(left);
        Array.Sort(original);
        Assert.Equal(original, left);
    }
}
