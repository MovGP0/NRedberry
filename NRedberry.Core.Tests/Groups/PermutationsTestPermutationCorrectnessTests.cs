using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsTestPermutationCorrectnessTests
{
    [Fact(DisplayName = "TestPermutationCorrectness without sign should return true for valid permutations across array overloads")]
    public void TestPermutationCorrectnessWithoutSignShouldReturnTrueForValidPermutationsAcrossArrayOverloads()
    {
        int[] intPermutation = [2, 0, 1, 4, 3];
        short[] shortPermutation = [2, 0, 1, 4, 3];
        sbyte[] sbytePermutation = [2, 0, 1, 4, 3];

        Assert.True(GroupPermutations.TestPermutationCorrectness(intPermutation));
        Assert.True(GroupPermutations.TestPermutationCorrectness(shortPermutation));
        Assert.True(GroupPermutations.TestPermutationCorrectness(sbytePermutation));
    }

    [Fact(DisplayName = "TestPermutationCorrectness without sign should return false for duplicate values across array overloads")]
    public void TestPermutationCorrectnessWithoutSignShouldReturnFalseForDuplicateValuesAcrossArrayOverloads()
    {
        int[] intPermutation = [1, 1, 0, 3];
        short[] shortPermutation = [1, 1, 0, 3];
        sbyte[] sbytePermutation = [1, 1, 0, 3];

        Assert.False(GroupPermutations.TestPermutationCorrectness(intPermutation));
        Assert.False(GroupPermutations.TestPermutationCorrectness(shortPermutation));
        Assert.False(GroupPermutations.TestPermutationCorrectness(sbytePermutation));
    }

    [Fact(DisplayName = "TestPermutationCorrectness without sign should return false for out of range values across array overloads")]
    public void TestPermutationCorrectnessWithoutSignShouldReturnFalseForOutOfRangeValuesAcrossArrayOverloads()
    {
        int[] intPermutation = [0, 4, 1, 2];
        short[] shortPermutation = [0, 4, 1, 2];
        sbyte[] sbytePermutation = [0, 4, 1, 2];

        Assert.False(GroupPermutations.TestPermutationCorrectness(intPermutation));
        Assert.False(GroupPermutations.TestPermutationCorrectness(shortPermutation));
        Assert.False(GroupPermutations.TestPermutationCorrectness(sbytePermutation));
    }

    [Fact(DisplayName = "TestPermutationCorrectness with sign should require even order across array overloads")]
    public void TestPermutationCorrectnessWithSignShouldRequireEvenOrderAcrossArrayOverloads()
    {
        int[] evenOrderIntPermutation = [1, 0, 2, 3];
        short[] evenOrderShortPermutation = [1, 0, 2, 3];
        sbyte[] evenOrderSbytePermutation = [1, 0, 2, 3];

        int[] oddOrderIntPermutation = [1, 2, 0, 3];
        short[] oddOrderShortPermutation = [1, 2, 0, 3];
        sbyte[] oddOrderSbytePermutation = [1, 2, 0, 3];

        Assert.True(GroupPermutations.TestPermutationCorrectness(evenOrderIntPermutation, true));
        Assert.True(GroupPermutations.TestPermutationCorrectness(evenOrderShortPermutation, true));
        Assert.True(GroupPermutations.TestPermutationCorrectness(evenOrderSbytePermutation, true));

        Assert.False(GroupPermutations.TestPermutationCorrectness(oddOrderIntPermutation, true));
        Assert.False(GroupPermutations.TestPermutationCorrectness(oddOrderShortPermutation, true));
        Assert.False(GroupPermutations.TestPermutationCorrectness(oddOrderSbytePermutation, true));

        Assert.True(GroupPermutations.TestPermutationCorrectness(oddOrderIntPermutation, false));
        Assert.True(GroupPermutations.TestPermutationCorrectness(oddOrderShortPermutation, false));
        Assert.True(GroupPermutations.TestPermutationCorrectness(oddOrderSbytePermutation, false));
    }
}
