using NRedberry.Physics.Feyncalc;
using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify1Test
{
    [Fact]
    public void ShouldThrowUntilDiracSimplify1IsPorted()
    {
        Should.Throw<NotImplementedException>(() => new DiracSimplify1(null!));
    }
}
