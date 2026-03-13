using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using Xunit;
using NumberComplex = NRedberry.Numbers.Complex;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class NumberParserTest
{
    [Fact]
    public void ShouldHandleZeroDivisionInMultiplication()
    {
        Real left = NumberParser<Real>.RealParser.Parse("2/3");
        Real right = NumberParser<Real>.RealParser.Parse("2/3-2/3");

        Assert.True(right.IsZero());
        Assert.True(left.Multiply(right).IsZero());
        Assert.True(left.Divide(right).IsInfinite());
    }

    [Fact]
    public void ShouldRejectInvalidLiteral1()
    {
        Assert.Throws<FormatException>(() => NumberParser<Real>.RealParser.Parse("1+a"));
    }

    [Fact]
    public void ShouldRejectInvalidLiteral2()
    {
        Assert.Throws<FormatException>(() => NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4+o-9))*5/4)"));
    }

    [Fact]
    public void ShouldRejectInvalidLiteral3()
    {
        Assert.Throws<FormatException>(() => NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4^-9))*5/4)"));
    }

    [Fact]
    public void ShouldParseExpression1()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)");

        Assert.Equal(new Rational(146, 15), value);
    }

    [Fact]
    public void ShouldParseExpression2()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)");

        Assert.Equal(new Rational(146, 15), value);
    }

    [Fact]
    public void ShouldHandleInfinity()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)+1/0");

        Assert.True(value.IsInfinite());
    }

    [Fact]
    public void ShouldHandleZeroFromInfiniteExpression()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");

        Assert.True(value.IsZero());
    }

    [Fact]
    public void ShouldHandleZeroFromCompositeExpression()
    {
        Real value = NumberParser<Real>.RealParser.Parse("(1-3/3*1.0-2.2+22/10)/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");

        Assert.True(value.IsZero());
    }

    [Fact]
    public void ShouldHandleZeroFromNumericExpression()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1-3/3*1.0-2.2+22/10");

        Assert.True(value.IsZero());
    }

    [Fact]
    public void ShouldHandleNaN()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1/(1-3/3*1.0-2.2+22/10)/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");

        Assert.True(value.IsNaN());
    }

    [Fact]
    public void ShouldDetectNumeric1()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1+0.0");

        Assert.True(value.IsNumeric());
    }

    [Fact]
    public void ShouldDetectNumeric2()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2*1.0");

        Assert.True(value.IsNumeric());
    }

    [Fact]
    public void ShouldDetectNumeric3()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/1.0");

        Assert.True(value.IsNumeric());
    }

    [Fact]
    public void ShouldParseComplexRational()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("4/2+I*3/2");
        NumberComplex expected = new(new Rational(4, 2), new Rational(3, 2));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldParseComplexNaN1()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("0/0");

        Assert.Equal(NumberComplex.ComplexNaN, actual);
    }

    [Fact]
    public void ShouldParseComplexNaN2()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("1/0");

        Assert.Equal(NumberComplex.ComplexNaN, actual);
    }

    [Fact]
    public void ShouldParseComplexSimplified()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("-2+3");

        Assert.Equal(NumberComplex.One, actual);
    }

    [Fact]
    public void ShouldParseExpressionWithNegatives()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(-3-(-2+1/(-4-9))*5/4)");

        Assert.Equal(new Rational(-254, 15), value);
    }
}
