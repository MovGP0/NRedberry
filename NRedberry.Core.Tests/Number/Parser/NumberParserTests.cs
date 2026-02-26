using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class NumberParserTests
{
    [Fact]
    public void ShouldThrowWhenParsersArrayIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new NumberParser<Real>(null!));
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = new NumberParser<Real>([new AlwaysRationalToken()]);
        Assert.Throws<ArgumentNullException>(() => parser.Parse(null!));
    }

    [Fact]
    public void ShouldThrowFormatExceptionWhenNoTokenCanParse()
    {
        var parser = new NumberParser<Real>([new NullToken()]);
        Assert.Throws<FormatException>(() => parser.Parse("1"));
    }

    [Fact]
    public void ShouldReturnFirstSuccessfulTokenResult()
    {
        var parser = new NumberParser<Real>([new AlwaysRationalToken(), new ThrowIfUsedToken()]);
        var result = parser.Parse("anything");
        Assert.Equal(Rational.One, result);
    }

    [Fact]
    public void ShouldTrimExpressionBeforeParsing()
    {
        var result = NumberParser<Real>.RealParser.Parse(" 1+0.0 ");
        Assert.True(result.IsNumeric());
    }

    [Theory]
    [InlineData("1+a")]
    [InlineData("2/5+7/(3-(2+1/(4+o-9))*5/4)")]
    [InlineData("2/5+7/(3-(2+1/(4^-9))*5/4)")]
    public void ShouldRejectInvalidRealExpressions(string expression)
    {
        Assert.Throws<FormatException>(() => NumberParser<Real>.RealParser.Parse(expression));
    }

    [Fact]
    public void ShouldParseRealCompositeExpression()
    {
        var result = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)");
        Assert.Equal(new Rational(146, 15), result);
    }

    [Fact]
    public void ShouldHandleInfinityNaNAndZeroCasesForRealExpressions()
    {
        var infinite = NumberParser<Real>.RealParser.Parse("2/5+7/(3-(2+1/(4-9))*5/4)+1/0");
        var zero = NumberParser<Real>.RealParser.Parse("1/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");
        var nan = NumberParser<Real>.RealParser.Parse("1/(1-3/3*1.0-2.2+22/10)/(2/5+7/(3-(2+1/(4-9))*5/4)+1/0)");
        Assert.True(infinite.IsInfinite());
        Assert.True(zero.IsZero());
        Assert.True(nan.IsNaN());
    }

    [Theory]
    [InlineData("1+0.0")]
    [InlineData("2*1.0")]
    [InlineData("2/1.0")]
    public void ShouldProduceNumericRealWhenExpressionContainsFloatingPoint(string expression)
    {
        var result = NumberParser<Real>.RealParser.Parse(expression);
        Assert.True(result.IsNumeric());
    }

    [Fact]
    public void ShouldParseComplexRationalExpression()
    {
        var parsed = NumberParser<Complex>.ComplexParser.Parse("4/2+I*3/2");
        var expected = new Complex(new Rational(4, 2), new Rational(3, 2));
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData("0/0")]
    [InlineData("1/0")]
    public void ShouldReturnComplexNaNForInvalidComplexDivision(string expression)
    {
        var result = NumberParser<Complex>.ComplexParser.Parse(expression);
        Assert.Equal(Complex.ComplexNaN, result);
    }

    [Fact]
    public void ShouldParseExpressionWithLeadingMinus()
    {
        var result = NumberParser<Complex>.ComplexParser.Parse("-2+3");
        Assert.Equal(Complex.One, result);
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
