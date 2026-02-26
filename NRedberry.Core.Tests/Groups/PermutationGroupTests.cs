using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupTests
{
    [Fact(DisplayName = "Should provide stable string for trivial group")]
    public void ShouldProvideStableStringForTrivialGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));

        // Act
        string value = group.ToString();

        // Assert
        Assert.Contains("Group(", value, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "Should provide java string for trivial group")]
    public void ShouldProvideJavaStringForTrivialGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));

        // Act
        string value = group.ToStringJava();

        // Assert
        Assert.Contains("PermutationGroup.createPermutationGroup", value, StringComparison.Ordinal);
    }
}
