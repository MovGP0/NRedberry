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
        TensorFactory.Parse("Power[1/2,2]").ToString().ShouldBe("1/4");
        TensorFactory.Parse("Power[1/9,1/2]").ToString().ShouldBe("1/3");
        TensorFactory.Parse("Power[1/9,-1/2]").ToString().ShouldBe("3");
        TensorFactory.Parse("Power[1/9,-3/2]").ToString().ShouldBe("27");
    }

    [Fact]
    public void ShouldPreserveRationalPowerStringForm()
    {
        TensorFactory.Parse("Power[1/2,1/2]").ToString(OutputFormat.Redberry).ShouldBe("(1/2)**(1/2)");
        TensorFactory.Parse("Power[1/2,1/3]").ToString(OutputFormat.Redberry).ShouldBe("(1/2)**(1/3)");
    }

    [Fact]
    public void ShouldSimplifyImaginaryUnitRoots()
    {
        NRedberry.Tensors.Tensor positiveHalf = PowerFactory.Power(Complex.MinusOne, Complex.OneHalf);
        NRedberry.Tensors.Tensor negativeHalf = PowerFactory.Power(Complex.MinusOne, Complex.MinusOneHalf);

        positiveHalf.ShouldBe(Complex.ImaginaryOne);
        negativeHalf.ShouldBe(Complex.ImaginaryOne.Negate());
    }

    [Fact]
    public void ShouldPreserveNegativeSymbolicRoot()
    {
        NRedberry.Tensors.Tensor actual = TensorFactory.Parse("(-a)**(1/2)");

        actual.ToString(OutputFormat.Redberry).ShouldBe("(-a)**(1/2)");
    }

    [Fact]
    public void ShouldCreateExpandedProductForIntegerPowerOfScalarFactor()
    {
        Product actual = PowerFactory.Power(TensorFactory.Parse("2*a"), new Complex(3)).ShouldBeOfType<Product>();

        actual.Size.ShouldBe(2);
        actual[0].ShouldBe(new Complex(8));

        Power power = actual[1].ShouldBeOfType<Power>();
        power[0].ShouldBeOfType<SimpleTensor>();
        power[1].ShouldBe(new Complex(3));
    }

    [Fact]
    public void ShouldRejectConcreteCreateOverloadWithSingleArgument()
    {
        Should.Throw<ArgumentException>(() => PowerFactory.Factory.Create(TensorFactory.Parse("a")));
    }
}
