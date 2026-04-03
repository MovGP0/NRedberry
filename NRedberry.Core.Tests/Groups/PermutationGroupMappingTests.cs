using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupMappingTests
{
    [Fact(DisplayName = "Should find mapping for points in same orbit")]
    public void ShouldFindMappingForPointsInSameOrbit()
    {
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        Permutation? mapping = group.Mapping(0, 1);

        mapping.ShouldNotBeNull();
        mapping.NewIndexOf(0).ShouldBe(1);
    }

    [Fact(DisplayName = "Should throw for mismatched mapping arrays")]
    public void ShouldThrowForMismatchedMappingArrays()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Should.Throw<ArgumentException>(() => group.Mapping([0], [0, 1]));
    }

    [Fact(DisplayName = "Should return mapping search for arrays")]
    public void ShouldReturnMappingSearchForArrays()
    {
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        BacktrackSearch mappingSearch = group.Mapping([0], [1]);

        mappingSearch.ShouldNotBeNull();
    }
}
