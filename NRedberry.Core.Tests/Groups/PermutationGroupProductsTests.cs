using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupProductsTests
{
    [Fact(DisplayName = "Should throw for direct product of identity generated groups")]
    public void ShouldThrowForDirectProductOfIdentityGeneratedGroups()
    {
        // Arrange
        PermutationGroup left = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        PermutationGroup right = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = left.DirectProduct(right));
    }

    [Fact(DisplayName = "Should throw for direct product of symmetric groups")]
    public void ShouldThrowForDirectProductOfSymmetricGroups()
    {
        // Arrange
        PermutationGroup left = PermutationGroup.SymmetricGroup(2);
        PermutationGroup right = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = left.DirectProduct(right));
    }

    [Fact(DisplayName = "Should throw for direct product of identity generated and symmetric groups")]
    public void ShouldThrowForDirectProductOfIdentityGeneratedAndSymmetricGroups()
    {
        // Arrange
        PermutationGroup identityGenerated = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = identityGenerated.DirectProduct(symmetric));
    }
}
