using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using Xunit;
using TensorCC = NRedberry.Tensors.CC;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracOrderTransformationTest
{
    [Fact]
    public void ShouldReturnNullForAlreadyOrderedGammaArray()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] orderedGammas = DiracOrderTransformation.CreateArrayForTesting(options, [0, 1]);
        Tensor? ordered = DiracOrderTransformation.OrderArrayForTesting(
            options,
            orderedGammas);

        Assert.Null(ordered);
    }

    [Fact]
    public void ShouldOrderTwoGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = DiracOrderTransformation.CreateArrayForTesting(options, [1, 0]);
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        AssertTensorEquals("2*g_{ba}*d^{a'}_{c'}-G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}", ordered);
    }

    [Fact]
    public void ShouldOrderThreeGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = DiracOrderTransformation.CreateArrayForTesting(options, [2, 1, 0]);
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        AssertTensorEquals(
            "-G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}*G_{c}^{c'}_{d'}+2*g_{bc}*G_{a}^{a'}_{d'}-2*g_{ac}*G_{b}^{a'}_{d'}+2*g_{ab}*G_{c}^{a'}_{d'}",
            ordered);
    }

    [Fact]
    public void ShouldCreatePermutedCanonicalGammaArray()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] created = DiracOrderTransformation.CreateArrayForTesting(options, [1, 0]);
        Tensor product = TensorApi.Multiply(created);

        AssertTensorEquals("G_{b}^{a'}_{b'}*G_{a}^{b'}_{c'}", product);
    }

    [Fact]
    public void ShouldCompareContractedGammasBeforeUncontracted()
    {
        ConfigureContext();

        SimpleTensor gammaA = TensorApi.ParseSimple("G_a^a'_b'");
        SimpleTensor gammaB = TensorApi.ParseSimple("G_b^a'_b'");
        SimpleTensor momentum = TensorApi.ParseSimple("p_a");
        int comparison = DiracOrderTransformation.CompareGammasForTesting(
            gammaA,
            gammaA.Indices[IndexType.LatinLower, 0],
            momentum,
            gammaB,
            gammaB.Indices[IndexType.LatinLower, 0],
            null);

        Assert.True(comparison < 0);
    }

    private static DiracOptions CreateOptions()
    {
        return new DiracOptions
        {
            GammaMatrix = TensorApi.ParseSimple("G_a^a'_b'")
        };
    }

    private static void ConfigureContext()
    {
        TensorCC.Reset();
        TensorCC.SetDefaultOutputFormat(OutputFormat.SimpleRedberry);
        TensorCC.SetParserAllowsSameVariance(true);
    }

    private static void AssertTensorEquals(string expected, Tensor actual)
    {
        Tensor expectedTensor = ExpandAndEliminateTransformation.ExpandAndEliminate(TensorApi.Parse(expected));
        Tensor actualTensor = ExpandAndEliminateTransformation.ExpandAndEliminate(actual);
        Assert.True(
            TensorUtils.Equals(expectedTensor, actualTensor),
            $"Expected: {expectedTensor}{Environment.NewLine}Actual: {actualTensor}");
    }
}
