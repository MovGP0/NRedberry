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
        ParserNumber.Instance.ShouldBeSameAs(ParserNumber.Instance);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserNumber.Instance.Priority.ShouldBe(9999);
    }

    [Fact]
    public void ShouldParseComplexNumberExpression()
    {
        var token = ParserNumber.Instance.ParseToken("4/2+I*3/2", RedberryParser.Default);

        var numberToken = token.ShouldBeOfType<ParseTokenNumber>();
        numberToken.TokenType.ShouldBe(TokenType.Number);
        numberToken.Value.ShouldBe(new Complex(new Rational(4, 2), new Rational(3, 2)));
    }

    [Theory]
    [InlineData("1+a")]
    [InlineData("")]
    public void ShouldReturnNullWhenExpressionIsNotANumber(string expression)
    {
        var token = ParserNumber.Instance.ParseToken(expression, RedberryParser.Default);

        token.ShouldBeNull();
    }
}
