using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupCentralizersTests
{
    [Fact(DisplayName = "Should return identity center for symmetric group")]
    public void ShouldReturnIdentityCenterForSymmetricGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act
        PermutationGroup center = group.Center();

        // Assert
        center.ShouldNotBeNull().ShouldSatisfyAllConditions(
            c => c.Order.ShouldBe(1),
            c => c.IsTrivial().ShouldBeTrue());
    }

    [Fact(DisplayName = "Should conjugate trivial group to itself")]
    public void ShouldConjugateTrivialGroupToItself()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        Permutation permutation = GroupPermutations.CreateIdentityPermutation(3);

        // Act
        PermutationGroup conjugated = group.Conjugate(permutation);

        // Assert
        conjugated.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Should compute centralizer of identity permutation")]
    public void ShouldComputeCentralizerOfIdentityPermutation()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);
        Permutation identity = GroupPermutations.CreateIdentityPermutation(3);

        // Act
        PermutationGroup centralizer = group.CentralizerOf(identity);

        // Assert
        centralizer
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                c => c.Order.ShouldBe(group.Order),
                c => c.Degree.ShouldBe(group.Degree));
    }
}
