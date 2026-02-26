using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class ComplexTokenTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        Assert.Same(ComplexToken.Instance, ComplexToken.Instance);
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = NumberParser<Complex>.ComplexParser;

        var exception = Assert.Throws<ArgumentNullException>(() => ComplexToken.Instance.Parse(null!, parser));

        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => ComplexToken.Instance.Parse("I", null!));

        Assert.Equal("parser", exception.ParamName);
    }

    [Fact]
    public void ShouldParseImaginaryUnitLiteral()
    {
        var result = ComplexToken.Instance.Parse("I", NumberParser<Complex>.ComplexParser);

        Assert.Equal(Complex.ImaginaryOne, result);
    }

    [Fact]
    public void ShouldParseBigIntegerLiteralAsRealComplex()
    {
        var value = global::System.Numerics.BigInteger.Parse("1234567890123456789012345678901234567890");

        var result = ComplexToken.Instance.Parse(value.ToString(global::System.Globalization.CultureInfo.InvariantCulture), NumberParser<Complex>.ComplexParser);

        Assert.Equal(new Complex(value), result);
    }

    [Fact]
    public void ShouldParseFloatingPointLiteralWithInvariantCulture()
    {
        var result = ComplexToken.Instance.Parse("12.5", NumberParser<Complex>.ComplexParser);

        Assert.Equal(new Complex(12.5), result);
    }

    [Fact]
    public void ShouldReturnNullForUnsupportedLiteral()
    {
        var result = ComplexToken.Instance.Parse("1/2", NumberParser<Complex>.ComplexParser);

        Assert.Null(result);
    }
}
