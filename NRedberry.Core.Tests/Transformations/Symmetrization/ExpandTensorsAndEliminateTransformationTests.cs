using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ExpandTensorsAndEliminateTransformationTests
{
    [Fact]
    public void ShouldLeaveSimpleTensorUntouched()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = ExpandTensorsAndEliminateTransformation.ExpandTensorsAndEliminate(tensor);

        Assert.Same(tensor, actual);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal(
            "ExpandTensorsAndEliminate",
            ExpandTensorsAndEliminateTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
