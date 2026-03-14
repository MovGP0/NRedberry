using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class AbstractFeynCalcTestTests
{
    [Fact]
    public void ShouldThrowDuringSetupUntilDiracOptionsIsPorted()
    {
        AbstractFeynCalcTestProbe probe = new();
        Should.Throw<NotImplementedException>(() => probe.SetUp(1));
    }
}

public sealed class AbstractFeynCalcTestProbe : AbstractFeynCalcTest;
