using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class InducedOrderingOfPermutationsTests
{
    [Fact(DisplayName = "Should throw for null base")]
    public void ShouldThrowForNullBase()
    {
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => _ = new InducedOrderingOfPermutations(null!));
    }

    [Fact(DisplayName = "Should handle nulls in compare")]
    public void ShouldHandleNullsInCompare()
    {
        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = new InducedOrderingOfPermutations([0, 1]));
    }

    [Fact(DisplayName = "Should compare by base images")]
    public void ShouldCompareByBaseImages()
    {
        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = new InducedOrderingOfPermutations([0, 1, 2]));
    }
}
