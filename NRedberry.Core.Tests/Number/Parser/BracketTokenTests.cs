using NRedberry;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class BracketTokenTests
{
    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var parser = CreateCapturingParser();

        Assert.Throws<ArgumentNullException>(() => BracketToken<Real>.Instance.Parse(null!, parser.Parser));
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => BracketToken<Real>.Instance.Parse("(1+2)", null!));
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionIsNotWrappedByOuterBrackets()
    {
        var parser = CreateCapturingParser();

        var result = BracketToken<Real>.Instance.Parse("1+2", parser.Parser);

        Assert.Null(result);
        Assert.Null(parser.Token.LastExpression);
    }

    [Fact]
    public void ShouldDelegateInnerExpressionWhenOuterBracketsAreBalanced()
    {
        var parser = CreateCapturingParser();

        var result = BracketToken<Real>.Instance.Parse("(1+2)", parser.Parser);

        Assert.Same(parser.Token.Result, result);
        Assert.Equal("1+2", parser.Token.LastExpression);
    }

    [Fact]
    public void ShouldReturnNullWhenClosingBracketAppearsBeforeCompletionOfOuterScope()
    {
        var parser = CreateCapturingParser();

        var result = BracketToken<Real>.Instance.Parse("())", parser.Parser);

        Assert.Null(result);
        Assert.Null(parser.Token.LastExpression);
    }

    [Fact]
    public void ShouldDelegateEvenWhenFinalBracketLevelIsNotZero()
    {
        var parser = CreateCapturingParser();

        var result = BracketToken<Real>.Instance.Parse("((1+2)", parser.Parser);

        Assert.Same(parser.Token.Result, result);
        Assert.Equal("(1+2", parser.Token.LastExpression);
    }

    private static TestParserContext CreateCapturingParser()
    {
        var token = new CapturingTokenParser();
        var parser = new NumberParser<Real>([token]);
        return new TestParserContext(parser, token);
    }
}

internal sealed record TestParserContext(NumberParser<Real> Parser, CapturingTokenParser Token);

internal sealed class CapturingTokenParser : INumberTokenParser<Real>
{
    public string? LastExpression { get; private set; }

    public Real Result { get; } = Rational.One;

    public Real Parse(string expression, NumberParser<Real> parser)
    {
        LastExpression = expression;
        return Result;
    }
}
