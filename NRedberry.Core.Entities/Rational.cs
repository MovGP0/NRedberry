using System.Numerics;

namespace NRedberry;

/// <summary>
/// Representation of a rational number without overflow. This class keeps
/// values canonical by wrapping <see cref="Rationals.Rational"/> and delegates
/// operations to numeric fallbacks when necessary.
/// </summary>
public sealed class Rational : Real
{
    private static readonly BigInteger BiMinusOne = BigInteger.MinusOne;

    private Rationals.Rational Fraction { get; }

    /// <summary>
    /// Not a Number (after a division by 0).
    /// </summary>
    public static Rational NaN { get; } = new(Rationals.Rational.NaN);

    /// <summary>
    /// A fraction representing "2 / 1".
    /// </summary>
    public static Rational Two { get; } = new(2);

    /// <summary>
    /// A fraction representing "4 / 1".
    /// </summary>
    public static Rational Four { get; } = new(4);

    /// <summary>
    /// A fraction representing "-2 / 1".
    /// </summary>
    public static Rational MinusTwo { get; } = new(-2);

    /// <summary>
    /// A fraction representing "1".
    /// </summary>
    public static Rational One { get; } = new(1);

    /// <summary>
    /// A fraction representing "0".
    /// </summary>
    public static Rational Zero { get; } = new(0);

    /// <summary>
    /// A fraction representing "-1 / 1".
    /// </summary>
    public static Rational MinusOne { get; } = new(-1);

    /// <summary>
    /// A fraction representing "4/5".
    /// </summary>
    public static Rational FourFifths { get; } = new(4, 5);

    /// <summary>
    /// A fraction representing "1/5".
    /// </summary>
    public static Rational OneFifth { get; } = new(1, 5);

    /// <summary>
    /// A fraction representing "1/2".
    /// </summary>
    public static Rational OneHalf { get; } = new(1, 2);

    /// <summary>
    /// A fraction representing "-1/2".
    /// </summary>
    public static Rational MinusOneHalf { get; } = new(-1, 2);

    /// <summary>
    /// A fraction representing "1/4".
    /// </summary>
    public static Rational OneQuarter { get; } = new(1, 4);

    /// <summary>
    /// A fraction representing "1/3".
    /// </summary>
    public static Rational OneThird { get; } = new(1, 3);

    /// <summary>
    /// A fraction representing "3/5".
    /// </summary>
    public static Rational ThreeFifths { get; } = new(3, 5);

    /// <summary>
    /// A fraction representing "3/4".
    /// </summary>
    public static Rational ThreeQuarters { get; } = new(3, 4);

    /// <summary>
    /// A fraction representing "2/5".
    /// </summary>
    public static Rational TwoFifths { get; } = new(2, 5);

    /// <summary>
    /// A fraction representing "2/3".
    /// </summary>
    public static Rational TwoThirds { get; } = new(2, 3);

    /// <summary>
    /// Creates a rational representing the specified whole <see cref="BigInteger"/>.
    /// </summary>
    public Rational(BigInteger number)
    {
        Fraction = new Rationals.Rational(number);
    }

    /// <summary>
    /// Initializes a rational set to <paramref name="numerator"/>/<paramref name="denominator"/>.
    /// </summary>
    public Rational(BigInteger numerator, BigInteger denominator)
    {
        Fraction = new Rationals.Rational(numerator, denominator);
    }

    /// <summary>
    /// Wraps an existing <see cref="Rationals.Rational"/>, keeping the underlying value intact.
    /// </summary>
    public Rational(Rationals.Rational rational)
    {
        Fraction = rational;
    }

    /// <summary>
    /// Gets the numerator of the underlying rational.
    /// </summary>
    public BigInteger Numerator => Fraction.Numerator;

    /// <summary>
    /// Gets the denominator of the underlying rational.
    /// </summary>
    public BigInteger Denominator => Fraction.Denominator;

    public override Real Add(Real other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (other is Numeric numeric)
            return numeric.Add(this);
        if (other is Rational rational)
            return FromFraction(Fraction + rational.Fraction);
        return new Numeric(DoubleValue() + other.DoubleValue());
    }

    public override Real Add(Rationals.Rational fraction) => FromFraction(Fraction + fraction);

    public override Real Add(double bg) => new Numeric(DoubleValue() + bg);

    public override Real Add(int i) => FromFraction(Fraction + (Rationals.Rational)i);

    public override Real Add(long l) => FromFraction(Fraction + (Rationals.Rational)l);

    public override Real Add(BigInteger bg) => FromFraction(Fraction + (Rationals.Rational)bg);

    public override Real Add(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return FromFraction(Fraction + fraction.Fraction);
    }

    public override Real Subtract(Real other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (other is Numeric numeric)
            return new Numeric(DoubleValue() - numeric.DoubleValue());
        if (other is Rational rational)
            return FromFraction(Fraction - rational.Fraction);
        return new Numeric(DoubleValue() - other.DoubleValue());
    }

    public override Real Subtract(double bg) => new Numeric(DoubleValue() - bg);

    public override Real Subtract(int i) => FromFraction(Fraction - (Rationals.Rational)i);

    public override Real Subtract(long l) => FromFraction(Fraction - (Rationals.Rational)l);

    public override Real Subtract(BigInteger bg) => FromFraction(Fraction - (Rationals.Rational)bg);

    public override Real Subtract(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return FromFraction(Fraction - fraction.Fraction);
    }

    public override Real Multiply(int n)
    {
        return n switch
        {
            0 => Zero,
            1 => this,
            _ => FromFraction(Fraction * (Rationals.Rational)n)
        };
    }

    public override Real Multiply(Real other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (other is Numeric numeric)
            return numeric.Multiply(this);
        if (other.IsOne())
            return this;
        if (other.IsZero())
            return Zero;
        if (other is Rational rational)
            return FromFraction(Fraction * rational.Fraction);
        return new Numeric(DoubleValue() * other.DoubleValue());
    }

    public override Real Divide(Real other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (other is Numeric numeric || other.IsZero())
            return new Numeric(DoubleValue() / other.DoubleValue());
        if (other is Rational rational)
            return FromFraction(Fraction / rational.Fraction);
        return new Numeric(DoubleValue() / other.DoubleValue());
    }

    public override Real Negate() => FromFraction(-Fraction);

    public override Real Multiply(double d) => new Numeric(DoubleValue() * d);

    public override Real Multiply(long l) => FromFraction(Fraction * (Rationals.Rational)l);

    public override Real Multiply(BigInteger bg) => FromFraction(Fraction * (Rationals.Rational)bg);

    public override Real Multiply(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        if (fraction.IsOne())
            return this;
        if (fraction.IsZero())
            return Zero;
        return FromFraction(Fraction * fraction.Fraction);
    }

    public override Real Divide(double d) => new Numeric(DoubleValue() / d);

    public override Real Divide(int i) => FromFraction(Fraction / (Rationals.Rational)i);

    public override Real Divide(long l) => FromFraction(Fraction / (Rationals.Rational)l);

    public override Real Divide(BigInteger bg) => FromFraction(Fraction / (Rationals.Rational)bg);

    public override Real Divide(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return FromFraction(Fraction / fraction.Fraction);
    }

    public override Real Reciprocal() => FromFraction(Rationals.Rational.Invert(Fraction));

    public override Real Pow(double exponent) => new Numeric(Math.Pow(DoubleValue(), exponent));

    public override Real Pow(BigInteger exponent) => FromFraction(Power(Fraction, exponent));

    public override Real Pow(long exponent) => FromFraction(Power(Fraction, new BigInteger(exponent)));

    public override Real Pow(int exponent) => FromFraction(Power(Fraction, new BigInteger(exponent)));

    public override Real Abs() => FromFraction(Rationals.Rational.Abs(Fraction));

    public override Real GetNumericValue() => new Numeric(DoubleValue());

    public override int SigNum() => Fraction.Sign;

    public override int IntValue()
    {
        var integerPart = Fraction.Numerator / Fraction.Denominator;
        return ConvertToInt(integerPart);
    }

    public override long LongValue()
    {
        var integerPart = Fraction.Numerator / Fraction.Denominator;
        return ConvertToLong(integerPart);
    }

    public override float FloatValue()
    {
        var value = (float)Fraction;
        if (float.IsInfinity(value))
            throw new OverflowException("Float overflow.");
        return value;
    }

    public override double DoubleValue()
    {
        var value = (double)Fraction;
        if (double.IsInfinity(value))
            throw new OverflowException("Double overflow.");
        return value;
    }

    public override bool IsInfinite() => false;

    public override bool IsNaN() => Fraction.IsNaN;

    public override bool IsZero() => Fraction.IsZero;

    public override bool IsOne() => Fraction.IsOne;

    public override bool IsMinusOne()
    {
        return Fraction.Numerator.Equals(BiMinusOne)
            && Fraction.Denominator.Equals(BigInteger.One);
    }

    public override bool IsNumeric() => false;

    public override bool IsInteger() => Fraction.Denominator.Equals(BigInteger.One);

    public override bool IsNatural() => Fraction.Sign >= 0 && IsInteger();

    public override int CompareTo(Real other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (other is Numeric numeric)
            return DoubleValue().CompareTo(numeric.DoubleValue());
        if (other is Rational rational)
            return Fraction.CompareTo(rational.Fraction);
        return DoubleValue().CompareTo(other.DoubleValue());
    }

    public override int GetHashCode() => Fraction.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is Rational rational)
            return Fraction.Equals(rational.Fraction);
        if (obj is INumber num)
            return num.DoubleValue() == DoubleValue();
        return false;
    }

    public override string ToString()
    {
        return Fraction.Numerator + (Fraction.Denominator.Equals(BigInteger.One) ? string.Empty : "/" + Fraction.Denominator);
    }

    public static Rational operator +(Rational left, Rational right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return FromFraction(left.Fraction + right.Fraction);
    }

    public static Rational operator -(Rational left, Rational right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return FromFraction(left.Fraction - right.Fraction);
    }

    public static Rational operator *(Rational left, Rational right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        if (right.IsOne())
            return left;
        if (right.IsZero())
            return Zero;
        return FromFraction(left.Fraction * right.Fraction);
    }

    public static Rational operator /(Rational left, Rational right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return FromFraction(left.Fraction / right.Fraction);
    }

    public static Rational operator -(Rational value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return FromFraction(-value.Fraction);
    }

    public static bool operator ==(Rational? left, Rational? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (left is null || right is null)
            return false;
        return left.Fraction.Equals(right.Fraction);
    }

    public static bool operator !=(Rational? left, Rational? right) => !(left == right);

    public static Real operator +(Rational left, double right) => left.Add(right);
    public static Real operator +(Rational left, long right) => left.Add(right);
    public static Real operator +(Rational left, BigInteger right) => left.Add(right);

    public static Real operator -(Rational left, double right) => left.Subtract(right);
    public static Real operator -(Rational left, long right) => left.Subtract(right);
    public static Real operator -(Rational left, BigInteger right) => left.Subtract(right);

    public static Real operator *(Rational left, double right) => left.Multiply(right);
    public static Real operator *(Rational left, long right) => left.Multiply(right);
    public static Real operator *(Rational left, BigInteger right) => left.Multiply(right);

    public static Real operator /(Rational left, double right) => left.Divide(right);
    public static Real operator /(Rational left, long right) => left.Divide(right);
    public static Real operator /(Rational left, BigInteger right) => left.Divide(right);

    public static Rational operator +(Rational value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value;
    }

    public static Rational operator ++(Rational value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return FromFraction(value.Fraction + One.Fraction);
    }

    public static Rational operator --(Rational value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return FromFraction(value.Fraction - One.Fraction);
    }

    private static Rational FromFraction(Rationals.Rational rational)
    {
        return new Rational(rational);
    }

    private static long ConvertToLong(BigInteger value)
    {
        if (value > long.MaxValue || value < long.MinValue)
            throw new OverflowException("Long overflow.");
        return (long)value;
    }

    private static int ConvertToInt(BigInteger value)
    {
        if (value > int.MaxValue || value < int.MinValue)
            throw new OverflowException("Integer overflow.");
        return (int)value;
    }

    private static Rationals.Rational Power(Rationals.Rational value, BigInteger exponent)
    {
        if (exponent.IsZero)
            return Rationals.Rational.One;

        if (exponent.Sign < 0)
        {
            var positive = Power(value, BigInteger.Abs(exponent));
            return Rationals.Rational.Invert(positive);
        }

        var result = Rationals.Rational.One;
        var baseValue = value;
        var exp = exponent;

        while (exp > 0)
        {
            if (!exp.IsEven)
                result *= baseValue;
            baseValue *= baseValue;
            exp >>= 1;
        }

        return result;
    }
}
