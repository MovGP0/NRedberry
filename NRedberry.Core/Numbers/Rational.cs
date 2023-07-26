using System;
using System.Numerics;

namespace NRedberry.Core.Numbers;

public sealed class Rational : Real
{
    private Rationals.Rational Fraction { get; set; }

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

    public override Real Add(Real a)
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

    public override int SigNum()
    {
        throw new NotImplementedException();
    }

    public override int IntValue()
    {
        throw new NotImplementedException();
    }

    public override long LongValue()
    {
        throw new NotImplementedException();
    }

    public override double DoubleValue()
    {
        throw new NotImplementedException();
    }

    public override float FloatValue()
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

    public override bool IsInfinite()
    {
        throw new NotImplementedException();
    }

    public override bool IsNaN()
    {
        throw new NotImplementedException();
    }

    public override bool IsZero()
    {
        throw new NotImplementedException();
    }

    public override bool IsOne()
    {
        throw new NotImplementedException();
    }

    public override bool IsMinusOne()
    {
        throw new NotImplementedException();
    }

    public override bool IsNumeric()
    {
        throw new NotImplementedException();
    }

    public override bool IsInteger()
    {
        throw new NotImplementedException();
    }

    public override bool IsNatural()
    {
        throw new NotImplementedException();
    }

    public override int CompareTo(Real other)
    {
        throw new NotImplementedException();
    }
}