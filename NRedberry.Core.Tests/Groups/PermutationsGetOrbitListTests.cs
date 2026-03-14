using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsGetOrbitListTests
{
    [Fact(DisplayName = "Should return singleton orbit for empty generators")]
    public void ShouldReturnSingletonOrbitForEmptyGenerators()
    {
        // Act
        List<int> orbit = GroupPermutations.GetOrbitList([], point: 2, degree: 5);

        // Assert
        Assert.Equal([2], orbit);
    }

    [Fact(DisplayName = "Should treat null generators as empty and return singleton orbit")]
    public void ShouldTreatNullGeneratorsAsEmptyAndReturnSingletonOrbit()
    {
        // Act
        List<int> orbit = GroupPermutations.GetOrbitList(null!, point: 1, degree: 4);

        // Assert
        Assert.Equal([1], orbit);
    }

    [Fact(DisplayName = "Should expand orbit transitively with multiple generators")]
    public void ShouldExpandOrbitTransitivelyWithMultipleGenerators()
    {
        // Arrange
        Permutation cycle012 = GroupPermutations.CreatePermutation(1, 2, 0, 3);
        Permutation transposition23 = GroupPermutations.CreatePermutation(0, 1, 3, 2);

        // Act
        List<int> orbit = GroupPermutations.GetOrbitList([cycle012, transposition23], point: 0, degree: 4);

        // Assert
        Assert.Equal([0, 1, 2, 3], orbit);
    }

    [Fact(DisplayName = "Should avoid duplicates when generators overlap")]
    public void ShouldAvoidDuplicatesWhenGeneratorsOverlap()
    {
        // Arrange
        Permutation swap01 = GroupPermutations.CreatePermutation(1, 0, 2);
        Permutation identity = GroupPermutations.CreateIdentityPermutation(3);

        // Act
        List<int> orbit = GroupPermutations.GetOrbitList([swap01, swap01, identity], point: 0, degree: 3);

        // Assert
        Assert.Equal([0, 1], orbit);
    }

    [Fact(DisplayName = "Should throw for point outside degree")]
    public void ShouldThrowForPointOutsideDegree()
    {
        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            GroupPermutations.GetOrbitList([], point: 3, degree: 3));
    }

    [Fact(DisplayName = "Should throw when generator maps outside degree")]
    public void ShouldThrowWhenGeneratorMapsOutsideDegree()
    {
        // Arrange
        Permutation moveZeroToThree = GroupPermutations.CreatePermutation(3, 1, 2, 0);

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            GroupPermutations.GetOrbitList([moveZeroToThree], point: 0, degree: 3));
    }
}
