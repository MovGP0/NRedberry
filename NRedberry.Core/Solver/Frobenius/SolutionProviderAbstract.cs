namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/SolutionProviderAbstract.java
 */

internal abstract class SolutionProviderAbstract : SolutionProvider
{
    protected SolutionProviderAbstract(SolutionProvider provider, int position, int[] coefficients)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(coefficients);

        Provider = provider;
        Position = position;
        Coefficients = coefficients;
    }

    public virtual bool Tick()
    {
        CurrentSolution = Provider.Take();
        CurrentRemainder = Provider.CurrentRemainders();
        CurrentCounter = 0;
        return CurrentSolution is not null;
    }

    public virtual int[] CurrentRemainders()
    {
        var remainder = CurrentRemainder!;
        var remainders = new int[Coefficients.Length];
        for (int i = 0; i < Coefficients.Length; ++i)
        {
            remainders[i] = remainder[i] - Coefficients[i] * (CurrentCounter - 1);
        }

        return remainders;
    }

    public abstract int[] Take();

    protected int Position { get; }

    protected int[] Coefficients { get; }

    protected int[]? CurrentSolution { get; set; }

    protected int CurrentCounter { get; set; }

    protected int[]? CurrentRemainder { get; set; }

    private SolutionProvider Provider { get; }
}
