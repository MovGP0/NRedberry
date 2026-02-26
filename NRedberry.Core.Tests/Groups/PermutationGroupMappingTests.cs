using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupMappingTests
{
    [Fact(DisplayName = "Should find mapping for points in same orbit")]
    public void ShouldFindMappingForPointsInSameOrbit()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = group.Mapping(0, 1));
    }

    [Fact(DisplayName = "Should throw for mismatched mapping arrays")]
    public void ShouldThrowForMismatchedMappingArrays()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Assert.Throws<ArgumentException>(() => group.Mapping([0], [0, 1]));
    }

    [Fact(DisplayName = "Should return mapping search for arrays")]
    public void ShouldReturnMappingSearchForArrays()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = group.Mapping([0], [1]));
    }
}
