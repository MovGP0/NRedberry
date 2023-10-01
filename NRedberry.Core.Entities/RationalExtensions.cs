using System.Numerics;

namespace NRedberry;

public static class RationalExtensions
{
    /// <summary>
    /// Gets the absolute value of the rational number.
    /// </summary>
    public static Rationals.Rational Abs(this Rationals.Rational p)
    {
        if (p.IsNaN)
            return Rationals.Rational.NaN;

        if (p.IsZero)
            return Rationals.Rational.Zero;

        var numerator = BigInteger.Abs(p.Numerator);
        var denominator = BigInteger.Abs(p.Denominator);
        var result = new Rationals.Rational(numerator, denominator);
        return result;
    }

    /// <summary>
    /// Gets the absolute value of the rational number.
    /// </summary>
    public static Rational Abs(Rational p)
    {
        if (p.IsNaN())
            return Rational.NaN;

        if (p.IsZero())
            return Rational.Zero;

        var numerator = BigInteger.Abs(p.Numerator);
        var denominator = BigInteger.Abs(p.Denominator);
        var result = new Rational(numerator, denominator);
        return result;
    }
}