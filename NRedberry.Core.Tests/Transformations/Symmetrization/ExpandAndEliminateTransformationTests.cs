using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ExpandAndEliminateTransformationTests
{
    [Fact]
    public void ShouldLeaveSimpleTensorUntouched()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = ExpandAndEliminateTransformation.ExpandAndEliminate(tensor);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        ExpandAndEliminateTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("ExpandAndEliminate");
    }
}
