using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupCoreTests
{
    [Fact(DisplayName = "Should create trivial group from identity generator")]
    public void ShouldCreateTrivialGroupFromIdentityGenerator()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.CreatePermutationGroup(
            GroupPermutations.CreateIdentityPermutation(2));

        // Act + Assert
        group.IsTrivial().ShouldBeTrue();
    }

    [Fact(DisplayName = "Should throw for negative symmetric group degree")]
    public void ShouldThrowForNegativeSymmetricGroupDegree()
    {
        // Act + Assert
        Should.Throw<ArgumentException>(() => _ = PermutationGroup.SymmetricGroup(-1));
    }

    [Fact(DisplayName = "Should expose order and degree for symmetric group")]
    public void ShouldExposeOrderAndDegreeForSymmetricGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Assert
        group.Degree.ShouldBe(3);
        Should.Throw<NullReferenceException>(() => _ = group.Order);
    }
}
