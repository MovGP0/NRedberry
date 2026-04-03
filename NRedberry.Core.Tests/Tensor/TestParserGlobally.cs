using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorCC = NRedberry.Tensors.CC;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensor;

public sealed class TestParserGlobally
{
    private static readonly string[] BaseExpressions =
    [
        "a+b",
        "A_mn*(B^na+C^na)",
        "Sin[x+y]+Cos[z]",
        "g_mn*k^m*k^n",
        "A_i^j = B_i^j + C_i^j"
    ];

    private static readonly string[] SameVarianceExpressions =
    [
        "T_aa",
        "F^aa",
        "A_ab + B_ab"
    ];

    private static readonly string[] KnownExpressions =
    [
        "A_m^n*B_n^p",
        "Power[a+b,2]",
        "Sin[x]+Cos[y]+Tan[z]"
    ];

    private static readonly string[] AdditionalExpressions =
    [
        "M_i^j = N_i^j + P_i^j",
        "g_ab*g^bc",
        "Power[1/2,2]+x"
    ];

    [Fact]
    public void ShouldParseAllExpressionsInTestDirectory()
    {
        ParseAll(BaseExpressions, allowSameVariance: false);
    }

    [Fact]
    public void ShouldParseAllExpressionsAllowingSameVariance()
    {
        ParseAll(BaseExpressions, allowSameVariance: true);
    }

    [Fact]
    public void ShouldParseAllExpressionsWithSameVariance()
    {
        ParseAll(SameVarianceExpressions, allowSameVariance: true);
    }

    [Fact]
    public void ShouldParseKnownTestStrings()
    {
        ParseAll(KnownExpressions, allowSameVariance: false);
    }

    [Fact]
    public void ShouldParseAdditionalTestStrings()
    {
        ParseAll(AdditionalExpressions, allowSameVariance: false);
    }

    [Fact]
    public void ShouldRoundTripParsedTensorStrings()
    {
        foreach (string expression in new[] { "a+b", "Sin[x]+Cos[y]" })
        {
            TensorType tensor = TensorApi.Parse(expression);
            TensorType reparsed = TensorApi.Parse(tensor.ToString());

            TensorUtils.EqualsExactly(tensor, reparsed).ShouldBeTrue();
        }
    }

    private static void ParseAll(IEnumerable<string> expressions, bool allowSameVariance)
    {
        OutputFormat originalFormat = TensorCC.GetDefaultOutputFormat();
        bool originalSameVariance = TensorCC.GetParserAllowsSameVariance();

        try
        {
            TensorCC.SetDefaultOutputFormat(OutputFormat.Redberry);
            TensorCC.SetParserAllowsSameVariance(allowSameVariance);

            foreach (string expression in expressions)
            {
                TensorType tensor = TensorApi.Parse(expression);
                tensor.ShouldNotBeNull();
            }
        }
        finally
        {
            TensorCC.SetParserAllowsSameVariance(originalSameVariance);
            TensorCC.SetDefaultOutputFormat(originalFormat);
        }
    }
}
