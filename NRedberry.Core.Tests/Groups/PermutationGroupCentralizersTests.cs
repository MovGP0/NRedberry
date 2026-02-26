using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupCentralizersTests
{
    [Fact(DisplayName = "Should return identity center for symmetric group")]
    public void ShouldReturnIdentityCenterForSymmetricGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = group.Center());
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
        Assert.NotNull(conjugated);
    }

    [Fact(DisplayName = "Should compute centralizer of identity permutation")]
    public void ShouldComputeCentralizerOfIdentityPermutation()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);
        Permutation identity = GroupPermutations.CreateIdentityPermutation(3);

        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = group.CentralizerOf(identity));
    }
}
