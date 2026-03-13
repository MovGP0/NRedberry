using NRedberry.Physics.Feyncalc;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplifyTransformationTest
{
    [Fact]
    public void ShouldThrowUntilDiracSimplifyTransformationIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new DiracSimplifyTransformation(null!));
    }
}
