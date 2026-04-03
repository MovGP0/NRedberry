using NRedberry.Numbers;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserSumTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        ParserSum.Instance.ShouldBeSameAs(ParserSum.Instance);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserSum.Instance.Priority.ShouldBe(1000);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotContainTopLevelPlusOrMinus()
    {
        var token = ParserSum.Instance.ParseToken("a*b", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldParsePlusAndMinusAtTopLevel()
    {
        var token = ParserSum.Instance.ParseToken("a+b-c", RedberryParser.Default);

        var sum = token.ShouldBeOfType<ParseToken>();
        sum.TokenType.ShouldBe(TokenType.Sum);
        sum.Content.Length.ShouldBe(3);
        sum.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        sum.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);

        var negativeTerm = sum.Content[2].ShouldBeOfType<ParseToken>();
        negativeTerm.TokenType.ShouldBe(TokenType.Product);
        negativeTerm.Content.Length.ShouldBe(2);

        var minusOne = negativeTerm.Content[0].ShouldBeOfType<ParseTokenNumber>();
        minusOne.Value.ShouldBe(Complex.MinusOne);
        negativeTerm.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);
    }

    [Fact]
    public void ShouldPrependMinusOneWhenSubtractingAProduct()
    {
        var token = ParserSum.Instance.ParseToken("a-b*c", RedberryParser.Default);

        var sum = token.ShouldBeOfType<ParseToken>();
        sum.TokenType.ShouldBe(TokenType.Sum);
        sum.Content.Length.ShouldBe(2);

        var negativeProduct = sum.Content[1].ShouldBeOfType<ParseToken>();
        negativeProduct.TokenType.ShouldBe(TokenType.Product);
        negativeProduct.Content.Length.ShouldBe(3);

        var minusOne = negativeProduct.Content[0].ShouldBeOfType<ParseTokenNumber>();
        minusOne.Value.ShouldBe(Complex.MinusOne);
        negativeProduct.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);
        negativeProduct.Content[2].TokenType.ShouldBe(TokenType.SimpleTensor);
    }

    [Fact]
    public void ShouldNotSplitTermsInsideBrackets()
    {
        var token = ParserSum.Instance.ParseToken("(a+b)-c", RedberryParser.Default);

        var sum = token.ShouldBeOfType<ParseToken>();
        sum.TokenType.ShouldBe(TokenType.Sum);
        sum.Content.Length.ShouldBe(2);
        sum.Content[0].TokenType.ShouldBe(TokenType.Sum);

        var negativeTerm = sum.Content[1].ShouldBeOfType<ParseToken>();
        negativeTerm.TokenType.ShouldBe(TokenType.Product);
    }

    [Fact]
    public void ShouldNormalizeDoubleMinusBeforeParsing()
    {
        var token = ParserSum.Instance.ParseToken("a--b", RedberryParser.Default);

        var sum = token.ShouldBeOfType<ParseToken>();
        sum.TokenType.ShouldBe(TokenType.Sum);
        sum.Content.Length.ShouldBe(2);
        sum.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        sum.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);
    }
}
