using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsOrderOfPermutationIsOddTests
{
    [Fact(DisplayName = "OrderOfPermutationIsOdd should return true for empty and identity permutations")]
    public void OrderOfPermutationIsOddShouldReturnTrueForEmptyAndIdentityPermutations()
    {
        int[] emptyIntPermutation = [];
        short[] emptyShortPermutation = [];
        sbyte[] emptySbytePermutation = [];

        int[] identityIntPermutation = [0, 1, 2, 3, 4];
        short[] identityShortPermutation = [0, 1, 2, 3, 4];
        sbyte[] identitySbytePermutation = [0, 1, 2, 3, 4];

        GroupPermutations.OrderOfPermutationIsOdd(emptyIntPermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(emptyShortPermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(emptySbytePermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(identityIntPermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(identityShortPermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(identitySbytePermutation).ShouldBeTrue();
    }

    [Fact(DisplayName = "OrderOfPermutationIsOdd should return true when all cycles are odd length")]
    public void OrderOfPermutationIsOddShouldReturnTrueWhenAllCyclesAreOddLength()
    {
        int[] intPermutation = [1, 2, 0, 4, 5, 3, 6];
        short[] shortPermutation = [1, 2, 0, 4, 5, 3, 6];
        sbyte[] sbytePermutation = [1, 2, 0, 4, 5, 3, 6];

        GroupPermutations.OrderOfPermutationIsOdd(intPermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(shortPermutation).ShouldBeTrue();
        GroupPermutations.OrderOfPermutationIsOdd(sbytePermutation).ShouldBeTrue();
    }

    [Fact(DisplayName = "OrderOfPermutationIsOdd should return false when any cycle has even length")]
    public void OrderOfPermutationIsOddShouldReturnFalseWhenAnyCycleHasEvenLength()
    {
        int[] intPermutation = [1, 0, 3, 2, 4];
        short[] shortPermutation = [1, 0, 3, 2, 4];
        sbyte[] sbytePermutation = [1, 0, 3, 2, 4];

        GroupPermutations.OrderOfPermutationIsOdd(intPermutation).ShouldBeFalse();
        GroupPermutations.OrderOfPermutationIsOdd(shortPermutation).ShouldBeFalse();
        GroupPermutations.OrderOfPermutationIsOdd(sbytePermutation).ShouldBeFalse();
    }

    [Fact(DisplayName = "OrderOfPermutationIsOdd should not mutate input arrays")]
    public void OrderOfPermutationIsOddShouldNotMutateInputArrays()
    {
        int[] intPermutation = [2, 0, 1, 4, 3];
        short[] shortPermutation = [2, 0, 1, 4, 3];
        sbyte[] sbytePermutation = [2, 0, 1, 4, 3];

        int[] intSnapshot = [.. intPermutation];
        short[] shortSnapshot = [.. shortPermutation];
        sbyte[] sbyteSnapshot = [.. sbytePermutation];

        _ = GroupPermutations.OrderOfPermutationIsOdd(intPermutation);
        _ = GroupPermutations.OrderOfPermutationIsOdd(shortPermutation);
        _ = GroupPermutations.OrderOfPermutationIsOdd(sbytePermutation);

        intPermutation.ShouldBe(intSnapshot);
        shortPermutation.ShouldBe(shortSnapshot);
        sbytePermutation.ShouldBe(sbyteSnapshot);
    }
}
