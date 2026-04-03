using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandTransformationTests
{
    [Fact]
    public void ShouldExpandSimpleProductOfSum()
    {
        NRedberry.Tensors.Tensor actual = ExpandTransformation.Expand(TensorApi.Parse("(a+b)*c"));

        NormalizeSumOfProducts(actual.ToString(OutputFormat.Redberry)).ShouldBe(["a*c", "b*c"]);
    }

    [Fact]
    public void ShouldSkipExpansionInsideFunctionsByDefault()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Sin[(a+b)*c]");

        NRedberry.Tensors.Tensor actual = ExpandTransformation.Expand(tensor);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        ExpandTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("Expand");
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
