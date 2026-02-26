using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ITokenParserTests
{
    [Fact]
    public void ShouldUseHigherPriorityParserWhenMultipleParsersCanParse()
    {
        var lowPriorityToken = new ParseToken(TokenType.Product);
        var highPriorityToken = new ParseToken(TokenType.Sum);
        var lowPriorityParser = new RecordingTokenParser(100, lowPriorityToken);
        var highPriorityParser = new RecordingTokenParser(200, highPriorityToken);
        var parser = new RedberryParser(lowPriorityParser, highPriorityParser);

        var result = parser.Parse(" expression ");

        Assert.Same(highPriorityToken, result);
        Assert.Equal(0, lowPriorityParser.CallCount);
        Assert.Equal(1, highPriorityParser.CallCount);
    }

    [Fact]
    public void ShouldTryNextParserWhenParserReturnsNull()
    {
        var fallbackToken = new ParseToken(TokenType.Power);
        var firstParser = new RecordingTokenParser(200, null);
        var secondParser = new RecordingTokenParser(100, fallbackToken);
        var parser = new RedberryParser(firstParser, secondParser);

        var result = parser.Parse(" x ");

        Assert.Same(fallbackToken, result);
        Assert.Equal(1, firstParser.CallCount);
        Assert.Equal(1, secondParser.CallCount);
        Assert.Equal("x", firstParser.LastExpression);
        Assert.Equal("x", secondParser.LastExpression);
    }

    [Fact]
    public void ShouldPassCurrentParserInstanceToTokenParser()
    {
        var token = new ParseToken(TokenType.Product);
        var tokenParser = new RecordingTokenParser(1, token);
        var parser = new RedberryParser(tokenParser);

        parser.Parse("expr");

        Assert.Same(parser, tokenParser.LastParser);
    }
}

file sealed class RecordingTokenParser : ITokenParser
{
    private readonly ParseToken? _result;

    public RecordingTokenParser(int priority, ParseToken? result)
    {
        Priority = priority;
        _result = result;
    }

    public int Priority { get; }

    public int CallCount { get; private set; }

    public string? LastExpression { get; private set; }

    public RedberryParser? LastParser { get; private set; }

    public ParseToken? ParseToken(string expression, RedberryParser parser)
    {
        CallCount++;
        LastExpression = expression;
        LastParser = parser;
        return _result;
    }
}
