namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/NumberParser.java
 */

public sealed class NumberParser<T>
    where T : NRedberry.INumber<T>
{
    private static readonly INumberTokenParser<Complex>[] ComplexTokens =
    [
        BracketToken<Complex>.Instance,
        new ComplexAddSubtractOperatorToken(),
        new ComplexMultiplyDivideOperatorToken(),
        ComplexToken.Instance
    ];

    private static readonly INumberTokenParser<Real>[] RealTokens =
    [
        BracketToken<Real>.Instance,
        new RealAddSubtractOperatorToken(),
        new RealMultiplyDivideOperatorToken(),
        RealToken.Instance
    ];

    public static NumberParser<Real> RealParser { get; } = new(RealTokens);

    public static NumberParser<Complex> ComplexParser { get; } = new(ComplexTokens);

    private readonly INumberTokenParser<T>[] _parsers;

    public NumberParser(INumberTokenParser<T>[] parsers)
    {
        ArgumentNullException.ThrowIfNull(parsers);
        _parsers = parsers;
    }

    public T Parse(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var normalized = expression.Trim();
        foreach (var parser in _parsers)
        {
            var element = parser.Parse(normalized, this);
            if (element is null)
            {
                continue;
            }

            return element;
        }

        throw new FormatException();
    }
}

internal sealed class ComplexAddSubtractOperatorToken : OperatorToken<Complex>
{
    public ComplexAddSubtractOperatorToken()
        : base('+', '-')
    {
    }

    protected override Complex Neutral()
    {
        return Complex.Zero;
    }

    protected override Complex Operation(Complex c1, Complex c2, bool mode)
    {
        if (mode)
        {
            return c1.Subtract(c2);
        }

        return c1.Add(c2);
    }
}

internal sealed class ComplexMultiplyDivideOperatorToken : OperatorToken<Complex>
{
    public ComplexMultiplyDivideOperatorToken()
        : base('*', '/')
    {
    }

    protected override Complex Neutral()
    {
        return Complex.One;
    }

    protected override Complex Operation(Complex c1, Complex c2, bool mode)
    {
        if (mode)
        {
            return c1.Divide(c2);
        }

        return c1.Multiply(c2);
    }
}

internal sealed class RealAddSubtractOperatorToken : OperatorToken<Real>
{
    public RealAddSubtractOperatorToken()
        : base('+', '-')
    {
    }

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

internal sealed class RealMultiplyDivideOperatorToken : OperatorToken<Real>
{
    public RealMultiplyDivideOperatorToken()
        : base('*', '/')
    {
    }

    protected override Real Neutral()
    {
        return Rational.One;
    }

    protected override Real Operation(Real c1, Real c2, bool mode)
    {
        if (mode)
        {
            return c1.Divide(c2);
        }

        return c1.Multiply(c2);
    }
}
