using NRedberry.Transformations.Powerexpand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerUnfoldTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouched()
    {
        PowerUnfoldTransformation transformation = new([]);
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Power[a+b,2]");

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.Same(tensor, actual);
    }

    [Fact]
    public void ShouldExposeReadableNames()
    {
        PowerUnfoldTransformation defaultTransformation = new([]);
        PowerUnfoldTransformation variableTransformation = new([Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"))]);

        Assert.Equal("PowerUnfold", defaultTransformation.ToString(OutputFormat.Redberry));
        Assert.Equal("PowerUnfold[x]", variableTransformation.ToString(OutputFormat.Redberry));
    }
}
