using NRedberry.Numbers.Parser;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class OperatorTokenTests
{
    [Fact]
    public void ShouldThrowWhenExpressionIsNull()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var exception = Should.Throw<ArgumentNullException>(() => token.Parse(null!, parser));

        exception.ParamName.ShouldBe("expression");
    }

    [Fact]
    public void ShouldThrowWhenParserIsNull()
    {
        var token = new AddSubtractToken();

        var exception = Should.Throw<ArgumentNullException>(() => token.Parse("1+2", null!));

        exception.ParamName.ShouldBe("parser");
    }

    [Fact]
    public void ShouldReturnNullWhenNoTopLevelOperatorIsPresent()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("(1+2)", parser);

        result.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenExpressionContainsExponentMarker()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("2**3", parser);

        result.ShouldBeNull();
    }

    [Fact]
    public void ShouldApplyOperationsLeftToRightAtTopLevel()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("10-3+2", parser);

        result.ShouldBe(new Rational(9));
    }

    [Fact]
    public void ShouldIgnoreOperatorsInsideParentheses()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var result = token.Parse("10-(3+2)", parser);

        result.ShouldBe(new Rational(5));
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenClosingBracketAppearsBeforeOpening()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var exception = Should.Throw<BracketsError>(() => token.Parse(")1+2", parser));

        exception.Message.ShouldBe("Unbalanced brackets in )1+2");
    }

    [Fact]
    public void ShouldThrowBracketsErrorWhenExpressionBecomesUnbalancedAfterTopLevelDetection()
    {
        var token = new AddSubtractToken();
        var parser = CreateParser(token);

        var exception = Should.Throw<BracketsError>(() => token.Parse("1+2)", parser));

        exception.Message.ShouldBe("Unbalanced brackets.");
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
