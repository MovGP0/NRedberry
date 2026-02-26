using NRedberry.Numbers;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserSumTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        Assert.Same(ParserSum.Instance, ParserSum.Instance);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(1000, ParserSum.Instance.Priority);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainTopLevelPlusOrMinus()
    {
        var token = ParserSum.Instance.ParseToken("a*b", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldParsePlusAndMinusAtTopLevel()
    {
        var token = ParserSum.Instance.ParseToken("a+b-c", RedberryParser.Default);

        var sum = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Sum, sum.TokenType);
        Assert.Equal(3, sum.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, sum.Content[0].TokenType);
        Assert.Equal(TokenType.SimpleTensor, sum.Content[1].TokenType);

        var negativeTerm = Assert.IsType<ParseToken>(sum.Content[2]);
        Assert.Equal(TokenType.Product, negativeTerm.TokenType);
        Assert.Equal(2, negativeTerm.Content.Length);

        var minusOne = Assert.IsType<ParseTokenNumber>(negativeTerm.Content[0]);
        Assert.Equal(Complex.MinusOne, minusOne.Value);
        Assert.Equal(TokenType.SimpleTensor, negativeTerm.Content[1].TokenType);
    }

    [Fact]
    public void ShouldPrependMinusOneWhenSubtractingAProduct()
    {
        var token = ParserSum.Instance.ParseToken("a-b*c", RedberryParser.Default);

        var sum = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Sum, sum.TokenType);
        Assert.Equal(2, sum.Content.Length);

        var negativeProduct = Assert.IsType<ParseToken>(sum.Content[1]);
        Assert.Equal(TokenType.Product, negativeProduct.TokenType);
        Assert.Equal(3, negativeProduct.Content.Length);

        var minusOne = Assert.IsType<ParseTokenNumber>(negativeProduct.Content[0]);
        Assert.Equal(Complex.MinusOne, minusOne.Value);
        Assert.Equal(TokenType.SimpleTensor, negativeProduct.Content[1].TokenType);
        Assert.Equal(TokenType.SimpleTensor, negativeProduct.Content[2].TokenType);
    }

    [Fact]
    public void ShouldNotSplitTermsInsideBrackets()
    {
        var token = ParserSum.Instance.ParseToken("(a+b)-c", RedberryParser.Default);

        var sum = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Sum, sum.TokenType);
        Assert.Equal(2, sum.Content.Length);
        Assert.Equal(TokenType.Sum, sum.Content[0].TokenType);

        var negativeTerm = Assert.IsType<ParseToken>(sum.Content[1]);
        Assert.Equal(TokenType.Product, negativeTerm.TokenType);
    }

    [Fact]
    public void ShouldNormalizeDoubleMinusBeforeParsing()
    {
        var token = ParserSum.Instance.ParseToken("a--b", RedberryParser.Default);

        var sum = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Sum, sum.TokenType);
        Assert.Equal(2, sum.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, sum.Content[0].TokenType);
        Assert.Equal(TokenType.SimpleTensor, sum.Content[1].TokenType);
    }
}
