using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class TimingTests
{
    [Fact]
    public void ShouldMeasureJobsWithoutPrinting()
    {
        object[] result = Timing.Measure(new ConstantTimingJob<int>(42), false);

        Assert.True((long)result[0] >= 0);
        Assert.Equal(42, Assert.IsType<int>(result[1]));
    }

    [Fact]
    public void ShouldMeasureMicroAndNanoJobs()
    {
        object[] micro = Timing.MeasureMicro(new ConstantTimingJob<string>("ok"), false);
        object[] nano = Timing.MeasureNano(new ConstantTimingJob<string>("ok"), false);

        Assert.True((long)micro[0] >= 0);
        Assert.True((long)nano[0] >= 0);
        Assert.Equal("ok", Assert.IsType<string>(micro[1]));
        Assert.Equal("ok", Assert.IsType<string>(nano[1]));
    }
}

public sealed class ConstantTimingJob<T>(T value) : Timing.ITimingJob<T>
{
    public T DoJob()
    {
        return value;
    }
}
