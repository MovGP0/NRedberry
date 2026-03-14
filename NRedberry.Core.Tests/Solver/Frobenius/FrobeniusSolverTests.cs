using System;
using System.Collections.Generic;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FrobeniusSolverTests
{
    [Fact]
    public void ShouldThrowWhenNoEquationsProvided()
    {
        Should.Throw<ArgumentException>(() => new FrobeniusSolver());
    }

    [Fact]
    public void ShouldThrowWhenEquationLengthIsLessThanTwo()
    {
        Should.Throw<ArgumentException>(() => new FrobeniusSolver([1]));
    }

    [Fact]
    public void ShouldThrowWhenEquationLengthsDiffer()
    {
        Should.Throw<ArgumentException>(() => new FrobeniusSolver([1, 2, 3], [1, 2]));
    }

    [Fact]
    public void ShouldThrowWhenAnyCoefficientIsNegative()
    {
        Should.Throw<ArgumentException>(() => new FrobeniusSolver([1, -2, 3]));
    }

    [Fact]
    public void ShouldReturnAllSolutionsForSimpleEquation()
    {
        FrobeniusSolver solver = new([1, 1, 1, 2]);

        List<int[]> solutions = TakeAllSolutions(solver);

        solutions.Count.ShouldBe(6);
        solutions[0].ShouldBe([0, 0, 2]);
        solutions[1].ShouldBe([0, 1, 1]);
        solutions[2].ShouldBe([0, 2, 0]);
        solutions[3].ShouldBe([1, 0, 1]);
        solutions[4].ShouldBe([1, 1, 0]);
        solutions[5].ShouldBe([2, 0, 0]);
    }

    [Fact]
    public void ShouldReturnSolutionsForMultiEquationSystem()
    {
        int[][] equations =
        [
            [12, 16, 20, 27, 123],
            [1, 0, 3, 0, 12]
        ];

        FrobeniusSolver solver = new(equations);

        List<int[]> solutions = TakeAllSolutions(solver);

        solutions.Count.ShouldBe(2);
        solutions[0].ShouldBe([0, 1, 4, 1]);
        solutions[1].ShouldBe([3, 0, 3, 1]);
    }

    [Fact]
    public void ShouldUseMinusOneForVariablesWithAllZeroCoefficients()
    {
        FrobeniusSolver solver = new([0, 1, 2]);

        List<int[]> solutions = TakeAllSolutions(solver);

        solutions.ShouldHaveSingleItem();
        solutions[0].ShouldBe([-1, 2]);
    }

    private static List<int[]> TakeAllSolutions(FrobeniusSolver solver)
    {
        List<int[]> result = [];
        int[]? solution;
        while ((solution = solver.Take()) is not null)
        {
            result.Add(solution);
        }

        return result;
    }
}
