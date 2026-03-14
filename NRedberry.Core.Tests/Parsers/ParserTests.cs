using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullTokenParsers()
    {
        Assert.Throws<ArgumentNullException>(() => new RedberryParser(null!));
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = new RedberryParser(new ParserRecordingTokenParser(0));

        Assert.Throws<ArgumentNullException>(() => parser.Parse(null!));
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsEmptyAfterRemovingComments()
    {
        var parser = new RedberryParser(new ParserRecordingTokenParser(0));

        var exception = Assert.Throws<ArgumentException>(() => parser.Parse("/* only comment */"));

        Assert.Equal("expression", exception.ParamName);
        Assert.Contains("Empty expression.", exception.Message);
    }

    [Fact]
    public void ShouldUseHighestPriorityParserFirst()
    {
        var lowPriorityParser = new ParserRecordingTokenParser(1, (_, _) => new ParseToken(TokenType.Sum));
        var highPriorityParser = new ParserRecordingTokenParser(10, (_, _) => new ParseToken(TokenType.Product));
        var parser = new RedberryParser(lowPriorityParser, highPriorityParser);

        var result = parser.Parse("a");

        Assert.Equal(TokenType.Product, result.TokenType);
        Assert.Equal(1, highPriorityParser.CallCount);
        Assert.Equal(0, lowPriorityParser.CallCount);
    }

    [Fact]
    public void ShouldPassTrimmedExpressionToTokenParsers()
    {
        var tokenParser = new ParserRecordingTokenParser(0, (_, _) => new ParseToken(TokenType.Number));
        var parser = new RedberryParser(tokenParser);

        _ = parser.Parse("   a + b   ");

        Assert.Equal("a + b", tokenParser.LastExpression);
    }

    [Fact]
    public void ShouldRemoveCommentsAndNewLinesBeforeParsing()
    {
        var tokenParser = new ParserRecordingTokenParser(0, (_, _) => new ParseToken(TokenType.Number));
        var parser = new RedberryParser(tokenParser);

        _ = parser.Parse("a/* block */ + b // line\n");

        Assert.Equal("a + b", tokenParser.LastExpression);
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenNoParserCanHandleExpression()
    {
        var parser = new RedberryParser(new ParserRecordingTokenParser(10), new ParserRecordingTokenParser(0));

        var exception = Assert.Throws<ParserException>(() => parser.Parse("a+b"));

        Assert.Contains("No appropriate parser for expression: \"a+b\"", exception.Message);
    }
}

internal sealed class ParserRecordingTokenParser(int priority, Func<string, RedberryParser, ParseToken?>? parse = null)
    : ITokenParser
{
    private readonly Func<string, RedberryParser, ParseToken?> _parse = parse ?? ((_, _) => null);

    public int Priority { get; } = priority;

    public int CallCount { get; private set; }

    public string? LastExpression { get; private set; }

    public ParseToken? ParseToken(string expression, RedberryParser parser)
    {
        CallCount++;
        LastExpression = expression;
        return _parse(expression, parser);
    }
}
