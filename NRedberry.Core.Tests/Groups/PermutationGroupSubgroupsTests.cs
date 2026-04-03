using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupSubgroupsTests
{
    [Fact(DisplayName = "Should return same instance when unioning group with itself")]
    public void ShouldReturnSameInstanceWhenUnioningGroupWithItself()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(5);

        // Act
        PermutationGroup union = group.Union(group);

        // Assert
        union.ShouldBeSameAs(group);
    }

    [Fact(DisplayName = "Should return false when subgroup degree is larger")]
    public void ShouldReturnFalseWhenSubgroupDegreeIsLarger()
    {
        // Arrange
        PermutationGroup smaller = PermutationGroup.SymmetricGroup(3);
        PermutationGroup larger = PermutationGroup.SymmetricGroup(5);

        // Act + Assert
        smaller.ContainsSubgroup(larger).ShouldBeFalse();
    }

    [Fact(DisplayName = "Should report that symmetric group contains alternating subgroup")]
    public void ShouldReportThatSymmetricGroupContainsAlternatingSubgroup()
    {
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);
        PermutationGroup alternating = PermutationGroup.AlternatingGroup(5);

        symmetric.ContainsSubgroup(alternating).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should return subgroup intersection when this is symmetric group")]
    public void ShouldReturnIntersectionWhenThisIsSymmetricGroup()
    {
        PermutationGroup largerSymmetric = PermutationGroup.SymmetricGroup(5);
        PermutationGroup smallerSymmetric = PermutationGroup.SymmetricGroup(3);

        PermutationGroup intersection = largerSymmetric.Intersection(smallerSymmetric);

        intersection.ShouldNotBeNull();
        intersection.Order.ShouldBeGreaterThan(0);
        intersection.Order.ShouldBeLessThanOrEqualTo(smallerSymmetric.Order);
    }

    [Fact(DisplayName = "Should return subgroup when normal closure input is trivial subgroup")]
    public void ShouldReturnSubgroupWhenNormalClosureInputIsTrivialSubgroup()
    {
        // Arrange
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);
        Permutation trivial = GroupPermutations.CreateIdentityPermutation(5);
        PermutationGroup trivialSubgroup = PermutationGroup.CreatePermutationGroup(trivial);

        // Act
        PermutationGroup closure = symmetric.NormalClosureOf(trivialSubgroup);

        // Assert
        closure.ShouldBeSameAs(trivialSubgroup);
    }

    [Fact(DisplayName = "Should return normal closure on symmetric path")]
    public void ShouldReturnNormalClosureOnSymmetricPath()
    {
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);
        Permutation oddPermutation = GroupPermutations.CreatePermutation(
            GroupPermutations.CreateTransposition(5, 0, 1));
        PermutationGroup oddGeneratedGroup = PermutationGroup.CreatePermutationGroup(oddPermutation);

        PermutationGroup closure = symmetric.NormalClosureOf(oddGeneratedGroup);

        closure.ShouldNotBeNull();
        closure.Order.ShouldBeGreaterThanOrEqualTo(oddGeneratedGroup.Order);
    }

    [Fact(DisplayName = "Should return derived subgroup of symmetric group")]
    public void ShouldReturnDerivedSubgroupOfSymmetricGroup()
    {
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);

        PermutationGroup derived = symmetric.DerivedSubgroup();

        derived.ShouldNotBeNull();
        derived.Degree.ShouldBe(5);
        derived.Order.ShouldBe(60);
    }

    [Fact(DisplayName = "Should create union when both groups are generator-backed")]
    public void ShouldCreateUnionWhenBothGroupsAreGeneratorBacked()
    {
        // Arrange
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);
        PermutationGroup alternating = PermutationGroup.AlternatingGroup(5);

        // Act
        PermutationGroup union = symmetric.Union(alternating);

        // Assert
        union.ShouldNotBeNull();
        union.Degree.ShouldBe(5);
    }

    [Fact(DisplayName = "Should return commutator of symmetric group with itself")]
    public void ShouldReturnCommutatorOfSymmetricGroupWithItself()
    {
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);

        PermutationGroup commutator = symmetric.Commutator(symmetric);

        commutator.ShouldNotBeNull();
        commutator.Degree.ShouldBe(5);
        commutator.Order.ShouldBe(60);
    }
}
