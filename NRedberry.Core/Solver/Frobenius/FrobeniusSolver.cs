using NRedberry.Concurrent;

namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/FrobeniusSolver.java
 */

public sealed class FrobeniusSolver : IOutputPort<int[]>
{
    private readonly IOutputPort<int[]> _provider;

    public FrobeniusSolver(params int[][] equations)
    {
        if (equations.Length == 0)
        {
            throw new ArgumentException();
        }

        int length = equations[0].Length;
        if (length < 2)
        {
            throw new ArgumentException();
        }

        for (int i = 1; i < equations.Length; ++i)
        {
            if (equations[i].Length != length && !AssertEq(equations[i]))
            {
                throw new ArgumentException();
            }
        }

        int[] initialSolution = new int[length - 1];
        int zeroCoefficientsCount = 0;
        for (int i = 0; i < length - 1; ++i)
        {
            bool allZero = true;
            for (int j = 0; j < equations.Length; ++j)
            {
                if (equations[j][i] != 0)
                {
                    allZero = false;
                    break;
                }
            }

            if (allZero)
            {
                initialSolution[i] = -1;
                zeroCoefficientsCount++;
            }
        }

        int[] initialRemainders = new int[equations.Length];
        for (int j = 0; j < equations.Length; ++j)
        {
            initialRemainders[j] = equations[j][length - 1];
        }

        SolutionProvider dummy = new DummySolutionProvider(initialSolution, initialRemainders);

        int providersCount = length - 1 - zeroCoefficientsCount;
        SolutionProvider[] providers = new SolutionProvider[providersCount];
        int count = 0;
        for (int i = 0; i < length - 1; ++i)
        {
            if (initialSolution[i] == -1)
            {
                continue;
            }

            int[] coefficients = new int[equations.Length];
            for (int j = 0; j < equations.Length; ++j)
            {
                coefficients[j] = equations[j][i];
            }

            if (count == 0)
            {
                providers[count] = providersCount == 1
                    ? new FinalSolutionProvider(dummy, i, coefficients)
                    : new SingleSolutionProvider(dummy, i, coefficients);
            }
            else if (count == providersCount - 1)
            {
                providers[count] = new FinalSolutionProvider(providers[count - 1], i, coefficients);
            }
            else
            {
                providers[count] = new SingleSolutionProvider(providers[count - 1], i, coefficients);
            }

            count++;
        }

        _provider = new TotalSolutionProvider(providers);
    }

    public int[] Take()
    {
        return _provider.Take();
    }

    private static bool AssertEq(int[] equation)
    {
        foreach (int value in equation)
        {
            if (value < 0)
            {
                return false;
            }
        }

        return true;
    }
}
