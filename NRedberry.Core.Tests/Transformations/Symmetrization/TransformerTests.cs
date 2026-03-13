using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class TransformerTests
{
    [Fact]
    public void ShouldApplyTransformationsAtSelectedTraverseState()
    {
        Transformer transformer = new(
            TraverseState.Entering,
            [new ReplaceATransformation()]);

        NRedberry.Tensors.Tensor actual = transformer.Transform(TensorApi.Parse("a+b"));
        string[] terms = actual.ToString(OutputFormat.Redberry).Split('+', StringSplitOptions.RemoveEmptyEntries);
        Array.Sort(terms, StringComparer.Ordinal);

        Assert.Equal(["b", "x"], terms);
    }

    [Fact]
    public void ShouldRespectTraverseGuide()
    {
        Transformer transformer = new(
            TraverseState.Entering,
            [new ReplaceATransformation()],
            TraverseGuide.ExceptFunctionsAndFields);

        NRedberry.Tensors.Tensor actual = transformer.Transform(TensorApi.Parse("Sin[a]"));

        Assert.Equal("Sin[a]", actual.ToString(OutputFormat.Redberry));
    }

    private sealed class ReplaceATransformation : ITransformation
    {
        public NRedberry.Tensors.Tensor Transform(NRedberry.Tensors.Tensor tensor)
        {
            return tensor.ToString(OutputFormat.Redberry) == "a"
                ? TensorApi.Parse("x")
                : tensor;
        }
    }
}
