using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class PowerTests
{
    [Fact]
    public void ShouldExposeChildrenAndEmptyIndices()
    {
        NRedberry.Tensors.Tensor argument = TensorFactory.Parse("a");
        NRedberry.Tensors.Tensor power = new Complex(3);
        Power tensor = new(argument, power);

        Assert.Equal(2, tensor.Size);
        Assert.Same(argument, tensor[0]);
        Assert.Same(power, tensor[1]);
        Assert.Same(IndicesFactory.EmptyIndices, tensor.Indices);
    }

    [Fact]
    public void ShouldThrowForInvalidIndexer()
    {
        Power tensor = new(TensorFactory.Parse("a"), new Complex(2));

        Assert.Throws<ArgumentOutOfRangeException>(() => _ = tensor[2]);
    }

    [Fact]
    public void ShouldRenderNegativeLatexPowerAsFraction()
    {
        Power tensor = new(TensorFactory.Parse("a"), new Complex(-2));

        Assert.Equal("\\frac{1}{a^2}", tensor.ToString(OutputFormat.LaTeX));
    }

    [Fact]
    public void ShouldRenderRedberryAndMathematicaForms()
    {
        Power tensor = new(TensorFactory.Parse("a"), TensorFactory.Parse("b"));

        Assert.Equal("a**b", tensor.ToString(OutputFormat.Redberry));
        Assert.Equal("a^b", tensor.ToString(OutputFormat.WolframMathematica));
    }

    [Fact]
    public void ShouldExposePowerBuilderAndFactory()
    {
        Power tensor = new(TensorFactory.Parse("a"), new Complex(2));

        Assert.IsType<PowerBuilder>(tensor.GetBuilder());
        Assert.Same(PowerFactory.Factory, tensor.GetFactory());
    }
}
