using System.Globalization;
using System.Numerics;
using NRedberry;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class RealTokenTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        Assert.Same(RealToken.Instance, RealToken.Instance);
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => RealToken.Instance.Parse(null!, NumberParser<Real>.RealParser));

        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => RealToken.Instance.Parse("1", null!));

        Assert.Equal("parser", exception.ParamName);
    }

    [Fact]
    public void ShouldParseBigIntegerLiteralAsRational()
    {
        var value = BigInteger.Parse("1234567890123456789012345678901234567890", CultureInfo.InvariantCulture);

        var result = RealToken.Instance.Parse(value.ToString(CultureInfo.InvariantCulture), NumberParser<Real>.RealParser);

        var rational = Assert.IsType<Rational>(result);
        Assert.Equal(new Rational(value), rational);
    }

    [Fact]
    public void ShouldParseFloatingPointLiteralAsNumeric()
    {
        var result = RealToken.Instance.Parse("12.5", NumberParser<Real>.RealParser);

        var numeric = Assert.IsType<Numeric>(result);
        Assert.Equal(new Numeric(12.5), numeric);
    }

    [Fact]
    public void ShouldReturnNullForUnsupportedLiteral()
    {
        var result = RealToken.Instance.Parse("1/2", NumberParser<Real>.RealParser);

        Assert.Null(result);
    }

    [Fact]
    public void ShouldUseInvariantCultureForFloatingPointParsing()
    {
        var result = RealToken.Instance.Parse("12,5", NumberParser<Real>.RealParser);

        Assert.Null(result);
    }
}
