namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/SingleSolutionProvider.java
 */

internal sealed class SingleSolutionProvider : SolutionProviderAbstract
{
    public SingleSolutionProvider(SolutionProvider provider, int position, int[] coefficients)
        : base(provider, position, coefficients)
    {
        throw new NotImplementedException();
    }

    public override int[] Take()
    {
        throw new NotImplementedException();
    }
}
