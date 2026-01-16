namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/SingleSolutionProvider.java
 */

internal sealed class SingleSolutionProvider : SolutionProviderAbstract
{
    public SingleSolutionProvider(SolutionProvider provider, int position, int[] coefficients)
        : base(provider, position, coefficients)
    {
    }

    public override int[] Take()
    {
        if (CurrentSolution is null)
        {
            return null!;
        }

        for (int i = 0; i < Coefficients.Length; ++i)
        {
            int remainder = CurrentRemainder![i] - Coefficients[i] * CurrentCounter;
            if (remainder < 0)
            {
                CurrentCounter = 0;
                CurrentSolution = null;
                return null!;
            }
        }

        int[] solution = (int[])CurrentSolution.Clone();
        solution[Position] += CurrentCounter;
        ++CurrentCounter;
        return solution;
    }
}
