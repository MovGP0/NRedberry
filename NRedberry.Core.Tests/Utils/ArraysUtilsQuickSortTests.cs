using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsQuickSortTests
{
    [Fact]
    public void ShouldReturnPermutationThatSortsArray()
    {
        int[] original = [4, 1, 3, 2];
        int[] sorted = (int[])original.Clone();

        int[] permutation = ArraysUtils.QuickSortP(sorted);

        Assert.Equal([1, 2, 3, 4], sorted);
        Assert.Equal([1, 3, 2, 0], permutation);
    }

    [Fact]
    public void ShouldSortUsingCustomComparator()
    {
        int[] values = [4, 1, 3, 2];

        ArraysUtils.QuickSort(values, Comparer<int>.Create(static (left, right) => right.CompareTo(left)));

        Assert.Equal([4, 3, 2, 1], values);
    }

    [Fact]
    public void ShouldSortWithCoSortUsingComparator()
    {
        int[] values = [4, 1, 3, 2];
        int[] coSort = [40, 10, 30, 20];

        ArraysUtils.QuickSort(values, coSort, Comparer<int>.Create(static (left, right) => right.CompareTo(left)));

        Assert.Equal([4, 3, 2, 1], values);
        Assert.Equal([40, 30, 20, 10], coSort);
    }
}
