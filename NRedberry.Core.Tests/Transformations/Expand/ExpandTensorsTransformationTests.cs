using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandTensorsTransformationTests
{
    [Fact]
    public void ShouldLeaveScalarSumsUntouchedByDefault()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a+b)*c");

        NRedberry.Tensors.Tensor actual = ExpandTensorsTransformation.Expand(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldAllowLeaveScalarsConstructorWithoutChangingScalarOnlyProduct()
    {
        ExpandTensorsTransformation transformation = new(true);
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a+b)*c");

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("ExpandTensors", ExpandTensorsTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
