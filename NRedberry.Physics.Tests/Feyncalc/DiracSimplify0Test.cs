using NRedberry.Physics.Feyncalc;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify0Test
{
    [Fact]
    public void ShouldConstructAndExposeName()
    {
        DiracOptions options = new()
        {
            GammaMatrix = TensorFactory.ParseSimple("G_a^a'_b'"),
            Gamma5 = TensorFactory.ParseSimple("G5^a'_b'"),
            LeviCivita = TensorFactory.ParseSimple("e_abcd")
        };

        DiracSimplify0 transformation = new(options);

        transformation.ToString(OutputFormat.Redberry).ShouldBe("DiracSimplify0");
    }
}
