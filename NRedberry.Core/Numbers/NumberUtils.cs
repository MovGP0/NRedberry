using System;
using System.Numerics;

namespace NRedberry.Core.Numbers;

public static class NumberUtils
{
    [Obsolete("Check for null and throw ArgumentNullException instead.")]
    public static void CheckNotNull(object o)
    {
        if(o is null) throw new ArgumentNullException(nameof(o));
    }

    public static Numeric CreateNumeric(double value)
    {
        return new Numeric(value);
    }

    public static Rational CreateRational(Rationals.Rational value)
    {
        return new Rational(value);
    }

    public static BigInteger Two = new BigInteger(2);

    public static BigInteger Sqrt(BigInteger value)
    {
        if(value.Sign < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "square root of negative number");
        var result = Rationals.Rational.RationalRoot(new Rationals.Rational(value), 2);
        return result.WholePart;
    }

    public static bool IsSqrtXxx(BigInteger value, BigInteger root)
    {
        var number = new Rationals.Rational(value);
        var lowerBound = Rationals.Rational.Pow(new Rationals.Rational(root), 2);
        var upperBound = Rationals.Rational.Pow(Rationals.Rational.Add(new Rationals.Rational(root), 1), 2);
        return lowerBound.CompareTo(number) <= 0
               && number.CompareTo(upperBound) < 0;
    }

    public static bool IsSqrt(BigInteger value, BigInteger root)
    {
        var number = new Rationals.Rational(value);
        var power = Rationals.Rational.Pow(new Rationals.Rational(root), 2);
        return number.CompareTo(power) == 0;
    }
}