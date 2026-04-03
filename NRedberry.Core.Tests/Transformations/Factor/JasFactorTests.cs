using NRedberry.Transformations.Factor;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class JasFactorTests
{
    [Fact]
    public void ShouldFactorSimplePolynomial()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = JasFactor.Engine.Transform(tensor);

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldLeaveSimpleTensorUntouched()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = JasFactor.Engine.Transform(tensor);

        actual.ShouldBeSameAs(tensor);
    }
}
