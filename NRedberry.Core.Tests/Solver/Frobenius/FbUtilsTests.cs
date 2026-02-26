using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FbUtilsTests
{
    [Fact]
    public void ShouldCompareSolutionsLexicographically()
    {
        int result = FbUtils.SolutionComparator.Compare([1, 2, 3], [1, 2, 4]);

        Assert.True(result < 0);
        Assert.Equal(0, FbUtils.SolutionComparator.Compare([1, 2, 3], [1, 2, 3]));
        Assert.True(FbUtils.SolutionComparator.Compare([1, 3, 0], [1, 2, 9]) > 0);
    }

    [Fact]
    public void ShouldThrowWhenComparingDifferentSolutionSizes()
    {
        Assert.Throws<ArgumentException>(() => FbUtils.SolutionComparator.Compare([1, 2], [1]));
    }

    [Fact]
    public void ShouldThrowWhenGetAllSolutionsInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => FbUtils.GetAllSolutions(null!));
    }

    [Fact]
    public void ShouldThrowWhenIterableInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => FbUtils.Iterable(null!));
    }

    [Fact]
    public void ShouldReturnAllSolutionsForSimpleEquation()
    {
        int[][] equations =
        [
            [1, 1, 1, 2]
        ];

        int[][] expected =
        [
            [0, 0, 2],
            [0, 1, 1],
            [0, 2, 0],
            [1, 0, 1],
            [1, 1, 0],
            [2, 0, 0]
        ];

        IList<int[]> solutions = FbUtils.GetAllSolutions(equations);

        AssertSolutionsEqual(expected, solutions);
    }

    [Fact]
    public void ShouldIterateSolutionsFromIterableAndIterator()
    {
        int[][] equations =
        [
            [2, 0, 2],
            [0, 1, 1]
        ];

        int[][] expected =
        [
            [1, 1]
        ];

        IEnumerable<int[]> iterable = FbUtils.Iterable(equations);

        int[][] firstPass = iterable.Select(solution => solution.ToArray()).ToArray();
        int[][] secondPass = iterable.Select(solution => solution.ToArray()).ToArray();

        AssertSolutionsEqual(expected, firstPass);
        AssertSolutionsEqual(expected, secondPass);

        IEnumerator<int[]> iterator = FbUtils.Iterator(equations);

        Assert.True(iterator.MoveNext());
        Assert.Equal(expected[0], iterator.Current);
        Assert.False(iterator.MoveNext());
    }

    private static void AssertSolutionsEqual(IReadOnlyCollection<int[]> expected, IEnumerable<int[]> actual)
    {
        List<int[]> expectedSorted = expected.Select(solution => solution.ToArray()).ToList();
        List<int[]> actualSorted = actual.Select(solution => solution.ToArray()).ToList();

        expectedSorted.Sort(FbUtils.SolutionComparator);
        actualSorted.Sort(FbUtils.SolutionComparator);

        Assert.Equal(expectedSorted.Count, actualSorted.Count);
        for (int i = 0; i < expectedSorted.Count; i++)
        {
            Assert.Equal(expectedSorted[i], actualSorted[i]);
        }
    }
}
