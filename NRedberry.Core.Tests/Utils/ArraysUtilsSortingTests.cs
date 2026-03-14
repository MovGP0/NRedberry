using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsSortingTests
{
    [Fact]
    public void ShouldStableSortSmallArrayUsingInsertionSortBranch()
    {
        int[] target = [3, 1, 2];
        int[] coSort = [30, 10, 20];

        ArraysUtils.StableSort(target, coSort);

        target.ShouldBe([1, 2, 3]);
        coSort.ShouldBe([10, 20, 30]);
    }

    [Fact]
    public void ShouldStableSortLargeArrayUsingTimSortBranch()
    {
        int[] target = Enumerable.Range(0, 101).Reverse().ToArray();
        int[] coSort = target.Select(static value => value * 10).ToArray();

        ArraysUtils.StableSort(target, coSort);

        target.ShouldBe(Enumerable.Range(0, 101).ToArray());
        coSort.ShouldBe(Enumerable.Range(0, 101).Select(static value => value * 10).ToArray());
    }
}
