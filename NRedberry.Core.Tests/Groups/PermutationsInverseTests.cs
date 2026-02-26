using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsInverseTests
{
    [Fact(DisplayName = "Inverse should return empty array for empty permutation")]
    public void InverseShouldReturnEmptyArrayForEmptyPermutation()
    {
        int[] permutation = [];

        int[] inverse = GroupPermutations.Inverse(permutation);

        Assert.Empty(inverse);
    }

    [Fact(DisplayName = "Inverse should return identity for identity permutation")]
    public void InverseShouldReturnIdentityForIdentityPermutation()
    {
        int[] permutation = [0, 1, 2, 3];

        int[] inverse = GroupPermutations.Inverse(permutation);

        Assert.Equal(new[] { 0, 1, 2, 3 }, inverse);
    }

    [Fact(DisplayName = "Inverse should compute nontrivial inverse mapping")]
    public void InverseShouldComputeNontrivialInverseMapping()
    {
        int[] permutation = [2, 4, 0, 1, 3];

        int[] inverse = GroupPermutations.Inverse(permutation);

        Assert.Equal(new[] { 2, 3, 0, 4, 1 }, inverse);
    }

    [Fact(DisplayName = "Inverse should be involutive for valid permutations")]
    public void InverseShouldBeInvolutiveForValidPermutations()
    {
        int[] permutation = [3, 0, 4, 1, 2];

        int[] inverse = GroupPermutations.Inverse(permutation);
        int[] roundTrip = GroupPermutations.Inverse(inverse);

        Assert.Equal(permutation, roundTrip);
    }

    [Fact(DisplayName = "Inverse should not mutate input permutation")]
    public void InverseShouldNotMutateInputPermutation()
    {
        int[] permutation = [1, 3, 0, 2];
        int[] snapshot = [.. permutation];

        _ = GroupPermutations.Inverse(permutation);

        Assert.Equal(snapshot, permutation);
    }
}
