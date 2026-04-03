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

    [Fact(DisplayName = "Pointwise stabilizer should return subgroup for non-empty set in symmetric group")]
    public void ShouldReturnSubgroupForPointwiseStabilizerWithNonEmptySet()
    {
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        PermutationGroup stabilizer = group.PointwiseStabilizer(0);

        stabilizer.ShouldNotBeNull();
        stabilizer.Degree.ShouldBe(4);
        stabilizer.Order.ShouldBe(6);
    }

    [Fact(DisplayName = "Pointwise restricted stabilizer should return subgroup for non-empty set")]
    public void ShouldReturnSubgroupForPointwiseRestrictedStabilizerWithNonEmptySet()
    {
        Permutation permutation1 = GroupPermutations.CreatePermutation(new int[][] { [1, 2, 3] });
        Permutation permutation2 = GroupPermutations.CreatePermutation(new int[][] { [3, 4, 5, 6, 7] });
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(permutation1, permutation2);

        PermutationGroup stabilizer = group.PointwiseStabilizerRestricted(1, 2, 3);

        stabilizer.ShouldNotBeNull();
        stabilizer.Order.ShouldBeGreaterThan(0);
        stabilizer.Order.ShouldBeLessThanOrEqualTo(group.Order);
    }

    [Fact(DisplayName = "Setwise stabilizer should return subgroup for non-empty set in symmetric group")]
    public void ShouldReturnSubgroupForSetwiseStabilizerWithNonEmptySet()
    {
        PermutationGroup group = PermutationGroup.SymmetricGroup(4);

        PermutationGroup stabilizer = group.SetwiseStabilizer(0, 1);

        stabilizer.ShouldNotBeNull();
        stabilizer.Degree.ShouldBe(4);
        stabilizer.Order.ShouldBe(4);
    }
}
