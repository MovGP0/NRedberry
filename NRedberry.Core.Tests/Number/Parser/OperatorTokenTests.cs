using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class OperatorTokenTests
{
    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var exception = Assert.Throws<ArgumentNullException>(() => token.Parse(null!, parser));

        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        var token = new AddSubtractToken();

        var exception = Assert.Throws<ArgumentNullException>(() => token.Parse("1+2", null!));

        Assert.Equal("parser", exception.ParamName);
    }

    [Fact]
    public void ShouldReturnNullWhenNoTopLevelOperatorIsPresent()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("(1+2)", parser);

        Assert.Null(result);
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsExponentMarker()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("2**3", parser);

        Assert.Null(result);
    }

    [Fact]
    public void ShouldApplyOperationsLeftToRightAtTopLevel()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("10-3+2", parser);

        Assert.Equal(new Rational(9), result);
    }

    [Fact]
    public void ShouldIgnoreOperatorsInsideParentheses()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("10-(3+2)", parser);

        Assert.Equal(new Rational(5), result);
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenClosingBracketAppearsBeforeOpening()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var exception = Assert.Throws<BracketsError>(() => token.Parse(")1+2", parser));

        Assert.Equal("Unbalanced brackets in )1+2", exception.Message);
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenExpressionBecomesUnbalancedAfterTopLevelDetection()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var exception = Assert.Throws<BracketsError>(() => token.Parse("1+2)", parser));

        Assert.Equal("Unbalanced brackets.", exception.Message);
    }

    private static NumberParser<Real> CreateParser(AddSubtractToken token)
    {
        return new NumberParser<Real>([BracketToken<Real>.Instance, token, RealToken.Instance]);
    }

    private sealed class AddSubtractToken() : OperatorToken<Real>('+', '-')
    {
        protected override Real Neutral()
        {
            return Rational.Zero;
        }

        protected override Real Operation(Real c1, Real c2, bool mode)
        {
            if (mode)
            {
                return c1.Subtract(c2);
            }

            return c1.Add(c2);
        }
    }
}
