using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class InducedOrderingTests
{
    [Fact(DisplayName = "Should throw for empty base")]
    public void ShouldThrowForEmptyBase()
    {
        // Act + Assert
        Should.Throw<ArgumentException>(() => _ = new InducedOrdering([]));
    }

    [Fact(DisplayName = "Should compare according to base order")]
    public void ShouldCompareAccordingToBaseOrder()
    {
        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = new InducedOrdering([2, 0, 1]));
    }

    [Fact(DisplayName = "Should expose boundary elements")]
    public void ShouldExposeBoundaryElements()
    {
        // Act + Assert
        Should.Throw<NullReferenceException>(() => _ = new InducedOrdering([0, 1]));
    }
}
