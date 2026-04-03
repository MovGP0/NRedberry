using NRedberry.Maths;

namespace NRedberry.Core.Tests.Number;

public sealed class MathUtilsTests
{
    [Fact]
    public void GetSortedDistinctShouldSortMutateAndRemoveDuplicates()
    {
        int[] values = [5, 2, 5, 1, 3, 1];

        int[] result = MathUtils.GetSortedDistinct(values);

        result.ShouldBe([1, 2, 3, 5]);
        values.ShouldNotBe([5, 2, 5, 1, 3, 1]);
        values[..result.Length].ShouldBe(result);
    }

    [Fact]
    public void GetSortedDistinctShouldReturnEmptyForEmptyInput()
    {
        int[] values = [];

        int[] result = MathUtils.GetSortedDistinct(values);

        result.ShouldBeEmpty();
        result.ShouldBeSameAs(values);
    }

    [Fact]
    public void IntSetDifferenceShouldReturnAllFromBWhenDisjoint()
    {
        int[] a = [1, 2];
        int[] b = [3, 4];

        int[] result = MathUtils.IntSetDifference(a, b);

        result.ShouldBe([3, 4]);
    }

    [Fact]
    public void IntSetDifferenceShouldExcludeOverlappingElements()
    {
        int[] a = [1, 3, 5];
        int[] b = [1, 2, 3, 4, 6];

        int[] result = MathUtils.IntSetDifference(a, b);

        result.ShouldBe([2, 4, 6]);
    }

    [Fact]
    public void IntSetDifferenceShouldHandleEmptyInputs()
    {
        int[] a = [];
        int[] b = [];

        int[] bothEmpty = MathUtils.IntSetDifference(a, b);
        int[] onlyAEmpty = MathUtils.IntSetDifference([], [1, 2, 3]);
        int[] onlyBEmpty = MathUtils.IntSetDifference([1, 2, 3], []);

        bothEmpty.ShouldBeEmpty();
        onlyAEmpty.ShouldBe([1, 2, 3]);
        onlyBEmpty.ShouldBeEmpty();
    }

    [Fact]
    public void IntSetUnionShouldIncludeAllWhenDisjoint()
    {
        int[] a = [1, 3];
        int[] b = [2, 4];

        int[] result = MathUtils.IntSetUnion(a, b);

        result.ShouldBe([1, 2, 3, 4]);
    }

    [Fact]
    public void IntSetUnionShouldIncludeSharedElementsOnceWhenOverlapping()
    {
        int[] a = [1, 3, 5];
        int[] b = [1, 2, 3, 6];

        int[] result = MathUtils.IntSetUnion(a, b);

        result.ShouldBe([1, 2, 3, 5, 6]);
    }

    [Fact]
    public void IntSetUnionShouldReturnSupersetWhenOneSetIsSubset()
    {
        int[] a = [1, 2, 3, 4];
        int[] b = [2, 3];

        int[] result = MathUtils.IntSetUnion(a, b);

        result.ShouldBe([1, 2, 3, 4]);
    }
}
