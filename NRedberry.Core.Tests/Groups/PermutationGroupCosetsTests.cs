using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupCosetsTests
{
    [Fact(DisplayName = "Should return identity left coset for trivial group")]
    public void ShouldReturnIdentityLeftCosetForTrivialGroup()
    {
        // Arrange
        PermutationGroup trivial = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));

        // Act
        Permutation[] reps = trivial.LeftCosetRepresentatives(trivial);

        // Assert
        reps.Length.ShouldBe(1);
        reps[0].IsIdentity.ShouldBeTrue();
    }

    [Fact(DisplayName = "Should return inverse right coset representatives")]
    public void ShouldReturnInverseRightCosetRepresentatives()
    {
        // Arrange
        PermutationGroup trivial = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));

        // Act
        Permutation[] left = trivial.LeftCosetRepresentatives(trivial);
        Permutation[] right = trivial.RightCosetRepresentatives(trivial);

        // Assert
        right.Length.ShouldBe(left.Length);
        right[0].Equals(left[0].Inverse()).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should compute left transversal for trivial group")]
    public void ShouldComputeLeftTransversalForTrivialGroup()
    {
        // Arrange
        PermutationGroup trivial = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        Permutation identity = GroupPermutations.GetIdentityPermutation();

        // Act
        Permutation transversal = trivial.LeftTransversalOf(trivial, identity);

        // Assert
        transversal.IsIdentity.ShouldBeTrue();
    }
}
