using System.Numerics;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsOrderOfPermutationTests
{
    [Fact(DisplayName = "OrderOfPermutation should return one for empty permutation")]
    public void OrderOfPermutationShouldReturnOneForEmptyPermutation()
    {
        int[] permutation = [];

        BigInteger order = GroupPermutations.OrderOfPermutation(permutation);

        order.ShouldBe(BigInteger.One);
    }

    [Fact(DisplayName = "OrderOfPermutation should return one for identity permutation")]
    public void OrderOfPermutationShouldReturnOneForIdentityPermutation()
    {
        int[] permutation = [0, 1, 2, 3, 4];

        BigInteger order = GroupPermutations.OrderOfPermutation(permutation);

        order.ShouldBe(BigInteger.One);
    }

    [Fact(DisplayName = "OrderOfPermutation should compute lcm of disjoint cycle lengths")]
    public void OrderOfPermutationShouldComputeLcmOfDisjointCycleLengths()
    {
        int[] permutation = [1, 2, 0, 4, 3, 6, 7, 5];

        BigInteger order = GroupPermutations.OrderOfPermutation(permutation);

        order.ShouldBe(new BigInteger(6));
    }

    [Fact(DisplayName = "OrderOfPermutation should ignore fixed points in lcm")]
    public void OrderOfPermutationShouldIgnoreFixedPointsInLcm()
    {
        int[] permutation = [2, 1, 0, 3, 4, 5];

        BigInteger order = GroupPermutations.OrderOfPermutation(permutation);

        order.ShouldBe(new BigInteger(2));
    }

    [Fact(DisplayName = "OrderOfPermutation should support short and sbyte overloads")]
    public void OrderOfPermutationShouldSupportShortAndSbyteOverloads()
    {
        short[] shortPermutation = [1, 2, 0, 4, 3];
        sbyte[] sbytePermutation = [1, 2, 0, 4, 3];

        BigInteger shortOrder = GroupPermutations.OrderOfPermutation(shortPermutation);
        BigInteger sbyteOrder = GroupPermutations.OrderOfPermutation(sbytePermutation);

        shortOrder.ShouldBe(new BigInteger(6));
        sbyteOrder.ShouldBe(new BigInteger(6));
    }

    [Fact(DisplayName = "OrderOfPermutation should not mutate input arrays")]
    public void OrderOfPermutationShouldNotMutateInputArrays()
    {
        int[] intPermutation = [1, 0, 2, 4, 3];
        short[] shortPermutation = [1, 0, 2, 4, 3];
        sbyte[] sbytePermutation = [1, 0, 2, 4, 3];

        int[] intSnapshot = [.. intPermutation];
        short[] shortSnapshot = [.. shortPermutation];
        sbyte[] sbyteSnapshot = [.. sbytePermutation];

        _ = GroupPermutations.OrderOfPermutation(intPermutation);
        _ = GroupPermutations.OrderOfPermutation(shortPermutation);
        _ = GroupPermutations.OrderOfPermutation(sbytePermutation);

        intPermutation.ShouldBe(intSnapshot);
        shortPermutation.ShouldBe(shortSnapshot);
        sbytePermutation.ShouldBe(sbyteSnapshot);
    }
}
