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

        tensor.Size.ShouldBe(2);
        tensor[0].ShouldBeSameAs(argument);
        tensor[1].ShouldBeSameAs(power);
        tensor.Indices.ShouldBeSameAs(IndicesFactory.EmptyIndices);
    }

    [Fact]
    public void ShouldThrowForInvalidIndexer()
    {
        Power tensor = new(TensorFactory.Parse("a"), new Complex(2));

        Should.Throw<ArgumentOutOfRangeException>(() => _ = tensor[2]);
    }

    [Fact]
    public void ShouldRenderNegativeLatexPowerAsFraction()
    {
        Power tensor = new(TensorFactory.Parse("a"), new Complex(-2));

        tensor.ToString(OutputFormat.LaTeX).ShouldBe("\\frac{1}{a^2}");
    }

    [Fact]
    public void ShouldRenderRedberryAndMathematicaForms()
    {
        Power tensor = new(TensorFactory.Parse("a"), TensorFactory.Parse("b"));

        tensor.ToString(OutputFormat.Redberry).ShouldBe("a**b");
        tensor.ToString(OutputFormat.WolframMathematica).ShouldBe("a^b");
    }

    [Fact]
    public void ShouldExposePowerBuilderAndFactory()
    {
        Power tensor = new(TensorFactory.Parse("a"), new Complex(2));

        tensor.GetBuilder().ShouldBeOfType<PowerBuilder>();
        tensor.GetFactory().ShouldBeSameAs(PowerFactory.Factory);
    }
}
