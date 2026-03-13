using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class MathUtilsTests
{
    [Fact]
    public void ShouldReturnSortedDistinctValues()
    {
        int[] actual = MathUtils.GetSortedDistinct([4, 2, 4, 1, 2]);

        Assert.Equal(new[] { 1, 2, 4 }, actual);
    }

    [Fact]
    public void ShouldReturnIntSetDifference()
    {
        int[] actual = MathUtils.IntSetDifference([1, 3], [1, 2, 3, 5]);

        Assert.Equal(new[] { 2, 5 }, actual);
    }

    [Fact]
    public void ShouldReturnIntSetUnion()
    {
        int[] actual = MathUtils.IntSetUnion([1, 3, 5], [2, 3, 4]);

        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, actual);
    }
}
