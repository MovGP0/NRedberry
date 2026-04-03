using NRedberry.Physics.Feyncalc;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracOptionsTest
{
    [Fact]
    public void ShouldThrowUntilDiracOptionsIsPorted()
    {
        Should.Throw<NotImplementedException>(() => new DiracOptions());
    }
}
