using NRedberry.Physics.Feyncalc;
using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplifyTransformationTest
{
    [Fact]
    public void ShouldThrowUntilDiracSimplifyTransformationIsPorted()
    {
        Should.Throw<NotImplementedException>(() => new DiracSimplifyTransformation(null!));
    }
}
