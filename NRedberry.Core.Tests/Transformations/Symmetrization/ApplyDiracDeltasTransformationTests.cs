using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ApplyDiracDeltasTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenNoDiracDeltaIsPresent()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");

        NRedberry.Tensors.Tensor actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);

        Assert.Same(tensor, actual);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("ApplyDiracDeltas", ApplyDiracDeltasTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
