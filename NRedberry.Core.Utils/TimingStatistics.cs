namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.TimingStatistics.
/// </summary>
public sealed class TimingStatistics
{
    private long samplesCount;
    private long minimum = long.MaxValue;
    private long maximum = long.MinValue;
    private double sum;
    private double sumOfSquares;

    public TimingStatistics()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void AddSample(long value)
    {
        throw new NotImplementedException();
    }

    public long SamplesCount => throw new NotImplementedException();

    public long Minimum => throw new NotImplementedException();

    public long Maximum => throw new NotImplementedException();

    public double Mean => throw new NotImplementedException();

    public double Variance => throw new NotImplementedException();

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
