using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class PowerFactoryTest
{
    [Fact]
    public void ShouldSimplifyRationalPowerValues()
    {
        Assert.Equal("1/4", TensorFactory.Parse("Power[1/2,2]").ToString());
        Assert.Equal("1/3", TensorFactory.Parse("Power[1/9,1/2]").ToString());
        Assert.Equal("3", TensorFactory.Parse("Power[1/9,-1/2]").ToString());
        Assert.Equal("27", TensorFactory.Parse("Power[1/9,-3/2]").ToString());
    }

    [Fact]
    public void ShouldPreserveRationalPowerStringForm()
    {
        Assert.Equal("(1/2)**(1/2)", TensorFactory.Parse("Power[1/2,1/2]").ToString(OutputFormat.Redberry));
        Assert.Equal("(1/2)**(1/3)", TensorFactory.Parse("Power[1/2,1/3]").ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldSimplifyImaginaryUnitRoots()
    {
        NRedberry.Tensors.Tensor positiveHalf = PowerFactory.Power(Complex.MinusOne, Complex.OneHalf);
        NRedberry.Tensors.Tensor negativeHalf = PowerFactory.Power(Complex.MinusOne, Complex.MinusOneHalf);

        Assert.Equal(Complex.ImaginaryOne, positiveHalf);
        Assert.Equal(Complex.ImaginaryOne.Negate(), negativeHalf);
    }

    [Fact]
    public void ShouldPreserveNegativeSymbolicRoot()
    {
        NRedberry.Tensors.Tensor actual = TensorFactory.Parse("(-a)**(1/2)");

        Assert.Equal("(-a)**(1/2)", actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldDistributeIntegerPowersAcrossScalarProductFactors()
    {
        NRedberry.Tensors.Tensor actual = PowerFactory.Power(TensorFactory.Parse("2*a"), new Complex(3));

        Assert.Equal("8*a**3", actual.ToString(OutputFormat.Redberry));
    }
}
