namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/DummySolutionProvider.java
 */

internal sealed class DummySolutionProvider : SolutionProvider
{
    public DummySolutionProvider(int[] solution, int[] currentRemainder)
    {
        ArgumentNullException.ThrowIfNull(solution);
        ArgumentNullException.ThrowIfNull(currentRemainder);

        _solution = solution;
        _currentRemainder = currentRemainder;
    }

    public bool Tick()
    {
        return _solution is not null;
    }

    public int[] Take()
    {
        if (_solution is null)
        {
            return null!;
        }

        var ret = _solution;
        _solution = null;
        return ret;
    }

    public int[] CurrentRemainders()
    {
        return _currentRemainder;
    }

    private int[]? _solution;

    private int[] _currentRemainder;
}
