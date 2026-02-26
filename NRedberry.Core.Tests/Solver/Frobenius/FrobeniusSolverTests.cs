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
        Assert.Throws<ArgumentException>(() => new FrobeniusSolver());
    }

    [Fact]
    public void ShouldThrowWhenEquationLengthIsLessThanTwo()
    {
        Assert.Throws<ArgumentException>(() => new FrobeniusSolver([1]));
    }

    [Fact]
    public void ShouldThrowWhenEquationLengthsDiffer()
    {
        Assert.Throws<ArgumentException>(() => new FrobeniusSolver([1, 2, 3], [1, 2]));
    }

    [Fact]
    public void ShouldThrowWhenAnyCoefficientIsNegative()
    {
        Assert.Throws<ArgumentException>(() => new FrobeniusSolver([1, -2, 3]));
    }

    [Fact]
    public void ShouldReturnAllSolutionsForSimpleEquation()
    {
        FrobeniusSolver solver = new([1, 1, 1, 2]);

        List<int[]> solutions = TakeAllSolutions(solver);

        Assert.Equal(6, solutions.Count);
        Assert.Equal([0, 0, 2], solutions[0]);
        Assert.Equal([0, 1, 1], solutions[1]);
        Assert.Equal([0, 2, 0], solutions[2]);
        Assert.Equal([1, 0, 1], solutions[3]);
        Assert.Equal([1, 1, 0], solutions[4]);
        Assert.Equal([2, 0, 0], solutions[5]);
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

        Assert.Equal(2, solutions.Count);
        Assert.Equal([0, 1, 4, 1], solutions[0]);
        Assert.Equal([3, 0, 3, 1], solutions[1]);
    }

    [Fact]
    public void ShouldUseMinusOneForVariablesWithAllZeroCoefficients()
    {
        FrobeniusSolver solver = new([0, 1, 2]);

        List<int[]> solutions = TakeAllSolutions(solver);

        Assert.Single(solutions);
        Assert.Equal([-1, 2], solutions[0]);
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
