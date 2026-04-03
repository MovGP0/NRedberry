using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateMetricsTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenNoMetricsArePresent()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");

        NRedberry.Tensors.Tensor actual = EliminateMetricsTransformation.Eliminate(tensor);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        EliminateMetricsTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("EliminateMetrics");
    }
}
