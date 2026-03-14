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
        RealToken.Instance.ShouldBeSameAs(RealToken.Instance);
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var exception = Should.Throw<ArgumentNullException>(() => RealToken.Instance.Parse(null!, NumberParser<Real>.RealParser));

        exception.ParamName.ShouldBe("expression");
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        var exception = Should.Throw<ArgumentNullException>(() => RealToken.Instance.Parse("1", null!));

        exception.ParamName.ShouldBe("parser");
    }

    [Fact]
    public void ShouldParseBigIntegerLiteralAsRational()
    {
        var value = BigInteger.Parse("1234567890123456789012345678901234567890", CultureInfo.InvariantCulture);

        var result = RealToken.Instance.Parse(value.ToString(CultureInfo.InvariantCulture), NumberParser<Real>.RealParser);

        var rational = result.ShouldBeOfType<Rational>();
        rational.ShouldBe(new Rational(value));
    }

    [Fact]
    public void ShouldParseFloatingPointLiteralAsNumeric()
    {
        var result = RealToken.Instance.Parse("12.5", NumberParser<Real>.RealParser);

        var numeric = result.ShouldBeOfType<Numeric>();
        numeric.ShouldBe(new Numeric(12.5));
    }

    [Fact]
    public void ShouldReturnNullForUnsupportedLiteral()
    {
        var result = RealToken.Instance.Parse("1/2", NumberParser<Real>.RealParser);

        result.ShouldBeNull();
    }

    [Fact]
    public void ShouldUseInvariantCultureForFloatingPointParsing()
    {
        var result = RealToken.Instance.Parse("12,5", NumberParser<Real>.RealParser);

        result.ShouldBeNull();
    }
}
