using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ProductFactoryTests
{
    [Fact]
    public void ShouldReturnOneForEmptyInput()
    {
        NRedberry.Tensors.TensorFactory factory = ProductFactory.Factory;

        Assert.Equal(Complex.One, factory.Create());
    }

    [Fact]
    public void ShouldReturnSingleTensorWithoutWrapping()
    {
        NRedberry.Tensors.TensorFactory factory = ProductFactory.Factory;
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("T_i");

        Assert.Same(tensor, factory.Create(tensor));
    }

    [Fact]
    public void ShouldReturnSingleTensorForConcreteOverload()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("T_i");

        Assert.Same(tensor, ProductFactory.Factory.Create(tensor));
    }

    [Fact]
    public void ShouldFlattenNestedProductsAndCombineNumericFactors()
    {
        NRedberry.Tensors.TensorFactory factory = ProductFactory.Factory;

        NRedberry.Tensors.Tensor result = factory.Create(
            TensorFactory.Parse("2*a"),
            TensorFactory.Parse("3*T_i"));

        Assert.Equal("6*a*T_{i}", result.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldNegateSingleSumWhenFactorIsMinusOne()
    {
        NRedberry.Tensors.TensorFactory factory = ProductFactory.Factory;

        NRedberry.Tensors.Tensor result = factory.Create(new Complex(-1), TensorFactory.Parse("a+b"));

        string text = result.ToString(OutputFormat.Redberry);

        Assert.True(text is "-a-b" or "-b-a");
    }
}
