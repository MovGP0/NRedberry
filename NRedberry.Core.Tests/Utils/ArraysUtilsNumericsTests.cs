using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsNumericsTests
{
    [Fact]
    public void ShouldReturnMaximumValue()
    {
        int actual = ArraysUtils.Max([2, 7, 4, 1]);

        Assert.Equal(7, actual);
    }

    [Fact]
    public void ShouldReturnSeriesFromZero()
    {
        int[] actual = ArraysUtils.GetSeriesFrom0(4);

        Assert.Equal([0, 1, 2, 3], actual);
    }

    [Fact]
    public void ShouldDeepCloneNestedArrays()
    {
        int[][] source =
        [
            [1, 2],
            [3, 4]
        ];

        int[][] clone = ArraysUtils.DeepClone(source);
        clone[0][0] = 99;

        Assert.Equal(1, source[0][0]);
        Assert.Equal(99, clone[0][0]);
    }

    [Fact]
    public void ShouldSumValues()
    {
        int actual = ArraysUtils.Sum([1, 2, 3, 4]);

        Assert.Equal(10, actual);
    }
}
