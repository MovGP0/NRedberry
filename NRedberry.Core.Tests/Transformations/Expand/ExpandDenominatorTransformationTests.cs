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

        NormalizeReciprocalSumOfProducts(actual.ToString(OutputFormat.Redberry)).ShouldBe("(a*c+b*c)**(-1)");
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        ExpandDenominatorTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("ExpandDenominator");
    }

    private static string NormalizeReciprocalSumOfProducts(string expression)
    {
        const string suffix = "**(-1)";
        expression.EndsWith(suffix, StringComparison.Ordinal).ShouldBeTrue();

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
