using NRedberry.Physics.Feyncalc;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracTraceTransformationTest
{
    [Fact]
    public void ShouldConstructTransformation()
    {
        DiracTraceTransformation transformation = new(CreateDiracOptions());

        transformation.ShouldNotBeNull();
    }

    private static DiracOptions CreateDiracOptions()
    {
        return new DiracOptions
        {
            GammaMatrix = TensorFactory.ParseSimple("G^a'_b'a"),
            Gamma5 = TensorFactory.ParseSimple("G5^a'_b'"),
            LeviCivita = TensorFactory.ParseSimple("e_abcd")
        };
    }
}
