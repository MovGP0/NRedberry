using System.Numerics;

namespace NRedberry;

public sealed class Numeric : Real
{
    /// <summary>
    /// A constant holding 0 value.
    /// </summary>
    public static readonly Numeric Zero = new Numeric(0);

    /// <summary>
    /// A constant holding 1 value.
    /// </summary>
    public static readonly Numeric One = new Numeric(1);

    /// <summary>
    /// A constant holding -1 value.
    /// </summary>
    public static readonly Numeric MinusOne = new Numeric(-1);

    /// <summary>
    /// A constant holding the positive infinity of type double.
    /// </summary>
    public static readonly Numeric PositiveInfinity = new Numeric(double.PositiveInfinity);

    /// <summary>
    /// A constant holding the negative infinity of type double.
    /// </summary>
    public static readonly Numeric NegativeInfinity = new Numeric(double.NegativeInfinity);

    /// <summary>
    /// A constant holding a Not-a-Number (NaN) value of type double.
    /// </summary>
    public static readonly Numeric NaN = new Numeric(double.NaN);

    /// <summary>
    /// A constant holding the largest positive finite value of type double, (2-2^-52)*2^1023.
    /// </summary>
    public static readonly Numeric MaxValue = new Numeric(double.MaxValue);

    /// <summary>
    /// A constant holding the smallest positive normal value of type double, 2^-1022.
    /// </summary>
    public static readonly Numeric MinNormal = new Numeric(double.MinValue);

    /// <summary>
    /// A constant holding the smallest positive nonzero value of type double, 2^-1074.
    /// </summary>
    public static readonly Numeric MinValue = new Numeric(double.Epsilon);

    private double Value { get; }

    public Numeric(double value)
    {
        Value = value;
    }

    public Numeric(int value)
    {
        Value = value;
    }

    public Numeric(float value)
    {
        Value = value;
    }

    public static Numeric FromNumber<T>(Number<T> value)
    {
        if(value is null) throw new ArgumentNullException(nameof(value));
        return new Numeric(value.DoubleValue());
    }

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