using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupStabilizersTests
{
    [Fact(DisplayName = "Pointwise stabilizer should return same instance for empty set")]
    public void ShouldReturnSameInstanceForPointwiseStabilizerWithEmptySet()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        // Act
        PermutationGroup stabilizer = group.PointwiseStabilizer();

        // Assert
        stabilizer.ShouldBeSameAs(group);
    }

    [Fact(DisplayName = "Setwise stabilizer should return same instance for empty set")]
    public void ShouldReturnSameInstanceForSetwiseStabilizerWithEmptySet()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        // Act
        PermutationGroup stabilizer = group.SetwiseStabilizer();

        // Assert
        stabilizer.ShouldBeSameAs(group);
    }

    [Fact(DisplayName = "Pointwise stabilizer should throw for non-empty set in symmetric group")]
    public void ShouldThrowForPointwiseStabilizerWithNonEmptySet()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = group.PointwiseStabilizer(0));
    }

    [Fact(DisplayName = "Pointwise restricted stabilizer should throw for non-empty set")]
    public void ShouldThrowForPointwiseRestrictedStabilizerWithNonEmptySet()
    {
        // Arrange
        Permutation permutation1 = GroupPermutations.CreatePermutation(new int[][] { [1, 2, 3] });
        Permutation permutation2 = GroupPermutations.CreatePermutation(new int[][] { [3, 4, 5, 6, 7] });
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(permutation1, permutation2);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = group.PointwiseStabilizerRestricted(1, 2, 3));
    }

    [Fact(DisplayName = "Setwise stabilizer should throw for non-empty set in symmetric group")]
    public void ShouldThrowForSetwiseStabilizerWithNonEmptySet()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = group.SetwiseStabilizer(0, 1));
    }
}
