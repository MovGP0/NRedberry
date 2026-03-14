using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class TimingTests
{
    [Fact]
    public void ShouldMeasureJobsWithoutPrinting()
    {
        object[] result = Timing.Measure(new ConstantTimingJob<int>(42), false);

        (long)result[0] >= 0.ShouldBeTrue();
        Assert.IsType<int>(result[1]).ShouldBe(42);
    }

    [Fact]
    public void ShouldMeasureMicroAndNanoJobs()
    {
        object[] micro = Timing.MeasureMicro(new ConstantTimingJob<string>("ok"), false);
        object[] nano = Timing.MeasureNano(new ConstantTimingJob<string>("ok"), false);

        (long)micro[0] >= 0.ShouldBeTrue();
        (long)nano[0] >= 0.ShouldBeTrue();
        Assert.IsType<string>(micro[1]).ShouldBe("ok");
        Assert.IsType<string>(nano[1]).ShouldBe("ok");
    }
}

public sealed class ConstantTimingJob<T>(T value) : Timing.ITimingJob<T>
{
    public T DoJob()
    {
        return value;
    }
}
