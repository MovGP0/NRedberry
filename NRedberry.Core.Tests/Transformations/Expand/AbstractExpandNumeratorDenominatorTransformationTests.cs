using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class AbstractExpandNumeratorDenominatorTransformationTests
{
    [Fact]
    public void ShouldTransformProductsInsideSums()
    {
        TestExpandNumeratorDenominatorTransformation transformation = new();

        NRedberry.Tensors.Tensor actual = transformation.Transform(TensorApi.Parse("a+b*c"));

        Assert.Equal(["a", "p"], NormalizeSum(actual.ToString(OutputFormat.Redberry)));
    }

    [Fact]
    public void ShouldUseTypeNameForDefaultStringRepresentation()
    {
        TestExpandNumeratorDenominatorTransformation transformation = new();

        Assert.Equal(
            nameof(TestExpandNumeratorDenominatorTransformation),
            transformation.ToString(OutputFormat.Redberry));
    }

    private sealed class TestExpandNumeratorDenominatorTransformation
        : AbstractExpandNumeratorDenominatorTransformation
    {
        protected override NRedberry.Tensors.Tensor ExpandProduct(NRedberry.Tensors.Tensor tensor)
        {
            return TensorApi.Parse("p");
        }
    }

    private static string[] NormalizeSum(string expression)
    {
        return expression
            .Split('+', StringSplitOptions.RemoveEmptyEntries)
            .OrderBy(term => term, StringComparer.Ordinal)
            .ToArray();
    }
}
