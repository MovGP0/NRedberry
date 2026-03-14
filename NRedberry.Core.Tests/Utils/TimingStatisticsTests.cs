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

        transformed.ShouldBeSameAs(payload);
        report.ShouldContain("identity");
        report.ShouldContain("1 invocations");
        report.ShouldContain("Total");
        timer.Invocations().ShouldBe(0);
        timer.ElapsedNanos().ShouldBe(0);
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

        report.ShouldContain("shared");
        report.ShouldContain("3,000ns");
        report.ShouldContain("0 invocations");
    }

    [Fact]
    public void ShouldUseTransformationFormattingWhenNameIsNotProvided()
    {
        TransformationWithTimer timer = new(new EchoTransformation("formatted"));

        timer.ToString("ignored-format").ShouldBe("formatted");
        timer.ToString().ShouldBe("formatted");
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
