using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IntComparatorTests
{
    [Fact]
    public void ShouldCompareIntegersUsingDefaultComparator()
    {
        IntComparator comparator = IntComparators.Default;

        Assert.True(comparator.Compare(1, 2) < 0);
        Assert.Equal(0, comparator.Compare(7, 7));
        Assert.True(comparator.Compare(9, 3) > 0);
    }
}
