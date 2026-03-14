using NRedberry.Physics.Feyncalc;
using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify0Test
{
    [Fact]
    public void ShouldThrowUntilDiracSimplify0IsPorted()
    {
        Should.Throw<NotImplementedException>(() => new DiracSimplify0(null!));
    }
}
