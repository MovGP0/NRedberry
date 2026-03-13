using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandAllTransformationTests
{
    [Fact]
    public void ShouldExpandSimpleProductOfSums()
    {
        NRedberry.Tensors.Tensor actual = ExpandAllTransformation.Expand(TensorApi.Parse("(a+b)*(c+d)"));

        Assert.Equal(
            ["a*c", "a*d", "b*c", "b*d"],
            NormalizeSumOfProducts(actual.ToString(OutputFormat.Redberry)));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("ExpandAll", ExpandAllTransformation.Instance.ToString(OutputFormat.Redberry));
    }

    private static string[] NormalizeSumOfProducts(string expression)
    {
        return expression
            .Split('+', StringSplitOptions.RemoveEmptyEntries)
            .Select(NormalizeProduct)
            .OrderBy(term => term, StringComparer.Ordinal)
            .ToArray();
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
