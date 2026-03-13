using NRedberry.Physics.Feyncalc;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracTraceTransformationTest
{
    [Fact]
    public void ShouldThrowUntilDiracTraceTransformationIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new DiracTraceTransformation(null!));
    }
}
