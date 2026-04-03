using NRedberry.Transformations;
using NRedberry.Transformations.Collect;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Collect;

public sealed class CollectTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenPatternDoesNotMatch()
    {
        CollectTransformation transformation = new([TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>()]);

        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a*y+b*y");
        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldUseOptionsConstructorForNoMatchPath()
    {
        CollectTransformation.CollectOptions options = new()
        {
            Simplifications = Transformation.Identity,
            ExpandSymbolic = false
        };
        CollectTransformation transformation = new([TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>()], options);

        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a*y+b*y");
        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }
}
