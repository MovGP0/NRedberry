using NRedberry.Physics.Feyncalc;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracTraceTransformationTest : AbstractFeynCalcTest
{
    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracTraceTransformation.")]
    public void ShouldConstructTransformation()
    {
        DiracTraceTransformation transformation = new(CreateDiracOptions());

        transformation.ShouldNotBeNull();
    }

    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracTraceTransformation.")]
    public void ShouldEvaluateTraceOfFourGammas()
    {
        SetUp(123L);

        var result = dTrace!.Transform(TensorFactory.Parse("Tr[G_a*G_b*G_c*G_d]"));

        ShouldMatchTensor("-4*g_{ac}*g_{bd}+4*g_{ad}*g_{bc}+4*g_{ab}*g_{cd}", result);
    }

    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracTraceTransformation.")]
    public void ShouldEvaluateTraceOfFourGammasWithGamma5()
    {
        SetUp(123L);

        var result = dTrace!.Transform(TensorFactory.Parse("Tr[G_a*G_b*G_c*G_d*G5]"));

        ShouldMatchTensor("(-4*I)*e_{abcd}", result);
    }

    [Fact(Skip = "Blocked by DiracOptions/SubstitutionTransformation(Expression) port gaps outside DiracTraceTransformation.")]
    public void ShouldReturnZeroForOddTraceWithGamma5()
    {
        SetUp(123L);

        var result = dTrace!.Transform(TensorFactory.Parse("Tr[G_a*G_b*G_c*G5]"));

        ShouldMatchTensor("0", result);
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
