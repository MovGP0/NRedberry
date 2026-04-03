using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class InducedOrderingTests
{
    [Fact(DisplayName = "Should support empty base")]
    public void ShouldSupportEmptyBase()
    {
        InducedOrdering ordering = new([]);

        ordering.MinElement().ShouldBe(-1);
        ordering.MaxElement().ShouldBe(0);
    }

    [Fact(DisplayName = "Should compare according to base order")]
    public void ShouldCompareAccordingToBaseOrder()
    {
        InducedOrdering ordering = new([2, 0, 1]);

        ordering.Compare(2, 0).ShouldBeLessThan(0);
        ordering.Compare(0, 1).ShouldBeLessThan(0);
        ordering.Compare(1, 2).ShouldBeGreaterThan(0);
    }

    [Fact(DisplayName = "Should expose boundary elements")]
    public void ShouldExposeBoundaryElements()
    {
        InducedOrdering ordering = new([0, 1]);

        ordering.MinElement().ShouldBe(-1);
        ordering.MaxElement().ShouldBe(2);
    }
}
