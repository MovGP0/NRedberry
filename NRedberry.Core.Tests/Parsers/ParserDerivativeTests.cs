using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserDerivativeTests
{
    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotStartWithDerivativePrefix()
    {
        var token = ParserDerivative.Instance.ParseToken("x][f]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsMultipleArgumentSeparators()
    {
        var token = ParserDerivative.Instance.ParseToken("D[x][f][g]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenArgumentBracketsAreInconsistent()
    {
        var token = ParserDerivative.Instance.ParseToken("D[x][f[a]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenDerivativeVariableBracketsAreInconsistent()
    {
        var token = ParserDerivative.Instance.ParseToken("D[x,(y][f]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldParseDerivativeWithNestedExpressionsInVariablesList()
    {
        var token = ParserDerivative.Instance.ParseToken("D[(x+y),f[a,b]][q[m]]", RedberryParser.Default);

        var derivative = Assert.IsType<ParseTokenDerivative>(token);
        Assert.Equal(TokenType.Derivative, derivative.TokenType);
        Assert.Equal(3, derivative.Content.Length);
        Assert.Equal(TokenType.TensorField, derivative.Content[0].TokenType);
        Assert.Equal(TokenType.Sum, derivative.Content[1].TokenType);
        Assert.Equal(TokenType.SimpleTensor, derivative.Content[2].TokenType);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(8000, ParserDerivative.Instance.Priority);
    }
}
