using NRedberry.Transformations;
using NRedberry.Transformations.Collect;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Collect;

public sealed class CollectTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenPatternDoesNotMatch()
    {
        CollectTransformation transformation = new([Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"))]);

        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a*y+b*y");
        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldUseOptionsConstructorForNoMatchPath()
    {
        CollectTransformation.CollectOptions options = new()
        {
            Simplifications = Transformation.Identity,
            ExpandSymbolic = false
        };
        CollectTransformation transformation = new([Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"))], options);

        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a*y+b*y");
        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }
}
