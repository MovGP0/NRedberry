using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IntComparatorTests
{
    [Fact]
    public void ShouldCompareIntegersUsingDefaultComparator()
    {
        IntComparator comparator = IntComparators.Default;

        (comparator.Compare(1, 2) < 0).ShouldBeTrue();
        comparator.Compare(7, 7).ShouldBe(0);
        (comparator.Compare(9, 3) > 0).ShouldBeTrue();
    }
}
