using NRedberry.Physics.Feyncalc;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify1Test
{
    [Fact]
    public void ShouldThrowUntilDiracSimplify1IsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new DiracSimplify1(null!));
    }
}
