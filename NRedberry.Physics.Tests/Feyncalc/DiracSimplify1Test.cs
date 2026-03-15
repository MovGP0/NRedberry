using NRedberry.Physics.Feyncalc;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify1Test
{
    [Fact(Skip = "Blocked by the unported SubstitutionTransformation dependency in NRedberry.Core.")]
    public void ShouldConstructTransformation()
    {
        DiracSimplify1 transformation = new(CreateDiracOptions());

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
