using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

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

        result.ShouldBeSameAs(highPriorityToken);
        lowPriorityParser.CallCount.ShouldBe(0);
        highPriorityParser.CallCount.ShouldBe(1);
    }

    [Fact]
    public void ShouldTryNextParserWhenParserReturnsNull()
    {
        var fallbackToken = new ParseToken(TokenType.Power);
        var firstParser = new RecordingTokenParser(200, null);
        var secondParser = new RecordingTokenParser(100, fallbackToken);
        var parser = new RedberryParser(firstParser, secondParser);

        var result = parser.Parse(" x ");

        result.ShouldBeSameAs(fallbackToken);
        firstParser.CallCount.ShouldBe(1);
        secondParser.CallCount.ShouldBe(1);
        firstParser.LastExpression.ShouldBe("x");
        secondParser.LastExpression.ShouldBe("x");
    }

    [Fact]
    public void ShouldPassCurrentParserInstanceToTokenParser()
    {
        var token = new ParseToken(TokenType.Product);
        var tokenParser = new RecordingTokenParser(1, token);
        var parser = new RedberryParser(tokenParser);

        parser.Parse("expr");

        tokenParser.LastParser.ShouldBeSameAs(parser);
    }
}

file sealed class RecordingTokenParser(int priority, ParseToken? result) : ITokenParser
{
    public int Priority { get; } = priority;

    public int CallCount { get; private set; }

    public string? LastExpression { get; private set; }

    public RedberryParser? LastParser { get; private set; }

    public ParseToken? ParseToken(string expression, RedberryParser parser)
    {
        CallCount++;
        LastExpression = expression;
        LastParser = parser;
        return result;
    }
}
