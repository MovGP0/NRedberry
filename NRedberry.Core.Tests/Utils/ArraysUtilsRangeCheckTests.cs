using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsRangeCheckTests
{
    [Fact]
    public void ShouldThrowWhenQuickSortRangeIsInvalid()
    {
        int[] values = [3, 2, 1];
        int[] coSort = [30, 20, 10];

        Assert.Throws<ArgumentException>(() => ArraysUtils.QuickSort(values, 2, 1, coSort));
    }

    [Fact]
    public void ShouldThrowWhenInsertionSortRangeIsOutsideArray()
    {
        int[] values = [3, 2, 1];
        int[] coSort = [30, 20, 10];

        Assert.Throws<ArgumentOutOfRangeException>(() => ArraysUtils.InsertionSort(values, -1, 2, coSort));
    }
}
