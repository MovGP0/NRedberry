using NRedberry.Solver.Frobenius;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FbUtilsTests
{
    [Fact]
    public void ShouldCompareSolutionsLexicographically()
    {
        int result = FbUtils.SolutionComparator.Compare([1, 2, 3], [1, 2, 4]);

        (result < 0).ShouldBeTrue();
        FbUtils.SolutionComparator.Compare([1, 2, 3], [1, 2, 3]).ShouldBe(0);
        (FbUtils.SolutionComparator.Compare([1, 3, 0], [1, 2, 9]) > 0).ShouldBeTrue();
    }

    [Fact]
    public void ShouldThrowWhenComparingDifferentSolutionSizes()
    {
        Should.Throw<ArgumentException>(() => FbUtils.SolutionComparator.Compare([1, 2], [1]));
    }

    [Fact]
    public void ShouldThrowWhenGetAllSolutionsInputIsNull()
    {
        Should.Throw<ArgumentNullException>(() => FbUtils.GetAllSolutions(null!));
    }

    [Fact]
    public void ShouldThrowWhenIterableInputIsNull()
    {
        Should.Throw<ArgumentNullException>(() => FbUtils.Iterable(null!));
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

        ShouldHaveSolutions(expected, solutions);
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

        ShouldHaveSolutions(expected, firstPass);
        ShouldHaveSolutions(expected, secondPass);

        IEnumerator<int[]> iterator = FbUtils.Iterator(equations);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(expected[0]);
        iterator.MoveNext().ShouldBeFalse();
    }

    private static void ShouldHaveSolutions(IReadOnlyCollection<int[]> expected, IEnumerable<int[]> actual)
    {
        List<int[]> expectedSorted = expected.Select(solution => solution.ToArray()).ToList();
        List<int[]> actualSorted = actual.Select(solution => solution.ToArray()).ToList();

        expectedSorted.Sort(FbUtils.SolutionComparator);
        actualSorted.Sort(FbUtils.SolutionComparator);

        actualSorted.Count.ShouldBe(expectedSorted.Count);
        for (int i = 0; i < expectedSorted.Count; i++)
        {
            actualSorted[i].ShouldBe(expectedSorted[i]);
        }
    }
}
