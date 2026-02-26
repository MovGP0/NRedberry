using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class RandomPermutationTests
{
    [Fact(DisplayName = "Random should throw when generator list size is less than 3")]
    public void RandomShouldThrowWhenGeneratorListSizeIsLessThanThree()
    {
        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(1, 0, 2),
            GroupPermutations.CreatePermutation(2, 0, 1)
        ];

        Assert.Throws<ArgumentException>(() => RandomPermutation.Random(generators, new Random(1)));
    }

    [Fact(DisplayName = "Randomness should throw when generators count and extend size are both less than 2")]
    public void RandomnessShouldThrowWhenGeneratorsCountAndExtendSizeAreBothLessThanTwo()
    {
        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(1, 0, 2)
        ];

        Assert.Throws<ArgumentException>(() => RandomPermutation.Randomness(generators, 1, 0, new Random(7)));
    }

    [Fact(DisplayName = "Randomness should extend list and append identity when needed")]
    public void RandomnessShouldExtendListAndAppendIdentityWhenNeeded()
    {
        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(1, 0, 2),
            GroupPermutations.CreatePermutation(2, 0, 1)
        ];

        RandomPermutation.Randomness(generators, 5, 0, new Random(3));

        Assert.True(generators.Count >= 6);
        Assert.True(generators[^1].IsIdentity);
    }

    [Fact(DisplayName = "Randomness should keep generators count at least requested size and last entry identity")]
    public void RandomnessShouldKeepGeneratorsCountAtLeastRequestedSizeAndLastEntryIdentity()
    {
        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(1, 0, 2),
            GroupPermutations.CreatePermutation(2, 0, 1),
            GroupPermutations.CreatePermutation(0, 2, 1)
        ];

        RandomPermutation.Randomness(generators, 3, 0, new Random(11));

        Assert.True(generators.Count >= 3);
        Assert.True(generators[^1].IsIdentity);
    }

    [Fact(DisplayName = "Random should return permutation and mutate deterministically for same seeded setup")]
    public void RandomShouldReturnPermutationAndMutateDeterministicallyForSameSeededSetup()
    {
        List<Permutation> baseline =
        [
            GroupPermutations.CreatePermutation(1, 0, 2),
            GroupPermutations.CreatePermutation(2, 0, 1),
            GroupPermutations.CreateIdentityPermutation(3)
        ];

        List<Permutation> left = ClonePermutationList(baseline);
        List<Permutation> right = ClonePermutationList(baseline);

        Permutation leftResult = RandomPermutation.Random(left, new Random(123456));
        Permutation rightResult = RandomPermutation.Random(right, new Random(123456));

        Assert.NotNull(leftResult);
        Assert.NotNull(rightResult);
        AssertPermutationEqual(leftResult, rightResult);

        AssertPermutationListsEqual(left, right);
        Assert.False(PermutationListsEqual(left, baseline));
    }

    private static List<Permutation> ClonePermutationList(IList<Permutation> source)
    {
        return source
            .Select(p => GroupPermutations.CreatePermutation(p.IsAntisymmetry, p.OneLine()))
            .ToList();
    }

    private static bool PermutationListsEqual(IList<Permutation> left, IList<Permutation> right)
    {
        if (left.Count != right.Count)
        {
            return false;
        }

        for (int i = 0; i < left.Count; i++)
        {
            if (left[i].IsAntisymmetry != right[i].IsAntisymmetry)
            {
                return false;
            }

            if (!left[i].OneLine().SequenceEqual(right[i].OneLine()))
            {
                return false;
            }
        }

        return true;
    }

    private static void AssertPermutationListsEqual(IList<Permutation> left, IList<Permutation> right)
    {
        Assert.True(PermutationListsEqual(left, right));
    }

    private static void AssertPermutationEqual(Permutation left, Permutation right)
    {
        Assert.Equal(left.IsAntisymmetry, right.IsAntisymmetry);
        Assert.Equal(left.OneLine(), right.OneLine());
    }
}
