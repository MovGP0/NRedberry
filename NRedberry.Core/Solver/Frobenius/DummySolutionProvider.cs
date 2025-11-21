namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/DummySolutionProvider.java
 */

internal sealed class DummySolutionProvider : SolutionProvider
{
    public DummySolutionProvider(int[] solution, int[] currentRemainder)
    {
        throw new NotImplementedException();
    }

    public bool Tick()
    {
        throw new NotImplementedException();
    }

    public int[] Take()
    {
        throw new NotImplementedException();
    }

    public int[] CurrentRemainders()
    {
        throw new NotImplementedException();
    }
}
