using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class AbstractExpandNumeratorDenominatorTransformationTests
{
    [Fact]
    public void ShouldTransformProductsInsideSums()
    {
        TestExpandNumeratorDenominatorTransformation transformation = new();

        NRedberry.Tensors.Tensor actual = transformation.Transform(TensorApi.Parse("a+b*c"));

        NormalizeSum(actual.ToString(OutputFormat.Redberry)).ShouldBe(["a", "p"]);
    }

    [Fact]
    public void ShouldUseTypeNameForDefaultStringRepresentation()
    {
        TestExpandNumeratorDenominatorTransformation transformation = new();

        transformation.ToString(OutputFormat.Redberry).ShouldBe(nameof(TestExpandNumeratorDenominatorTransformation));
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
