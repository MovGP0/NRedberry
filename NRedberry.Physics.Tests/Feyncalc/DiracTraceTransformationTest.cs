using NRedberry.Physics.Feyncalc;
using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracTraceTransformationTest
{
    [Fact]
    public void ShouldThrowUntilDiracTraceTransformationIsPorted()
    {
        Should.Throw<NotImplementedException>(() => new DiracTraceTransformation(null!));
    }
}
