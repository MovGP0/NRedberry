namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/FinalSolutionProvider.java
 */

internal sealed class FinalSolutionProvider : SolutionProviderAbstract
{
    public FinalSolutionProvider(SolutionProvider provider, int position, int[] coefficients)
        : base(provider, position, coefficients)
    {
    }

    public override int[] Take()
    {
        if (CurrentSolution is null)
        {
            return null!;
        }

        int i = 0;
        while (i < Coefficients.Length && Coefficients[i++] == 0)
        {
        }

        --i;

        System.Diagnostics.Debug.Assert(i == 0 || i != Coefficients.Length);

        var remainder = CurrentRemainder!;
        if (remainder[i] % Coefficients[i] != 0)
        {
            CurrentSolution = null;
            return null!;
        }

        CurrentCounter = remainder[i] / Coefficients[i];
        for (i = 0; i < Coefficients.Length; ++i)
        {
            if (Coefficients[i] == 0)
            {
                if (remainder[i] != 0)
                {
                    CurrentSolution = null;
                    return null!;
                }
            }
            else if (remainder[i] % Coefficients[i] != 0)
            {
                CurrentSolution = null;
                return null!;
            }
            else if (remainder[i] / Coefficients[i] != CurrentCounter)
            {
                CurrentSolution = null;
                return null!;
            }
        }

        int[] solution = (int[])CurrentSolution.Clone();
        solution[Position] += CurrentCounter;
        CurrentSolution = null;
        return solution;
    }
}
