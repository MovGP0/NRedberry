using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserPowerTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        var first = ParserPower.Instance;
        var second = ParserPower.Instance;

        Assert.Same(first, second);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        Assert.Equal(9986, ParserPower.Instance.Priority);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionIsTooShort()
    {
        var token = ParserPower.Instance.ParseToken("Power[]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotStartWithPowerLiteral()
    {
        var token = ParserPower.Instance.ParseToken("Pow[a,b]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotEndWithClosingBracket()
    {
        var token = ParserPower.Instance.ParseToken("Power[a,b", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldReturnNullWhenBracketsAreInconsistentInArguments()
    {
        var token = ParserPower.Instance.ParseToken("Power[a],b]", RedberryParser.Default);

        Assert.Null(token);
    }

    [Fact]
    public void ShouldThrowWhenPowerHasMoreThanTwoArguments()
    {
        var exception = Assert.Throws<ParserException>(() => ParserPower.Instance.ParseToken("Power[a,b,c]", RedberryParser.Default));

        Assert.Equal("Power takes only two arguments.", exception.Message);
    }

    [Fact]
    public void ShouldThrowWhenPowerHasOnlyOneArgument()
    {
        var exception = Assert.Throws<ParserException>(() => ParserPower.Instance.ParseToken("Power[a]", RedberryParser.Default));

        Assert.Equal("Power takes exactly two arguments.", exception.Message);
    }

    [Fact]
    public void ShouldParsePowerWithTwoArguments()
    {
        var token = ParserPower.Instance.ParseToken("Power[a,b]", RedberryParser.Default);

        var power = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Power, power.TokenType);
        Assert.Equal(2, power.Content.Length);
        Assert.Equal(TokenType.SimpleTensor, power.Content[0].TokenType);
        Assert.Equal(TokenType.SimpleTensor, power.Content[1].TokenType);
    }

    [Fact]
    public void ShouldParsePowerWhenArgumentsContainNestedCommas()
    {
        var token = ParserPower.Instance.ParseToken("Power[f[a,b],g[c,d]]", RedberryParser.Default);

        var power = Assert.IsType<ParseToken>(token);
        Assert.Equal(TokenType.Power, power.TokenType);
        Assert.Equal(2, power.Content.Length);
        Assert.Equal(TokenType.TensorField, power.Content[0].TokenType);
        Assert.Equal(TokenType.TensorField, power.Content[1].TokenType);
    }
}
