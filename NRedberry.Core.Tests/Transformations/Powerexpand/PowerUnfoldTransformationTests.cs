using NRedberry.Transformations.Powerexpand;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerUnfoldTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouched()
    {
        PowerUnfoldTransformation transformation = new([]);
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Power[a+b,2]");

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldExposeReadableNames()
    {
        PowerUnfoldTransformation defaultTransformation = new([]);
        PowerUnfoldTransformation variableTransformation = new([TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>()]);

        defaultTransformation.ToString(OutputFormat.Redberry).ShouldBe("PowerUnfold");
        variableTransformation.ToString(OutputFormat.Redberry).ShouldBe("PowerUnfold[x]");
    }
}
