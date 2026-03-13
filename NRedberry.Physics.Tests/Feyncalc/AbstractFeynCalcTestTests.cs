using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class AbstractFeynCalcTestTests
{
    [Fact]
    public void ShouldThrowDuringSetupUntilDiracOptionsIsPorted()
    {
        AbstractFeynCalcTestProbe probe = new();
        Assert.Throws<NotImplementedException>(() => probe.SetUp(1));
    }
}

public sealed class AbstractFeynCalcTestProbe : AbstractFeynCalcTest
{
}
