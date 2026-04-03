using NRedberry.Numbers;
using NRedberry.Numbers.Parser;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class ComplexTokenTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        ComplexToken.Instance.ShouldBeSameAs(ComplexToken.Instance);
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = NumberParser<Complex>.ComplexParser;

        var exception = Should.Throw<ArgumentNullException>(() => ComplexToken.Instance.Parse(null!, parser));

        exception.ParamName.ShouldBe("expression");
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        var exception = Should.Throw<ArgumentNullException>(() => ComplexToken.Instance.Parse("I", null!));

        exception.ParamName.ShouldBe("parser");
    }

    [Fact]
    public void ShouldParseImaginaryUnitLiteral()
    {
        var result = ComplexToken.Instance.Parse("I", NumberParser<Complex>.ComplexParser);

        result.ShouldBe(Complex.ImaginaryOne);
    }

    [Fact]
    public void ShouldParseBigIntegerLiteralAsRealComplex()
    {
        var value = global::System.Numerics.BigInteger.Parse("1234567890123456789012345678901234567890");

        var result = ComplexToken.Instance.Parse(value.ToString(global::System.Globalization.CultureInfo.InvariantCulture), NumberParser<Complex>.ComplexParser);

        result.ShouldBe(new Complex(value));
    }

    [Fact]
    public void ShouldParseFloatingPointLiteralWithInvariantCulture()
    {
        var result = ComplexToken.Instance.Parse("12.5", NumberParser<Complex>.ComplexParser);

        result.ShouldBe(new Complex(12.5));
    }

    [Fact]
    public void ShouldReturnNullForUnsupportedLiteral()
    {
        var result = ComplexToken.Instance.Parse("1/2", NumberParser<Complex>.ComplexParser);

        result.ShouldBeNull();
    }
}
