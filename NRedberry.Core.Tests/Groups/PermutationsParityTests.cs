using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsParityTests
{
    [Fact(DisplayName = "Parity should return zero for identity across array overloads")]
    public void ParityShouldReturnZeroForIdentityAcrossArrayOverloads()
    {
        int[] intIdentity = [0, 1, 2, 3, 4];
        short[] shortIdentity = [0, 1, 2, 3, 4];
        sbyte[] sbyteIdentity = [0, 1, 2, 3, 4];

        int intParity = GroupPermutations.Parity(intIdentity);
        int shortParity = GroupPermutations.Parity(shortIdentity);
        int sbyteParity = GroupPermutations.Parity(sbyteIdentity);

        Assert.Equal(0, intParity);
        Assert.Equal(0, shortParity);
        Assert.Equal(0, sbyteParity);
    }

    [Fact(DisplayName = "Parity should return one for a single transposition across array overloads")]
    public void ParityShouldReturnOneForASingleTranspositionAcrossArrayOverloads()
    {
        int[] intTransposition = [1, 0, 2, 3];
        short[] shortTransposition = [1, 0, 2, 3];
        sbyte[] sbyteTransposition = [1, 0, 2, 3];

        int intParity = GroupPermutations.Parity(intTransposition);
        int shortParity = GroupPermutations.Parity(shortTransposition);
        int sbyteParity = GroupPermutations.Parity(sbyteTransposition);

        Assert.Equal(1, intParity);
        Assert.Equal(1, shortParity);
        Assert.Equal(1, sbyteParity);
    }

    [Fact(DisplayName = "Parity should match mixed disjoint cycle parity calculation")]
    public void ParityShouldMatchMixedDisjointCycleParityCalculation()
    {
        int[] permutation = [1, 2, 0, 4, 3];

        int parity = GroupPermutations.Parity(permutation);

        Assert.Equal(1, parity);
    }

    [Fact(DisplayName = "Parity overloads should produce consistent results")]
    public void ParityOverloadsShouldProduceConsistentResults()
    {
        int[] intPermutation = [2, 0, 3, 1, 4, 5];
        short[] shortPermutation = [2, 0, 3, 1, 4, 5];
        sbyte[] sbytePermutation = [2, 0, 3, 1, 4, 5];

        int intParity = GroupPermutations.Parity(intPermutation);
        int shortParity = GroupPermutations.Parity(shortPermutation);
        int sbyteParity = GroupPermutations.Parity(sbytePermutation);

        Assert.Equal(intParity, shortParity);
        Assert.Equal(intParity, sbyteParity);
    }

    [Fact(DisplayName = "Parity should not mutate input arrays")]
    public void ParityShouldNotMutateInputArrays()
    {
        int[] intPermutation = [2, 0, 1, 4, 3];
        short[] shortPermutation = [2, 0, 1, 4, 3];
        sbyte[] sbytePermutation = [2, 0, 1, 4, 3];

        int[] intSnapshot = [.. intPermutation];
        short[] shortSnapshot = [.. shortPermutation];
        sbyte[] sbyteSnapshot = [.. sbytePermutation];

        _ = GroupPermutations.Parity(intPermutation);
        _ = GroupPermutations.Parity(shortPermutation);
        _ = GroupPermutations.Parity(sbytePermutation);

        Assert.Equal(intSnapshot, intPermutation);
        Assert.Equal(shortSnapshot, shortPermutation);
        Assert.Equal(sbyteSnapshot, sbytePermutation);
    }
}
