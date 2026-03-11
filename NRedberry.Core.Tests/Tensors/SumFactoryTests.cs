using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumFactoryTests
{
    [Fact]
    public void ShouldReturnZeroForEmptyInput()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;

        Assert.Equal(Complex.Zero, factory.Create());
    }

    [Fact]
    public void ShouldReturnSingleTensorWithoutWrapping()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a");

        Assert.Same(tensor, factory.Create(tensor));
    }

    [Fact]
    public void ShouldBuildSimplifiedSumFromMultipleTerms()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;

        NRedberry.Tensors.Tensor result = factory.Create(
            TensorFactory.Parse("a"),
            TensorFactory.Parse("-a"),
            TensorFactory.Parse("b"));

        Assert.Equal("b", result.ToString(OutputFormat.Redberry));
    }
}
