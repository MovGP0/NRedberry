using NRedberry.Physics.Feyncalc;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify0Test
{
    [Fact]
    public void ShouldThrowUntilDiracSimplify0IsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new DiracSimplify0(null!));
    }
}
