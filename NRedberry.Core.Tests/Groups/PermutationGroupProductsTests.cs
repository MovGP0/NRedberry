using NRedberry.Groups;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupProductsTests
{
    [Fact(DisplayName = "Should create direct product of identity generated groups")]
    public void ShouldCreateDirectProductOfIdentityGeneratedGroups()
    {
        PermutationGroup left = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        PermutationGroup right = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(3));

        PermutationGroup product = left.DirectProduct(right);

        product.ShouldNotBeNull();
        product.Degree.ShouldBe(5);
        product.Order.ShouldBe(1);
    }

    [Fact(DisplayName = "Should create direct product of symmetric groups")]
    public void ShouldCreateDirectProductOfSymmetricGroups()
    {
        PermutationGroup left = PermutationGroup.SymmetricGroup(2);
        PermutationGroup right = PermutationGroup.SymmetricGroup(3);

        PermutationGroup product = left.DirectProduct(right);

        product.ShouldNotBeNull();
        product.Degree.ShouldBe(5);
        product.Order.ShouldBe(left.Order * right.Order);
    }

    [Fact(DisplayName = "Should create direct product of identity generated and symmetric groups")]
    public void ShouldCreateDirectProductOfIdentityGeneratedAndSymmetricGroups()
    {
        PermutationGroup identityGenerated = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));
        PermutationGroup symmetric = PermutationGroup.SymmetricGroup(3);

        PermutationGroup product = identityGenerated.DirectProduct(symmetric);

        product.ShouldNotBeNull();
        product.Degree.ShouldBe(5);
        product.Order.ShouldBe(symmetric.Order);
    }
}
