using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using Xunit;
using SystemBigInteger = System.Numerics.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class PowerTests
{
    [Fact]
    public void ShouldComputePositivePowerAndInstancePowerMethods()
    {
        Power<BigRational> power = new(new BigRational());

        Assert.Equal("8", Power<BigRational>.PositivePower(new BigRational(2), 3).ToString());
        Assert.Equal("16", Power<BigRational>.PositivePower(new BigRational(2), new SystemBigInteger(4)).ToString());
        Assert.Equal("1/4", power.PowerMethod(new BigRational(2), -2).ToString());
    }

    [Fact]
    public void ShouldComputeModularPowerProductsSumsAndLogarithms()
    {
        BigRational factory = new();
        BigInteger integerFactory = new();
        List<BigRational> values =
        [
            new BigRational(2),
            new BigRational(3),
            new BigRational(4)
        ];

        Assert.Equal("2", Power<BigInteger>.ModPower(integerFactory, new BigInteger(2), new SystemBigInteger(10), new BigInteger(7)).ToString());
        Assert.Equal("24", Power<BigRational>.Multiply(factory, values).ToString());
        Assert.Equal("9", Power<BigRational>.Sum(factory, values).ToString());
        Assert.Equal(4L, Power<BigRational>.Logarithm(new BigRational(2), new BigRational(9)));
    }
}
