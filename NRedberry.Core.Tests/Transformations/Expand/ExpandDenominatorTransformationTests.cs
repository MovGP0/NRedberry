using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandDenominatorTransformationTests
{
    [Fact]
    public void ShouldExpandSimpleDenominator()
    {
        NRedberry.Tensors.Tensor actual = ExpandDenominatorTransformation.Expand(TensorApi.Parse("1/((a+b)*c)"));

        Assert.Equal(
            "(a*c+b*c)**(-1)",
            NormalizeReciprocalSumOfProducts(actual.ToString(OutputFormat.Redberry)));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("ExpandDenominator", ExpandDenominatorTransformation.Instance.ToString(OutputFormat.Redberry));
    }

    private static string NormalizeReciprocalSumOfProducts(string expression)
    {
        const string suffix = "**(-1)";
        Assert.EndsWith(suffix, expression, StringComparison.Ordinal);

        string body = expression[..^suffix.Length];
        body = body.Trim('(', ')');
        string normalizedBody = string.Join(
            "+",
            body
                .Split('+', StringSplitOptions.RemoveEmptyEntries)
                .Select(NormalizeProduct)
                .OrderBy(term => term, StringComparer.Ordinal));

        return string.Concat("(", normalizedBody, ")", suffix);
    }

    private static string NormalizeProduct(string expression)
    {
        return string.Join(
            "*",
            expression
                .Split('*', StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(factor => factor, StringComparer.Ordinal));
    }
}
