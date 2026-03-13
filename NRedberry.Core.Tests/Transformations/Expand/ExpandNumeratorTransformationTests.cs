using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandNumeratorTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenNumeratorHasNothingToExpand()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a/c");
        NRedberry.Tensors.Tensor actual = ExpandNumeratorTransformation.Expand(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("ExpandNumerator", ExpandNumeratorTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
