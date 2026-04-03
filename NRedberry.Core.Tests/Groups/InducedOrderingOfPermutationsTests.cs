using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class InducedOrderingOfPermutationsTests
{
    [Fact(DisplayName = "Should throw for null base")]
    public void ShouldThrowForNullBase()
    {
        // Act + Assert
        Should.Throw<ArgumentNullException>(() => _ = new InducedOrderingOfPermutations(null!));
    }

    [Fact(DisplayName = "Should handle nulls in compare")]
    public void ShouldHandleNullsInCompare()
    {
        InducedOrderingOfPermutations ordering = new([0, 1]);

        ordering.Compare(null, null).ShouldBe(0);
        ordering.Compare(null, GroupPermutations.CreateIdentityPermutation(2)).ShouldBeLessThan(0);
        ordering.Compare(GroupPermutations.CreateIdentityPermutation(2), null).ShouldBeGreaterThan(0);
    }

    [Fact(DisplayName = "Should compare by base images")]
    public void ShouldCompareByBaseImages()
    {
        InducedOrderingOfPermutations ordering = new([0, 1, 2]);
        Permutation first = GroupPermutations.CreatePermutation(new int[][] { [0, 1] });
        Permutation second = GroupPermutations.CreatePermutation(new int[][] { [1, 2] });

        ordering.Compare(first, first).ShouldBe(0);
        ordering.Compare(first, second).ShouldNotBe(0);
    }
}
