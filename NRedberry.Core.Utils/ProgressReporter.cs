namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.ProgressReporter.
/// </summary>
public sealed class ProgressReporter
{
    private readonly TextWriter output = TextWriter.Null;
    private readonly string prefix = string.Empty;
    private readonly long totalWork;
    private long currentWork;
    private int previousPercent = -1;

    public ProgressReporter(TextWriter output, string prefix, long totalWork)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Report(long workDone)
    {
        throw new NotImplementedException();
    }

    public void Complete()
    {
        throw new NotImplementedException();
    }
}
