using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandNumeratorTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenNumeratorHasNothingToExpand()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a/c");
        NRedberry.Tensors.Tensor actual = ExpandNumeratorTransformation.Expand(tensor);

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        ExpandNumeratorTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("ExpandNumerator");
    }
}
