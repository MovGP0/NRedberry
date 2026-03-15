using NRedberry.Physics.Feyncalc;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplify1Test : AbstractFeynCalcTest
{
    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracSimplify1.")]
    public void ShouldConstructTransformation()
    {
        DiracSimplify1 transformation = new(CreateDiracOptions());

        transformation.ShouldNotBeNull();
    }

    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracSimplify1.")]
    public void ShouldSimplifyRepeatedContractionsToScalar()
    {
        SetUp(123L);

        var result = dSimplify1!.Transform(TensorFactory.Parse("G_a*G_a*G_b*G_c*G_b*G_c"));

        ShouldMatchTensor("-32", result);
    }

    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracSimplify1.")]
    public void ShouldSimplifySingleWrappedContraction()
    {
        SetUp(123L);

        var result = dSimplify1!.Transform(TensorFactory.Parse("G_{a}*G_{b}*G^{a}"));

        ShouldMatchTensor("-2*G_{b}", result);
    }

    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracSimplify1.")]
    public void ShouldSimplifyFourGammaWrappedContraction()
    {
        SetUp(123L);

        var result = dSimplify1!.Transform(TensorFactory.Parse("G_{a}*G_{b}*G_c*G_d*G^{a}"));

        ShouldMatchTensor("-2*G_d*G_c*G_b", result);
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
