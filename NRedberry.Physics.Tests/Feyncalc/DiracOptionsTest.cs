using NRedberry.Physics.Feyncalc;
using Shouldly;
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
