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

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsMultipleArgumentSeparators()
    {
        var token = ParserDerivative.Instance.ParseToken("D[x][f][g]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenArgumentBracketsAreInconsistent()
    {
        var token = ParserDerivative.Instance.ParseToken("D[x][f[a]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenDerivativeVariableBracketsAreInconsistent()
    {
        var token = ParserDerivative.Instance.ParseToken("D[x,(y][f]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldParseDerivativeWithNestedExpressionsInVariablesList()
    {
        var token = ParserDerivative.Instance.ParseToken("D[(x+y),f[a,b]][q[m]]", RedberryParser.Default);

        var derivative = token.ShouldBeOfType<ParseTokenDerivative>();
        derivative.TokenType.ShouldBe(TokenType.Derivative);
        derivative.Content.Length.ShouldBe(3);
        derivative.Content[0].TokenType.ShouldBe(TokenType.TensorField);
        derivative.Content[1].TokenType.ShouldBe(TokenType.Sum);
        derivative.Content[2].TokenType.ShouldBe(TokenType.SimpleTensor);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserDerivative.Instance.Priority.ShouldBe(8000);
    }
}
