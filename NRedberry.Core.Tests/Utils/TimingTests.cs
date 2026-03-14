using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class TimingTests
{
    [Fact]
    public void ShouldMeasureJobsWithoutPrinting()
    {
        object[] result = Timing.Measure(new ConstantTimingJob<int>(42), false);

        (((long)result[0]) >= 0).ShouldBeTrue();
        result[1].ShouldBeOfType<int>().ShouldBe(42);
    }

    [Fact]
    public void ShouldMeasureMicroAndNanoJobs()
    {
        object[] micro = Timing.MeasureMicro(new ConstantTimingJob<string>("ok"), false);
        object[] nano = Timing.MeasureNano(new ConstantTimingJob<string>("ok"), false);

        (((long)micro[0]) >= 0).ShouldBeTrue();
        (((long)nano[0]) >= 0).ShouldBeTrue();
        micro[1].ShouldBeOfType<string>().ShouldBe("ok");
        nano[1].ShouldBeOfType<string>().ShouldBe("ok");
    }
}

public sealed class ConstantTimingJob<T>(T value) : Timing.ITimingJob<T>
{
    public T DoJob()
    {
        return value;
    }
}
