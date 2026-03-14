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

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void ShouldExposeExpectedPriority()
    {
        ParserPower.Instance.Priority.ShouldBe(9986);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionIsTooShort()
    {
        var token = ParserPower.Instance.ParseToken("Power[]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotStartWithPowerLiteral()
    {
        var token = ParserPower.Instance.ParseToken("Pow[a,b]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionDoesNotEndWithClosingBracket()
    {
        var token = ParserPower.Instance.ParseToken("Power[a,b", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenBracketsAreInconsistentInArguments()
    {
        var token = ParserPower.Instance.ParseToken("Power[a],b]", RedberryParser.Default);

        token.ShouldBeNull();
    }

    [Fact]
    public void ShouldThrowWhenPowerHasMoreThanTwoArguments()
    {
        var exception = Should.Throw<ParserException>(() => ParserPower.Instance.ParseToken("Power[a,b,c]", RedberryParser.Default));

        exception.Message.ShouldBe("Power takes only two arguments.");
    }

    [Fact]
    public void ShouldThrowWhenPowerHasOnlyOneArgument()
    {
        var exception = Should.Throw<ParserException>(() => ParserPower.Instance.ParseToken("Power[a]", RedberryParser.Default));

        exception.Message.ShouldBe("Power takes exactly two arguments.");
    }

    [Fact]
    public void ShouldParsePowerWithTwoArguments()
    {
        var token = ParserPower.Instance.ParseToken("Power[a,b]", RedberryParser.Default);

        var power = token.ShouldBeOfType<ParseToken>();
        power.TokenType.ShouldBe(TokenType.Power);
        power.Content.Length.ShouldBe(2);
        power.Content[0].TokenType.ShouldBe(TokenType.SimpleTensor);
        power.Content[1].TokenType.ShouldBe(TokenType.SimpleTensor);
    }

    [Fact]
    public void ShouldParsePowerWhenArgumentsContainNestedCommas()
    {
        var token = ParserPower.Instance.ParseToken("Power[f[a,b],g[c,d]]", RedberryParser.Default);

        var power = token.ShouldBeOfType<ParseToken>();
        power.TokenType.ShouldBe(TokenType.Power);
        power.Content.Length.ShouldBe(2);
        power.Content[0].TokenType.ShouldBe(TokenType.TensorField);
        power.Content[1].TokenType.ShouldBe(TokenType.TensorField);
    }
}
