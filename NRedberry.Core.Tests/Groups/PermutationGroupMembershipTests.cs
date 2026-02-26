using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupMembershipTests
{
    [Fact(DisplayName = "Should match contains and membership test")]
    public void ShouldMatchContainsAndMembershipTest()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);
        Permutation identity = GroupPermutations.CreateIdentityPermutation(3);
        Exception? exception = null;

        // Act
        try
        {
            _ = group.Contains(identity);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        if (exception is null)
        {
            Assert.Equal(group.Contains(identity), group.MembershipTest(identity));
        }
        else
        {
            Assert.IsType<NullReferenceException>(exception);
        }
    }

    [Fact(DisplayName = "Should accept identity in trivial group")]
    public void ShouldAcceptIdentityInTrivialGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        Permutation identity = GroupPermutations.GetIdentityPermutation();

        // Act + Assert
        Assert.True(group.MembershipTest(identity));
    }

    [Fact(DisplayName = "Should reject moved permutation in trivial group")]
    public void ShouldRejectMovedPermutationInTrivialGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        Permutation transposition = GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(2, 0, 1));

        // Act + Assert
        Assert.False(group.MembershipTest(transposition));
    }
}
