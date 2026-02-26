using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserBracketsTests
{
    [Fact]
    public void ShouldUseMaximumPriority()
    {
        var parserBrackets = ParserBrackets.Instance;

        Assert.Equal(int.MaxValue, parserBrackets.Priority);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotStartWithOpenBracket()
    {
        var token = ParserBrackets.Instance.ParseToken("a+b", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldParseInnerExpressionWhenWrappedWithBalancedBrackets()
    {
        var token = ParserBrackets.Instance.ParseToken("(a+b)", RedberryParser.Default);

        Assert.NotNull(token);
        Assert.Equal(TokenType.Sum, token!.TokenType);
        Assert.Equal(2, token.Content.Length);
    }

    [Fact]
    public void ShouldReturnNullWhenBracketsAreBalancedButDoNotWrapWholeExpression()
    {
        var token = ParserBrackets.Instance.ParseToken("(a)+b", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenUnbalancedBracketsAndExpressionDoesNotEndWithClosingBracket()
    {
        var exception = Assert.Throws<BracketsError>(() => ParserBrackets.Instance.ParseToken("(a+b", RedberryParser.Default));

        Assert.Equal("Unbalanced brackets in (a+b", exception.Message);
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenExpressionEndsWithClosingBracketButHasUnmatchedOpeningBracket()
    {
        var exception = Assert.Throws<BracketsError>(() => ParserBrackets.Instance.ParseToken("((a)", RedberryParser.Default));

        Assert.Equal("Unbalanced brackets.", exception.Message);
    }
}
