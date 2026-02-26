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

        Assert.Null(token);
    }

    [Fact]
    public void ShouldThrowWhenExpressionContainsSeveralEqualsSymbols()
    {
        var exception = Assert.Throws<ParserException>(() => ParserExpression.Instance.ParseToken("a=b=c", RedberryParser.Default));

        Assert.Equal("Several '=' symbols.", exception.Message);
    }

    [Fact]
    public void ShouldParseExpressionWithoutPreprocessingMarker()
    {
        var token = ParserExpression.Instance.ParseToken("a=b+c", RedberryParser.Default);

        var expression = Assert.IsType<ParseTokenExpression>(token);
        Assert.Equal(TokenType.Expression, expression.TokenType);
        Assert.False(expression.Preprocess);
        Assert.Equal(2, expression.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, expression.Content[0].TokenType);
        Assert.Equal(TokenType.Sum, expression.Content[1].TokenType);
    }

    [Fact]
    public void ShouldParseExpressionWithPreprocessingMarker()
    {
        var token = ParserExpression.Instance.ParseToken("a:=b", RedberryParser.Default);

        var expression = Assert.IsType<ParseTokenExpression>(token);
        Assert.True(expression.Preprocess);
        Assert.Equal(2, expression.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, expression.Content[0].TokenType);
        Assert.Equal(TokenType.SimpleTensor, expression.Content[1].TokenType);
    }

    [Fact]
    public void ShouldTrimLeftPartBeforeCheckingPreprocessingMarker()
    {
        var token = ParserExpression.Instance.ParseToken("  a:   =   b  ", RedberryParser.Default);

        var expression = Assert.IsType<ParseTokenExpression>(token);
        Assert.True(expression.Preprocess);
        Assert.Equal(TokenType.SimpleTensor, expression.Content[0].TokenType);
        Assert.Equal(TokenType.SimpleTensor, expression.Content[1].TokenType);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(10100, ParserExpression.Instance.Priority);
    }
}
