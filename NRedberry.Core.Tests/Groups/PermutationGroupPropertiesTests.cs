using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupPropertiesTests
{
    [Fact(DisplayName = "Should report trivial for identity generated group")]
    public void ShouldReportTrivialForIdentityGeneratedGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act + Assert
        group.IsTrivial().ShouldBeTrue();
    }

    [Fact(DisplayName = "Should report non trivial for transposition generated group")]
    public void ShouldReportNonTrivialForTranspositionGeneratedGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1)));

        // Act + Assert
        group.IsTrivial().ShouldBeFalse();
    }

    [Fact(DisplayName = "Should report transitive for cycle generated group")]
    public void ShouldReportTransitiveForCycleGeneratedGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(3)));

        // Act + Assert
        group.IsTransitive().ShouldBeTrue();
    }

    [Fact(DisplayName = "Should report non transitive for disconnected orbit group")]
    public void ShouldReportNonTransitiveForDisconnectedOrbitGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1)));

        // Act + Assert
        group.IsTransitive().ShouldBeFalse();
    }

    [Fact(DisplayName = "Should validate interval transitivity and bounds")]
    public void ShouldValidateIntervalTransitivityAndBounds()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1)));

        // Act + Assert
        group.IsTransitive(0, 2).ShouldBeTrue();
        group.IsTransitive(0, 3).ShouldBeFalse();
    }

    [Fact(DisplayName = "Should throw when transitivity interval is invalid")]
    public void ShouldThrowWhenTransitivityIntervalIsInvalid()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(3)));

        // Act + Assert
        Should.Throw<ArgumentException>(() => group.IsTransitive(2, 1));
    }

    [Fact(DisplayName = "Should report abelian for cyclic and non abelian for symmetric")]
    public void ShouldReportAbelianForCyclicAndNonAbelianForSymmetric()
    {
        // Arrange
        PermutationGroup cyclic = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(3)));
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        cyclic.IsAbelian().ShouldBeTrue();
        symmetric.IsAbelian().ShouldBeFalse();
    }

    [Fact(DisplayName = "Should report expected properties for trivial group")]
    public void ShouldReportExpectedPropertiesForTrivialGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act + Assert
        group.IsSymmetric().ShouldBeFalse();
        group.IsAlternating().ShouldBeFalse();
        group.IsRegular().ShouldBeFalse();
    }

    [Fact(DisplayName = "Should report non regular for non transitive group")]
    public void ShouldReportNonRegularForNonTransitiveGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1)));

        // Act + Assert
        group.IsRegular().ShouldBeFalse();
    }
}
