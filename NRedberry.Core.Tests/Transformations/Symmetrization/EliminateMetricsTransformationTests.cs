using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateMetricsTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenNoMetricsArePresent()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");

        NRedberry.Tensors.Tensor actual = EliminateMetricsTransformation.Eliminate(tensor);

        Assert.Same(tensor, actual);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("EliminateMetrics", EliminateMetricsTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
