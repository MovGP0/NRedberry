using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsQuickSortTests
{
    [Fact]
    public void ShouldReturnPermutationThatSortsArray()
    {
        int[] original = [4, 1, 3, 2];
        int[] sorted = (int[])original.Clone();

        int[] permutation = ArraysUtils.QuickSortP(sorted);

        sorted.ShouldBe([1, 2, 3, 4]);
        permutation.ShouldBe([1, 3, 2, 0]);
    }

    [Fact]
    public void ShouldSortUsingCustomComparator()
    {
        int[] values = [4, 1, 3, 2];

        ArraysUtils.QuickSort(values, Comparer<int>.Create(static (left, right) => right.CompareTo(left)));

        values.ShouldBe([4, 3, 2, 1]);
    }

    [Fact]
    public void ShouldSortWithCoSortUsingComparator()
    {
        int[] values = [4, 1, 3, 2];
        int[] coSort = [40, 10, 30, 20];

        ArraysUtils.QuickSort(values, coSort, Comparer<int>.Create(static (left, right) => right.CompareTo(left)));

        values.ShouldBe([4, 3, 2, 1]);
        coSort.ShouldBe([40, 30, 20, 10]);
    }
}
