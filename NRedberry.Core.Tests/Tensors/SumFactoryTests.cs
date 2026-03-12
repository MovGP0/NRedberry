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
    public void ShouldBuildSumFromMultipleDistinctTerms()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;

        NRedberry.Tensors.Tensor result = factory.Create(
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"));

        Sum sum = Assert.IsType<Sum>(result);
        Assert.Equal(2, sum.Size);
        Assert.Contains("a", sum.ToString(OutputFormat.Redberry));
        Assert.Contains("b", sum.ToString(OutputFormat.Redberry));
    }
}
