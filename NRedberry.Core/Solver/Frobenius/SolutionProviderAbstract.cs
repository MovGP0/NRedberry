namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/SolutionProviderAbstract.java
 */

internal abstract class SolutionProviderAbstract : SolutionProvider
{
    protected SolutionProviderAbstract(SolutionProvider provider, int position, int[] coefficients)
    {
        throw new NotImplementedException();
    }

    public virtual bool Tick()
    {
        throw new NotImplementedException();
    }

    public virtual int[] CurrentRemainders()
    {
        throw new NotImplementedException();
    }

    public abstract int[] Take();
}
