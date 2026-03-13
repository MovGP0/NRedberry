using System.Numerics;
using NRedberry.Numbers.Parser;
using NumberComplex = NRedberry.Numbers.Complex;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexTest
{
    [Fact]
    public void ShouldComputeAbsFromRational()
    {
        NumberComplex value = new(new Rational(4), new Rational(3));

        Assert.Equal(5, value.Abs().IntValue());
    }

    [Fact]
    public void ShouldComputeAbsFromParsed()
    {
        NumberComplex value = NumberParser<NumberComplex>.ComplexParser.Parse("2+I*3/2");

        Assert.Equal(2.5, value.Abs().DoubleValue(), 12);
    }

    [Fact]
    public void ShouldExponentiateWithIntegerExponent()
    {
        NumberComplex first = NumberParser<NumberComplex>.ComplexParser.Parse("1+I");
        NumberComplex second = NumberParser<NumberComplex>.ComplexParser.Parse("5+I");

        Assert.Equal(NumberParser<NumberComplex>.ComplexParser.Parse("I*32"), first.Pow(10));
        Assert.Equal(
            NumberParser<NumberComplex>.ComplexParser.Parse("35285997703156887662757093411637173142881213037477773358335032217829376+43564079327764355710590239114714227097865139047852601182929616371712000*I"),
            second.Pow(100));
    }

    [Fact]
    public void ShouldExponentiateWithNumericComplexExponent()
    {
        NumberComplex value = NumberParser<NumberComplex>.ComplexParser.Parse("5+I");
        NumberComplex actual = value.PowNumeric(value);
        NumberComplex expected = NumberParser<NumberComplex>.ComplexParser.Parse(
            "-2447.6068984138622390537015124004469474415099289143+1419.5557138599609517808549217505859917260231093976*I");

        Assert.True(actual.Subtract(expected).AbsNumeric() <= 1E-10);
    }

    [Fact]
    public void ShouldHandleNegativeIntegerExponent()
    {
        NumberComplex value = NumberParser<NumberComplex>.ComplexParser.Parse("5+I");
        NumberComplex expected = NumberParser<NumberComplex>.ComplexParser.Parse("5/26-I/26");

        Assert.Equal(expected, value.Pow(-1));
    }

    [Fact]
    public void ShouldExponentiateWithNumericDoubleExponent()
    {
        NumberComplex value = NumberParser<NumberComplex>.ComplexParser.Parse("1+I");
        NumberComplex expected = NumberParser<NumberComplex>.ComplexParser.Parse("0.581657+1.61562*I");

        Assert.True(value.Pow(1.56).Subtract(expected).AbsNumeric() <= 1E-5);
    }

    [Fact]
    public void ShouldThrowOnIntegerOverflowLarge()
    {
        Real value = NumberParser<Real>.RealParser.Parse("999999999999999999999999999999999999999999999999999999999999999999999999999");

        Assert.Throws<OverflowException>(() => value.IntValue());
    }

    [Fact]
    public void ShouldThrowOnIntegerOverflowEdge()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2147483648");

        Assert.Throws<OverflowException>(() => value.IntValue());
    }

    [Fact]
    public void ShouldAllowMaxInt()
    {
        Real value = NumberParser<Real>.RealParser.Parse(int.MaxValue.ToString());

        Assert.Equal(int.MaxValue, value.IntValue());
    }

    [Fact]
    public void ShouldThrowOnLongOverflowLarge()
    {
        Real value = NumberParser<Real>.RealParser.Parse("999999999999999999999999999999999999999999999999999999999999999999999999999");

        Assert.Throws<OverflowException>(() => value.LongValue());
    }

    [Fact]
    public void ShouldThrowOnLongOverflowEdge()
    {
        Real value = NumberParser<Real>.RealParser.Parse("9223372036854775808");

        Assert.Throws<OverflowException>(() => value.LongValue());
    }

    [Fact]
    public void ShouldAllowMaxLong()
    {
        Real value = NumberParser<Real>.RealParser.Parse(long.MaxValue.ToString());

        Assert.Equal(long.MaxValue, value.LongValue());
    }

    [Fact]
    public void ShouldThrowOnDoubleOverflowLarge()
    {
        string value = new string('9', 310) + "/1";
        Real parsed = NumberParser<Real>.RealParser.Parse(value);

        Assert.Throws<OverflowException>(() => parsed.DoubleValue());
    }

    [Fact]
    public void ShouldAllowLargeDouble()
    {
        const string value = "1e100";
        Real parsed = NumberParser<Real>.RealParser.Parse(value);

        Assert.True(parsed.DoubleValue() > 0.0);
    }

    [Fact]
    public void ShouldComputeHashWithSign()
    {
        NumberComplex rational = new(123, 23);
        NumberComplex numeric = new(123.0, 23.0);
        NumberComplex large = new(BigInteger.Parse("921312312321312312"), BigInteger.Parse("9213123123213123122"));

        Assert.NotEqual(rational.HashWithSign(), rational.Negate().HashWithSign());
        Assert.NotEqual(numeric.HashWithSign(), numeric.Negate().HashWithSign());
        Assert.NotEqual(large.HashWithSign(), large.Negate().HashWithSign());
    }

    [Fact]
    public void ShouldComputeStaticHashCodes()
    {
        Assert.Equal(NumberComplex.Zero.GetHashCode(), NumberComplex.Zero.GetHashCode());
        Assert.NotEqual(NumberComplex.Zero.GetHashCode(), NumberComplex.One.GetHashCode());
        Assert.NotEqual(NumberComplex.One.GetHashCode(), NumberComplex.MinusOne.GetHashCode());
        Assert.NotEqual(NumberComplex.MinusOne.GetHashCode(), NumberComplex.MinusOne.HashWithSign());
    }
}
