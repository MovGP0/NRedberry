using NRedberry.Numbers;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserNumberTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        Assert.Same(ParserNumber.Instance, ParserNumber.Instance);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(9999, ParserNumber.Instance.Priority);
    }

    [Fact]
    public void ShouldParseComplexNumberExpression()
    {
        var token = ParserNumber.Instance.ParseToken("4/2+I*3/2", RedberryParser.Default);

        var numberToken = Assert.IsType<ParseTokenNumber>(token);
        Assert.Equal(TokenType.Number, numberToken.TokenType);
        Assert.Equal(new Complex(new Rational(4, 2), new Rational(3, 2)), numberToken.Value);
    }

    [Theory]
    [InlineData("1+a")]
    [InlineData("")]
    public void ShouldReturnNullWhenExpressionIsNotANumber(string expression)
    {
        var token = ParserNumber.Instance.ParseToken(expression, RedberryParser.Default);

        Assert.Null(token);
    }
}
