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

    [Fact(DisplayName = "Should throw for symmetric contains subgroup path")]
    public void ShouldThrowForSymmetricContainsSubgroupPath()
    {
        // Arrange
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);
        PermutationGroup alternating = PermutationGroup.AlternatingGroup(5);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = symmetric.ContainsSubgroup(alternating));
    }

    [Fact(DisplayName = "Should throw for intersection when this is symmetric group")]
    public void ShouldThrowForIntersectionWhenThisIsSymmetricGroup()
    {
        // Arrange
        PermutationGroup largerSymmetric = PermutationGroup.SymmetricGroup(5);
        PermutationGroup smallerSymmetric = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = largerSymmetric.Intersection(smallerSymmetric));
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

    [Fact(DisplayName = "Should throw for normal closure on symmetric path")]
    public void ShouldThrowForNormalClosureOnSymmetricPath()
    {
        // Arrange
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);
        Permutation oddPermutation = GroupPermutations.CreatePermutation(
            GroupPermutations.CreateTransposition(5, 0, 1));
        PermutationGroup oddGeneratedGroup = PermutationGroup.CreatePermutationGroup(oddPermutation);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = symmetric.NormalClosureOf(oddGeneratedGroup));
    }

    [Fact(DisplayName = "Should throw for derived subgroup of symmetric group")]
    public void ShouldThrowForDerivedSubgroupOfSymmetricGroup()
    {
        // Arrange
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = symmetric.DerivedSubgroup());
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

    [Fact(DisplayName = "Should throw for commutator of symmetric group with itself")]
    public void ShouldThrowForCommutatorOfSymmetricGroupWithItself()
    {
        // Arrange
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(5);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = symmetric.Commutator(symmetric));
    }
}
