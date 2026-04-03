using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullTokenParsers()
    {
        Should.Throw<ArgumentNullException>(() => new RedberryParser(null!));
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = new RedberryParser(new ParserRecordingTokenParser(0));

        Should.Throw<ArgumentNullException>(() => parser.Parse(null!));
    }

    [Fact]
    public void ShouldThrowWhenExpressionIsEmptyAfterRemovingComments()
    {
        var parser = new RedberryParser(new ParserRecordingTokenParser(0));

        var exception = Should.Throw<ArgumentException>(() => parser.Parse("/* only comment */"));

        exception.ParamName.ShouldBe("expression");
        exception.Message.ShouldContain("Empty expression.");
    }

    [Fact]
    public void ShouldUseHighestPriorityParserFirst()
    {
        var lowPriorityParser = new ParserRecordingTokenParser(1, (_, _) => new ParseToken(TokenType.Sum));
        var highPriorityParser = new ParserRecordingTokenParser(10, (_, _) => new ParseToken(TokenType.Product));
        var parser = new RedberryParser(lowPriorityParser, highPriorityParser);

        var result = parser.Parse("a");

        result.TokenType.ShouldBe(TokenType.Product);
        highPriorityParser.CallCount.ShouldBe(1);
        lowPriorityParser.CallCount.ShouldBe(0);
    }

    [Fact]
    public void ShouldPassTrimmedExpressionToTokenParsers()
    {
        var tokenParser = new ParserRecordingTokenParser(0, (_, _) => new ParseToken(TokenType.Number));
        var parser = new RedberryParser(tokenParser);

        _ = parser.Parse("   a + b   ");

        tokenParser.LastExpression.ShouldBe("a + b");
    }

    [Fact]
    public void ShouldRemoveCommentsAndNewLinesBeforeParsing()
    {
        var tokenParser = new ParserRecordingTokenParser(0, (_, _) => new ParseToken(TokenType.Number));
        var parser = new RedberryParser(tokenParser);

        _ = parser.Parse("a/* block */ + b // line\n");

        tokenParser.LastExpression.ShouldBe("a + b");
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenNoParserCanHandleExpression()
    {
        var parser = new RedberryParser(new ParserRecordingTokenParser(10), new ParserRecordingTokenParser(0));

        var exception = Should.Throw<ParserException>(() => parser.Parse("a+b"));

        exception.Message.ShouldContain("No appropriate parser for expression: \"a+b\"");
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
