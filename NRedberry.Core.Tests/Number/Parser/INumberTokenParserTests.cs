using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class INumberTokenParserTests
{
    [Fact]
    public void Parse_ShouldPassTrimmedExpressionAndParserToToken()
    {
        var token = new ObservingToken(new Rational(5));
        var parser = new NumberParser<Real>([token]);

        var result = parser.Parse("  42  ");

        token.LastExpression.ShouldBe("42");
        token.LastParser.ShouldBeSameAs(parser);
        result.ShouldBe(new Rational(5));
        token.CallCount.ShouldBe(1);
    }

    [Fact]
    public void Parse_ShouldTryNextTokenWhenCurrentTokenReturnsNull()
    {
        var firstToken = new ObservingToken(null);
        var secondToken = new ObservingToken(new Rational(7));
        var parser = new NumberParser<Real>([firstToken, secondToken]);

        var result = parser.Parse("17");

        result.ShouldBe(new Rational(7));
        firstToken.CallCount.ShouldBe(1);
        secondToken.CallCount.ShouldBe(1);
    }

    [Fact]
    public void Parse_ShouldStopAfterFirstSuccessfulToken()
    {
        var firstToken = new ObservingToken(new Rational(11));
        var secondToken = new ObservingToken(new Rational(13));
        var parser = new NumberParser<Real>([firstToken, secondToken]);

        var result = parser.Parse("9");

        result.ShouldBe(new Rational(11));
        firstToken.CallCount.ShouldBe(1);
        secondToken.CallCount.ShouldBe(0);
    }

    [Fact]
    public void Parse_ShouldThrowFormatExceptionWhenAllTokensReturnNull()
    {
        var parser = new NumberParser<Real>([new ObservingToken(null)]);

        Should.Throw<FormatException>(() => parser.Parse("not-a-number"));
    }
}

internal sealed class ObservingToken(Real? result) : INumberTokenParser<Real>
{
    public int CallCount { get; private set; }

    public string? LastExpression { get; private set; }

    public NumberParser<Real>? LastParser { get; private set; }

    public Real Parse(string expression, NumberParser<Real> parser)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(parser);

        CallCount++;
        LastExpression = expression;
        LastParser = parser;
        return result!;
    }
}
