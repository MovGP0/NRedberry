using NRedberry.Maths;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class MathUtilsTests
{
    [Fact]
    public void GetSortedDistinctShouldSortMutateAndRemoveDuplicates()
    {
        int[] values = [5, 2, 5, 1, 3, 1];

        int[] result = MathUtils.GetSortedDistinct(values);

        Assert.Equal([1, 2, 3, 5], result);
        Assert.NotEqual([5, 2, 5, 1, 3, 1], values);
        Assert.Equal(result, values[..result.Length]);
    }

    [Fact]
    public void GetSortedDistinctShouldReturnEmptyForEmptyInput()
    {
        int[] values = [];

        int[] result = MathUtils.GetSortedDistinct(values);

        Assert.Empty(result);
        Assert.Same(values, result);
    }

    [Fact]
    public void IntSetDifferenceShouldReturnAllFromBWhenDisjoint()
    {
        int[] a = [1, 2];
        int[] b = [3, 4];

        int[] result = MathUtils.IntSetDifference(a, b);

        Assert.Equal([3, 4], result);
    }

    [Fact]
    public void IntSetDifferenceShouldExcludeOverlappingElements()
    {
        int[] a = [1, 3, 5];
        int[] b = [1, 2, 3, 4, 6];

        int[] result = MathUtils.IntSetDifference(a, b);

        Assert.Equal([2, 4, 6], result);
    }

    [Fact]
    public void IntSetDifferenceShouldHandleEmptyInputs()
    {
        int[] a = [];
        int[] b = [];

        int[] bothEmpty = MathUtils.IntSetDifference(a, b);
        int[] onlyAEmpty = MathUtils.IntSetDifference([], [1, 2, 3]);
        int[] onlyBEmpty = MathUtils.IntSetDifference([1, 2, 3], []);

        Assert.Empty(bothEmpty);
        Assert.Equal([1, 2, 3], onlyAEmpty);
        Assert.Empty(onlyBEmpty);
    }

    [Fact]
    public void IntSetUnionShouldIncludeAllWhenDisjoint()
    {
        int[] a = [1, 3];
        int[] b = [2, 4];

        int[] result = MathUtils.IntSetUnion(a, b);

        Assert.Equal([1, 2, 3, 4], result);
    }

    [Fact]
    public void IntSetUnionShouldIncludeSharedElementsOnceWhenOverlapping()
    {
        int[] a = [1, 3, 5];
        int[] b = [1, 2, 3, 6];

        int[] result = MathUtils.IntSetUnion(a, b);

        Assert.Equal([1, 2, 3, 5, 6], result);
    }

    [Fact]
    public void IntSetUnionShouldReturnSupersetWhenOneSetIsSubset()
    {
        int[] a = [1, 2, 3, 4];
        int[] b = [2, 3];

        int[] result = MathUtils.IntSetUnion(a, b);

        Assert.Equal([1, 2, 3, 4], result);
    }
}
