using System;
using System.Numerics;

namespace NRedberry.Core.Numbers;

public sealed class Numeric : Real
{
    public static Numeric Zero = new Numeric(0);
    public static Numeric One = new Numeric(1);
    public static Numeric MinusOne = new Numeric(-1);

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

    public static Numeric FromNumber<T>(INumber<T> value)
    {
        if(value is null) throw new ArgumentNullException(nameof(value));
        return new Numeric(value.DoubleValue());
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