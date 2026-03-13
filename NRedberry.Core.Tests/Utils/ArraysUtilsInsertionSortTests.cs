using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsInsertionSortTests
{
    [Fact]
    public void ShouldSortIntArrayWithIntCoSort()
    {
        int[] target = [4, 1, 3, 2];
        int[] coSort = [40, 10, 30, 20];

        ArraysUtils.InsertionSort(target, coSort);

        Assert.Equal([1, 2, 3, 4], target);
        Assert.Equal([10, 20, 30, 40], coSort);
    }

    [Fact]
    public void ShouldSortIntArrayWithLongCoSortRange()
    {
        int[] target = [9, 4, 3, 8, 1];
        long[] coSort = [90, 40, 30, 80, 10];

        ArraysUtils.InsertionSort(target, 1, 4, coSort);

        Assert.Equal([9, 3, 4, 8, 1], target);
        Assert.Equal([90L, 30L, 40L, 80L, 10L], coSort);
    }

    [Fact]
    public void ShouldSortGenericArrayWithIntCoSort()
    {
        string[] target = ["d", "a", "c", "b"];
        int[] coSort = [4, 1, 3, 2];

        ArraysUtils.InsertionSort(target, coSort);

        Assert.Equal(["a", "b", "c", "d"], target);
        Assert.Equal([1, 2, 3, 4], coSort);
    }
}
