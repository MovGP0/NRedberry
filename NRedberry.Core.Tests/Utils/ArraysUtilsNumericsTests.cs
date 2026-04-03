using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsNumericsTests
{
    [Fact]
    public void ShouldReturnMaximumValue()
    {
        int actual = ArraysUtils.Max([2, 7, 4, 1]);

        actual.ShouldBe(7);
    }

    [Fact]
    public void ShouldReturnSeriesFromZero()
    {
        int[] actual = ArraysUtils.GetSeriesFrom0(4);

        actual.ShouldBe([0, 1, 2, 3]);
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

        source[0][0].ShouldBe(1);
        clone[0][0].ShouldBe(99);
    }

    [Fact]
    public void ShouldSumValues()
    {
        int actual = ArraysUtils.Sum([1, 2, 3, 4]);

        actual.ShouldBe(10);
    }
}
