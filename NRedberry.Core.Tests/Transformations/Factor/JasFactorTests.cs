using NRedberry.Transformations.Factor;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class JasFactorTests
{
    [Fact]
    public void ShouldFactorSimplePolynomial()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = JasFactor.Engine.Transform(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldLeaveSimpleTensorUntouched()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = JasFactor.Engine.Transform(tensor);

        Assert.Same(tensor, actual);
    }
}
