using System.Numerics;

namespace NRedberry;

public sealed class Rational : Real
{
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

    public Rational(BigInteger number)
    {
        Fraction = new Rationals.Rational(number, 0);
    }

    public Rational(BigInteger numerator, BigInteger denominator)
    {
        Fraction = new Rationals.Rational(numerator, denominator);
    }

    public Rational(Rationals.Rational rational)
    {
        Fraction = rational;
    }

    public BigInteger GetNumerator() => Fraction.Numerator;
    public BigInteger GetDenominator() => Fraction.Denominator;

    public BigInteger Numerator => Fraction.Numerator;
    public BigInteger Denominator => Fraction.Denominator;

    public override Real Add(Real a)
    {
        throw new NotImplementedException();
    }

    public override Real Add(Rationals.Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Real Subtract(Real a)
    {
        throw new NotImplementedException();
    }

    public override Real Negate()
    {
        throw new NotImplementedException();
    }

    public override Real Multiply(int n)
    {
        throw new NotImplementedException();
    }

    public override Real Multiply(Real a)
    {
        throw new NotImplementedException();
    }

    public override Real Divide(Real a)
    {
        throw new NotImplementedException();
    }

    public override Real Reciprocal()
    {
        throw new NotImplementedException();
    }

    public override Real GetNumericValue()
    {
        throw new NotImplementedException();
    }

    public override Real Abs()
    {
        throw new NotImplementedException();
    }

    public override Real Add(double bg)
    {
        throw new NotImplementedException();
    }

    public override Real Add(int i)
    {
        throw new NotImplementedException();
    }

    public override Real Add(long l)
    {
        throw new NotImplementedException();
    }

    public override Real Add(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public override Real Add(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Real Subtract(double bg)
    {
        throw new NotImplementedException();
    }

    public override Real Subtract(int i)
    {
        throw new NotImplementedException();
    }

    public override Real Subtract(long l)
    {
        throw new NotImplementedException();
    }

    public override Real Subtract(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public override Real Subtract(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Real Divide(double d)
    {
        throw new NotImplementedException();
    }

    public override Real Divide(int i)
    {
        throw new NotImplementedException();
    }

    public override Real Divide(long l)
    {
        throw new NotImplementedException();
    }

    public override Real Divide(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public override Real Divide(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Real Multiply(double d)
    {
        throw new NotImplementedException();
    }

    public override Real Multiply(long l)
    {
        throw new NotImplementedException();
    }

    public override Real Multiply(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public override Real Multiply(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Real Pow(double exponent)
    {
        throw new NotImplementedException();
    }

    public override Real Pow(BigInteger exponent)
    {
        throw new NotImplementedException();
    }

    public override Real Pow(long exponent)
    {
        throw new NotImplementedException();
    }

    public override Real Pow(int exponent)
    {
        throw new NotImplementedException();
    }

    public override bool IsNumeric() => false;
    public override bool IsZero() =>Fraction.Numerator.Equals(BigInteger.Zero);
    public override bool IsOne() => Fraction.Numerator.Equals(BigInteger.One) && Fraction.Denominator.Equals(BigInteger.One);
    public override bool IsMinusOne() => Fraction.Numerator.Equals(BigInteger.MinusOne) && Fraction.Denominator.Equals(BigInteger.One);
    public override int SigNum() => Fraction.Numerator.Sign;

    public int Sign => Fraction.Numerator.Sign;

    public override bool IsInteger() => Fraction.Denominator.CompareTo(BigInteger.One) == 0;
    public override bool IsNatural() => Fraction.Numerator.Sign >= 0 && IsInteger();

    public override int CompareTo(Real other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (other is Numeric numeric)
        {
            return DoubleValue().CompareTo(numeric.DoubleValue());
        }

        return Fraction.CompareTo(((Rational)other).Fraction);
    }

    public override long LongValue()
    {
        return (long) Fraction;
    }

    public override int IntValue()
    {
        return (int) Fraction;
    }

    public override float FloatValue()
    {
        return (float) Fraction;
    }

    public override double DoubleValue()
    {
        return (double) Fraction;
    }

    public override int GetHashCode()
    {
        return Fraction.Abs().GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is Rational rational)
        {
            return Fraction.Equals(rational.Fraction);
        }

        if (obj is INumber num)
        {
            return num.DoubleValue() == DoubleValue();
        }

        return false;
    }

    public override string ToString() => Fraction.Numerator + (Fraction.Denominator.Equals(BigInteger.One) ? string.Empty : "/" + Fraction.Denominator);
    public override bool IsInfinite() => false;
    public override bool IsNaN() => false;
}
