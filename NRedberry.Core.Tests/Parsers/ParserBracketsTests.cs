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

        parserBrackets.Priority.ShouldBe(int.MaxValue);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotStartWithOpenBracket()
    {
        var token = ParserBrackets.Instance.ParseToken("a+b", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldParseInnerExpressionWhenWrappedWithBalancedBrackets()
    {
        var token = ParserBrackets.Instance.ParseToken("(a+b)", RedberryParser.Default);

        token.ShouldNotBeNull();
        token!.TokenType.ShouldBe(TokenType.Sum);
        token.Content.Length.ShouldBe(2);
    }

    [Fact]
    public void ShouldReturnNullWhenBracketsAreBalancedButDoNotWrapWholeExpression()
    {
        var token = ParserBrackets.Instance.ParseToken("(a)+b", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenUnbalancedBracketsAndExpressionDoesNotEndWithClosingBracket()
    {
        var exception = Should.Throw<BracketsError>(() => ParserBrackets.Instance.ParseToken("(a+b", RedberryParser.Default));

        exception.Message.ShouldBe("Unbalanced brackets in (a+b");
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenExpressionEndsWithClosingBracketButHasUnmatchedOpeningBracket()
    {
        var exception = Should.Throw<BracketsError>(() => ParserBrackets.Instance.ParseToken("((a)", RedberryParser.Default));

        exception.Message.ShouldBe("Unbalanced brackets.");
    }
}
