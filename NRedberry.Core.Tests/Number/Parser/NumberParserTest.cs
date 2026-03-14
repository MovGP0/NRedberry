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

        right.IsZero().ShouldBeTrue();
        left.Multiply(right).IsZero().ShouldBeTrue();
        left.Divide(right).IsInfinite().ShouldBeTrue();
    }

    [Fact]
    public void ShouldRejectInvalidLiteral1()
    {
        Should.Throw<FormatException>(() => NumberParser<Real>.RealParser.Parse("1+a"));
    }

    [Fact]
    public void ShouldRejectInvalidLiteral2()
    {
        Should.Throw<FormatException>(() => NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4+o-9))*5/4)"));
    }

    [Fact]
    public void ShouldRejectInvalidLiteral3()
    {
        Should.Throw<FormatException>(() => NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4^-9))*5/4)"));
    }

    [Fact]
    public void ShouldParseExpression1()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)");

        value.ShouldBe(new Rational(146, 15));
    }

    [Fact]
    public void ShouldParseExpression2()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)");

        value.ShouldBe(new Rational(146, 15));
    }

    [Fact]
    public void ShouldHandleInfinity()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)+1/0");

        value.IsInfinite().ShouldBeTrue();
    }

    [Fact]
    public void ShouldHandleZeroFromInfiniteExpression()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");

        value.IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldHandleZeroFromCompositeExpression()
    {
        Real value = NumberParser<Real>.RealParser.Parse("(1-3/3*1.0-2.2+22/10)/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");

        value.IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldHandleZeroFromNumericExpression()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1-3/3*1.0-2.2+22/10");

        value.IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldHandleNaN()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1/(1-3/3*1.0-2.2+22/10)/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");

        value.IsNaN().ShouldBeTrue();
    }

    [Fact]
    public void ShouldDetectNumeric1()
    {
        Real value = NumberParser<Real>.RealParser.Parse("1+0.0");

        value.IsNumeric().ShouldBeTrue();
    }

    [Fact]
    public void ShouldDetectNumeric2()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2*1.0");

        value.IsNumeric().ShouldBeTrue();
    }

    [Fact]
    public void ShouldDetectNumeric3()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/1.0");

        value.IsNumeric().ShouldBeTrue();
    }

    [Fact]
    public void ShouldParseComplexRational()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("4/2+I*3/2");
        NumberComplex expected = new(new Rational(4, 2), new Rational(3, 2));

        actual.ShouldBe(expected);
    }

    [Fact]
    public void ShouldParseComplexNaN1()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("0/0");

        actual.ShouldBe(NumberComplex.ComplexNaN);
    }

    [Fact]
    public void ShouldParseComplexNaN2()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("1/0");

        actual.ShouldBe(NumberComplex.ComplexNaN);
    }

    [Fact]
    public void ShouldParseComplexSimplified()
    {
        NumberComplex actual = NumberParser<NumberComplex>.ComplexParser.Parse("-2+3");

        actual.ShouldBe(NumberComplex.One);
    }

    [Fact]
    public void ShouldParseExpressionWithNegatives()
    {
        Real value = NumberParser<Real>.RealParser.Parse("2/5+7/(-3-(-2+1/(-4-9))*5/4)");

        value.ShouldBe(new Rational(-254, 15));
    }
}
