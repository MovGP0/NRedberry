using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class NumberParserTests
{
    [Fact]
    public void ShouldThrowWhenParsersArrayIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new NumberParser<Real>(null!));
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = new NumberParser<Real>([new AlwaysRationalToken()]);
        Should.Throw<ArgumentNullException>(() => parser.Parse(null!));
    }

    [Fact]
    public void ShouldThrowFormatExceptionWhenNoTokenCanParse()
    {
        var parser = new NumberParser<Real>([new NullToken()]);
        Should.Throw<FormatException>(() => parser.Parse("1"));
    }

    [Fact]
    public void ShouldReturnFirstSuccessfulTokenResult()
    {
        var parser = new NumberParser<Real>([new AlwaysRationalToken(), new ThrowIfUsedToken()]);
        var result = parser.Parse("anything");
        result.ShouldBe(Rational.One);
    }

    [Fact]
    public void ShouldTrimExpressionBeforeParsing()
    {
        var result = NumberParser<Real>.RealParser.Parse(" 1+0.0 ");
        result.IsNumeric().ShouldBeTrue();
    }

    [Theory]
    [InlineData("1+a")]
    [InlineData("2/5+7/(3-(2+1/(4+o-9))*5/4)")]
    [InlineData("2/5+7/(3-(2+1/(4^-9))*5/4)")]
    public void ShouldRejectInvalidRealExpressions(string expression)
    {
        Should.Throw<FormatException>(() => NumberParser<Real>.RealParser.Parse(expression));
    }

    [Fact]
    public void ShouldParseRealCompositeExpression()
    {
        var result = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)");
        result.ShouldBe(new Rational(146, 15));
    }

    [Fact]
    public void ShouldHandleInfinityNaNAndZeroCasesForRealExpressions()
    {
        var infinite = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)+1/0");
        var zero = NumberParser<Real>.RealParser.Parse("1/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");
        var nan = NumberParser<Real>.RealParser.Parse("1/(1-3/3*1.0-2.2+22/10)/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");
        infinite.IsInfinite().ShouldBeTrue();
        zero.IsZero().ShouldBeTrue();
        nan.IsNaN().ShouldBeTrue();
    }

    [Theory]
    [InlineData("1+0.0")]
    [InlineData("2*1.0")]
    [InlineData("2/1.0")]
    public void ShouldProduceNumericRealWhenExpressionContainsFloatingPoint(string expression)
    {
        var result = NumberParser<Real>.RealParser.Parse(expression);
        result.IsNumeric().ShouldBeTrue();
    }

    [Fact]
    public void ShouldParseComplexRationalExpression()
    {
        var parsed = NumberParser<Complex>.ComplexParser.Parse("4/2+I*3/2");
        var expected = new Complex(new Rational(4, 2), new Rational(3, 2));
        parsed.ShouldBe(expected);
    }

    [Theory]
    [InlineData("0/0")]
    [InlineData("1/0")]
    public void ShouldReturnComplexNaNForInvalidComplexDivision(string expression)
    {
        var result = NumberParser<Complex>.ComplexParser.Parse(expression);
        result.ShouldBe(Complex.ComplexNaN);
    }

    [Fact]
    public void ShouldParseExpressionWithLeadingMinus()
    {
        var result = NumberParser<Complex>.ComplexParser.Parse("-2+3");
        result.ShouldBe(Complex.One);
    }
}

internal sealed class NullToken : INumberTokenParser<Real>
{
    public Real Parse(string expression, NumberParser<Real> parser)
    {
        return default!;
    }
}

internal sealed class AlwaysRationalToken : INumberTokenParser<Real>
{
    public Real Parse(string expression, NumberParser<Real> parser)
    {
        return Rational.One;
    }
}

internal sealed class ThrowIfUsedToken : INumberTokenParser<Real>
{
    public Real Parse(string expression, NumberParser<Real> parser)
    {
        throw new InvalidOperationException("Token should not be invoked.");
    }
}
