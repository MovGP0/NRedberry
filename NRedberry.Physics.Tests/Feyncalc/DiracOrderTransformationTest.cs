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
        Tensor[] orderedGammas = ParseGammaArray(
            "G_{a}^{a'}_{b'}",
            "G_{b}^{b'}_{c'}");
        Tensor? ordered = DiracOrderTransformation.OrderArrayForTesting(
            options,
            orderedGammas);

        ordered.ShouldBeNull();
    }

    [Fact]
    public void ShouldOrderTwoGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = ParseGammaArray(
            "G_{b}^{a'}_{b'}",
            "G_{a}^{b'}_{c'}");
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        ShouldMatchTensor("2*g_{ba}*d^{a'}_{c'}-G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}", ordered);
    }

    [Fact]
    public void ShouldOrderThreeGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = ParseGammaArray(
            "G_{c}^{a'}_{b'}",
            "G_{b}^{b'}_{c'}",
            "G_{a}^{c'}_{d'}");
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        ShouldMatchTensor(
            "-G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}*G_{c}^{c'}_{d'}+2*g_{bc}*G_{a}^{a'}_{d'}-2*g_{ac}*G_{b}^{a'}_{d'}+2*g_{ab}*G_{c}^{a'}_{d'}",
            ordered);
    }

    [Fact]
    public void ShouldOrderPartiallyPermutedThreeGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = ParseGammaArray(
            "G_{c}^{a'}_{b'}",
            "G_{a}^{b'}_{c'}",
            "G_{b}^{c'}_{d'}");
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        ShouldMatchTensor(
            "G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}*G_{c}^{c'}_{d'}-2*g_{bc}*G_{a}^{a'}_{d'}+2*g_{ac}*G_{b}^{a'}_{d'}",
            ordered);
    }

    [Fact]
    public void ShouldOrderFourGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = ParseGammaArray(
            "G_{d}^{a'}_{b'}",
            "G_{c}^{b'}_{c'}",
            "G_{b}^{c'}_{d'}",
            "G_{a}^{d'}_{e'}");
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        ShouldMatchTensor(
            string.Concat(
                "G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}*G_{c}^{c'}_{d'}*G_{d}^{d'}_{e'}",
                "-2*g_{ab}*G_{c}^{a'}_{d'}*G_{d}^{d'}_{e'}",
                "+2*g_{ac}*G_{b}^{a'}_{d'}*G_{d}^{d'}_{e'}",
                "-2*g_{ad}*G_{b}^{a'}_{c'}*G_{c}^{c'}_{e'}",
                "-2*g_{bc}*G_{a}^{a'}_{d'}*G_{d}^{d'}_{e'}",
                "+4*g_{ad}*g_{bc}*d^{a'}_{e'}",
                "+2*g_{bd}*G_{a}^{a'}_{c'}*G_{c}^{c'}_{e'}",
                "-4*g_{ac}*g_{bd}*d^{a'}_{e'}",
                "-2*g_{cd}*G_{a}^{a'}_{b'}*G_{b}^{b'}_{e'}",
                "+4*g_{ab}*g_{cd}*d^{a'}_{e'}"),
            ordered);
    }

    [Fact]
    public void ShouldOrderFiveGammas()
    {
        ConfigureContext();

        DiracOptions options = CreateOptions();
        Tensor[] gammas = ParseGammaArray(
            "G_{e}^{a'}_{b'}",
            "G_{d}^{b'}_{c'}",
            "G_{c}^{c'}_{d'}",
            "G_{b}^{d'}_{e'}",
            "G_{a}^{e'}_{f'}");
        Tensor ordered = DiracOrderTransformation.OrderArrayForTesting(options, gammas)!;

        ShouldMatchTensor(
            string.Concat(
                "G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}*G_{c}^{c'}_{d'}*G_{d}^{d'}_{e'}*G_{e}^{e'}_{f'}",
                "-2*g_{ab}*G_{c}^{a'}_{d'}*G_{d}^{d'}_{e'}*G_{e}^{e'}_{f'}",
                "+2*g_{ac}*G_{b}^{a'}_{d'}*G_{d}^{d'}_{e'}*G_{e}^{e'}_{f'}",
                "-2*g_{ad}*G_{b}^{a'}_{c'}*G_{c}^{c'}_{e'}*G_{e}^{e'}_{f'}",
                "+2*g_{ae}*G_{b}^{a'}_{c'}*G_{c}^{c'}_{d'}*G_{d}^{d'}_{f'}",
                "-2*g_{bc}*G_{a}^{a'}_{d'}*G_{d}^{d'}_{e'}*G_{e}^{e'}_{f'}",
                "+2*g_{bd}*G_{a}^{a'}_{c'}*G_{c}^{c'}_{e'}*G_{e}^{e'}_{f'}",
                "-2*g_{be}*G_{a}^{a'}_{c'}*G_{c}^{c'}_{d'}*G_{d}^{d'}_{f'}",
                "-2*g_{cd}*G_{a}^{a'}_{b'}*G_{b}^{b'}_{e'}*G_{e}^{e'}_{f'}",
                "+2*g_{ce}*G_{a}^{a'}_{b'}*G_{b}^{b'}_{d'}*G_{d}^{d'}_{f'}",
                "-2*g_{de}*G_{a}^{a'}_{b'}*G_{b}^{b'}_{c'}*G_{c}^{c'}_{f'}",
                "+4*g_{be}*g_{cd}*G_{a}^{a'}_{f'}",
                "-4*g_{bd}*g_{ce}*G_{a}^{a'}_{f'}",
                "+4*g_{bc}*g_{de}*G_{a}^{a'}_{f'}",
                "-4*g_{ae}*g_{cd}*G_{b}^{a'}_{f'}",
                "+4*g_{ad}*g_{ce}*G_{b}^{a'}_{f'}",
                "-4*g_{ac}*g_{de}*G_{b}^{a'}_{f'}",
                "+4*g_{ae}*g_{bd}*G_{c}^{a'}_{f'}",
                "-4*g_{ad}*g_{be}*G_{c}^{a'}_{f'}",
                "+4*g_{ab}*g_{de}*G_{c}^{a'}_{f'}",
                "-4*g_{ae}*g_{bc}*G_{d}^{a'}_{f'}",
                "+4*g_{ac}*g_{be}*G_{d}^{a'}_{f'}",
                "-4*g_{ab}*g_{ce}*G_{d}^{a'}_{f'}",
                "+4*g_{ad}*g_{bc}*G_{e}^{a'}_{f'}",
                "-4*g_{ac}*g_{bd}*G_{e}^{a'}_{f'}",
                "+4*g_{ab}*g_{cd}*G_{e}^{a'}_{f'}"),
            ordered);
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

        comparison.ShouldBeLessThan(0);
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

    private static Tensor[] ParseGammaArray(params string[] gammaStrings)
    {
        ArgumentNullException.ThrowIfNull(gammaStrings);

        Tensor[] gammas = new Tensor[gammaStrings.Length];
        for (int i = 0; i < gammaStrings.Length; ++i)
        {
            gammas[i] = TensorApi.ParseSimple(gammaStrings[i]);
        }

        return gammas;
    }

    private static void ShouldMatchTensor(string expected, Tensor actual)
    {
        Tensor expectedTensor = ExpandAndEliminateTransformation.ExpandAndEliminate(TensorApi.Parse(expected));
        Tensor actualTensor = ExpandAndEliminateTransformation.ExpandAndEliminate(actual);
        TensorUtils.Equals(expectedTensor, actualTensor).ShouldBeTrue(
            $"Expected: {expectedTensor}{Environment.NewLine}Actual: {actualTensor}");
    }
}
