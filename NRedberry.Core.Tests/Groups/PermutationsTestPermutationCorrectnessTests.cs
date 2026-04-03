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

        GroupPermutations.TestPermutationCorrectness(intPermutation).ShouldBeTrue();
        GroupPermutations.TestPermutationCorrectness(shortPermutation).ShouldBeTrue();
        GroupPermutations.TestPermutationCorrectness(sbytePermutation).ShouldBeTrue();
    }

    [Fact(DisplayName = "TestPermutationCorrectness without sign should return false for duplicate values across array overloads")]
    public void TestPermutationCorrectnessWithoutSignShouldReturnFalseForDuplicateValuesAcrossArrayOverloads()
    {
        int[] intPermutation = [1, 1, 0, 3];
        short[] shortPermutation = [1, 1, 0, 3];
        sbyte[] sbytePermutation = [1, 1, 0, 3];

        GroupPermutations.TestPermutationCorrectness(intPermutation).ShouldBeFalse();
        GroupPermutations.TestPermutationCorrectness(shortPermutation).ShouldBeFalse();
        GroupPermutations.TestPermutationCorrectness(sbytePermutation).ShouldBeFalse();
    }

    [Fact(DisplayName = "TestPermutationCorrectness without sign should return false for out of range values across array overloads")]
    public void TestPermutationCorrectnessWithoutSignShouldReturnFalseForOutOfRangeValuesAcrossArrayOverloads()
    {
        int[] intPermutation = [0, 4, 1, 2];
        short[] shortPermutation = [0, 4, 1, 2];
        sbyte[] sbytePermutation = [0, 4, 1, 2];

        GroupPermutations.TestPermutationCorrectness(intPermutation).ShouldBeFalse();
        GroupPermutations.TestPermutationCorrectness(shortPermutation).ShouldBeFalse();
        GroupPermutations.TestPermutationCorrectness(sbytePermutation).ShouldBeFalse();
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

        GroupPermutations.TestPermutationCorrectness(evenOrderIntPermutation, true).ShouldBeTrue();
        GroupPermutations.TestPermutationCorrectness(evenOrderShortPermutation, true).ShouldBeTrue();
        GroupPermutations.TestPermutationCorrectness(evenOrderSbytePermutation, true).ShouldBeTrue();

        GroupPermutations.TestPermutationCorrectness(oddOrderIntPermutation, true).ShouldBeFalse();
        GroupPermutations.TestPermutationCorrectness(oddOrderShortPermutation, true).ShouldBeFalse();
        GroupPermutations.TestPermutationCorrectness(oddOrderSbytePermutation, true).ShouldBeFalse();

        GroupPermutations.TestPermutationCorrectness(oddOrderIntPermutation, false).ShouldBeTrue();
        GroupPermutations.TestPermutationCorrectness(oddOrderShortPermutation, false).ShouldBeTrue();
        GroupPermutations.TestPermutationCorrectness(oddOrderSbytePermutation, false).ShouldBeTrue();
    }
}
