using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserExpressionTests
{
    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainEqualsSymbol()
    {
        var token = ParserExpression.Instance.ParseToken("a+b", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldThrowWhenExpressionContainsSeveralEqualsSymbols()
    {
        var exception = Should.Throw<ParserException>(() => ParserExpression.Instance.ParseToken("a=b=c", RedberryParser.Default));

        exception.Message.ShouldBe("Several '=' symbols.");
    }

    [Fact]
    public void ShouldParseExpressionWithoutPreprocessingMarker()
    {
        var token = ParserExpression.Instance.ParseToken("a=b+c", RedberryParser.Default);

        var expression = token.ShouldBeOfType<ParseTokenExpression>();
        expression.TokenType.ShouldBe(TokenType.Expression);
        expression.Preprocess.ShouldBeFalse();
        expression.Content.Length.ShouldBe(2);
        expression.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        expression.Content[1].TokenType.ShouldBe(TokenType.Sum);
    }

    [Fact]
    public void ShouldParseExpressionWithPreprocessingMarker()
    {
        var token = ParserExpression.Instance.ParseToken("a:=b", RedberryParser.Default);

        var expression = token.ShouldBeOfType<ParseTokenExpression>();
        expression.Preprocess.ShouldBeTrue();
        expression.Content.Length.ShouldBe(2);
        expression.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        expression.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);
    }

    [Fact]
    public void ShouldTrimLeftPartBeforeCheckingPreprocessingMarker()
    {
        var token = ParserExpression.Instance.ParseToken("  a:   =   b  ", RedberryParser.Default);

        var expression = token.ShouldBeOfType<ParseTokenExpression>();
        expression.Preprocess.ShouldBeTrue();
        expression.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        expression.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserExpression.Instance.Priority.ShouldBe(10100);
    }
}
