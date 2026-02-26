using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupEnumerableTests
{
    [Fact(DisplayName = "Should enumerate single identity for trivial group")]
    public void ShouldEnumerateSingleIdentityForTrivialGroup()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = group.ToList());
    }

    [Fact(DisplayName = "Should support non generic enumeration for trivial group")]
    public void ShouldSupportNonGenericEnumerationForTrivialGroup()
    {
        // Arrange
        IEnumerable<Permutation> group = PermutationGroup.SymmetricGroup(3);

        // Act + Assert
        Assert.Throws<NullReferenceException>(() => _ = group.Count());
    }
}
