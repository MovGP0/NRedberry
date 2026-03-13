using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class TimingStatisticsTests
{
    [Fact]
    public void ShouldTrackTransformationsAndResetLiveCountersAfterCollection()
    {
        TransformationWithTimer timer = new(new EchoTransformation(), "identity");
        TimingStatistics statistics = new();
        object payload = new();

        statistics.Track(timer);
        timer.IncrementNanos(1_500);
        object transformed = timer.Transform(payload);

        string report = statistics.ToStringNanos();

        Assert.Same(payload, transformed);
        Assert.Contains("identity", report);
        Assert.Contains("1 invocations", report);
        Assert.Contains("Total", report);
        Assert.Equal(0, timer.Invocations());
        Assert.Equal(0, timer.ElapsedNanos());
    }

    [Fact]
    public void ShouldMergeTimingStatistics()
    {
        TimingStatistics first = new();
        TimingStatistics second = new();
        TransformationWithTimer firstTimer = new(new EchoTransformation("shared"));
        TransformationWithTimer secondTimer = new(new EchoTransformation("shared"));

        first.Track(firstTimer);
        firstTimer.IncrementNanos(1_000);

        second.Track(secondTimer);
        secondTimer.IncrementNanos(2_000);

        first.Merge(second);
        string report = first.ToStringNanos();

        Assert.Contains("shared", report);
        Assert.Contains("3,000ns", report);
        Assert.Contains("0 invocations", report);
    }

    [Fact]
    public void ShouldUseTransformationFormattingWhenNameIsNotProvided()
    {
        TransformationWithTimer timer = new(new EchoTransformation("formatted"));

        Assert.Equal("formatted", timer.ToString("ignored-format"));
        Assert.Equal("formatted", timer.ToString());
    }
}

public sealed class EchoTransformation(string text = "identity") : ITimerTransformation
{
    public object Transform(object value)
    {
        return value;
    }

    public string ToString(object outputFormat)
    {
        return text;
    }

    public override string ToString()
    {
        return text;
    }
}
