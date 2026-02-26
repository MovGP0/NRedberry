using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupOrbitsTests
{
    [Fact(DisplayName = "Should return orbit and size for point in transitive group")]
    public void ShouldReturnOrbitAndSizeForPointInTransitiveGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        // Act
        int[] orbit = group.Orbit(2);
        Array.Sort(orbit);
        int orbitSize = group.OrbitSize(2);

        // Assert
        Assert.Equal([0, 1, 2, 3], orbit);
        Assert.Equal(4, orbitSize);
    }

    [Fact(DisplayName = "Should return empty orbit and zero size for point outside degree")]
    public void ShouldReturnEmptyOrbitAndZeroSizeForPointOutsideDegree()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act
        int[] orbit = group.Orbit(10);
        int orbitSize = group.OrbitSize(10);

        // Assert
        Assert.Empty(orbit);
        Assert.Equal(0, orbitSize);
    }

    [Fact(DisplayName = "Should return deduplicated orbit for multiple points and skip out-of-range points")]
    public void ShouldReturnDeduplicatedOrbitForMultiplePointsAndSkipOutOfRangePoints()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act
        int[] orbit = group.Orbit(0, 2, 2, 10);
        Array.Sort(orbit);

        // Assert
        Assert.Equal([0, 2], orbit);
    }

    [Fact(DisplayName = "Should return positions in orbits as copy")]
    public void ShouldReturnPositionsInOrbitsAsCopy()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act
        int[] positions = group.PositionsInOrbits;
        positions[0] = 99;
        int[] freshPositions = group.PositionsInOrbits;

        // Assert
        Assert.Equal([0, 1, 2], freshPositions);
    }

    [Fact(DisplayName = "Should return all orbits as deep copy")]
    public void ShouldReturnAllOrbitsAsDeepCopy()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act
        int[][] orbits = group.Orbits();
        orbits[0][0] = 99;
        int[] orbitZero = group.Orbit(0);

        // Assert
        Assert.Equal([0], orbitZero);
    }

    [Fact(DisplayName = "Should return orbit index for point and minus one outside degree")]
    public void ShouldReturnOrbitIndexForPointAndMinusOneOutsideDegree()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act
        int indexInside = group.IndexOfOrbit(2);
        int indexOutside = group.IndexOfOrbit(10);

        // Assert
        Assert.Equal(2, indexInside);
        Assert.Equal(-1, indexOutside);
    }
}
