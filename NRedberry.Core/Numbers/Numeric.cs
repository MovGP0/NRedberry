using System;
using System.Numerics;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization;

namespace NRedberry.Core.Numbers;

[Serializable]
public sealed class Numeric : Real, ISerializable
{
    public static readonly Numeric Zero = new(0);
    public static readonly Numeric One = new(1);
    public static readonly Numeric MinusOne = new(-1);
    public static readonly Numeric PositiveInfinity = new(double.PositiveInfinity);
    public static readonly Numeric NegativeInfinity = new(double.NegativeInfinity);
    public static readonly Numeric NaN = new(double.NaN);
    public static readonly Numeric MaxValue = new(double.MaxValue);
    public static readonly Numeric MinNormal = new(double.Epsilon);
    public static readonly Numeric MinValue = new(double.MinValue);

    private readonly double value;

    public Numeric(double value)
    {
        this.value = value;
    }

    public Numeric(int value)
    {
        this.value = value;
    }

    public Numeric(float value)
    {
        this.value = value;
    }

    public Numeric(JSType.Number value)
    {
        var ci = CultureInfo.InvariantCulture;
        if (value == null) throw new ArgumentNullException(nameof(value));
        this.value = double.Parse(value.ToString() ?? "0", ci);
    }

    public override double DoubleValue() => value;
    public override float FloatValue() => (float)value;
    public override int SigNum()
    {
        throw new NotImplementedException();
    }

    public override int IntValue() => (int)value;
    public BigInteger BigIntValue() => new (value);
    public override long LongValue() => (long)value;
    public override Numeric Add(Real n) => Add(n.DoubleValue());
    public override Real Add(Rationals.Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Numeric Divide(Real n) => Divide(n.DoubleValue());
    public override Numeric Multiply(int n) => new(value * n);
    public override Numeric Multiply(Real n) => Multiply(n.DoubleValue());
    public override Numeric Subtract(Real n) => Subtract(n.DoubleValue());
    public override Numeric Negate() => new(-value);
    public override Numeric Reciprocal() => new(1.0 / value);
    public override int GetHashCode() => value.GetHashCode();
    public override Real Pow(int exponent)
    {
        throw new NotImplementedException();
    }

    public override bool IsInfinite() => double.IsInfinity(value);
    public override bool IsNaN() => double.IsNaN(value);
    public override Numeric Abs() => value >= 0 ? this : Negate();
    public override Numeric Add(double d) => new(value + d);
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

    public override Real Subtract(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public override Numeric Divide(double d) => new(value / d);
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

    public override Numeric Multiply(double d) => new(value * d);
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

    public override Numeric Subtract(double d) => new(value - d);
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

    public override Numeric Pow(double exponent) => new(Math.Pow(value, exponent));
    public override Real Pow(BigInteger exponent)
    {
        throw new NotImplementedException();
    }

    public override Real Pow(long exponent)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj) => obj is JSType.Number n && value.Equals(n);
    public override string ToString() => value.ToString();
    public override Numeric GetNumericValue() => this;
    public override bool IsZero() => value == 0.0;
    public override int CompareTo(Real o) => value.CompareTo(o.DoubleValue());
    public override bool IsOne() => value == 1.0;
    public override bool IsMinusOne() => value == -1.0;
    public override bool IsNumeric()
    {
        throw new NotImplementedException();
    }

    public int Signum() => Math.Sign(value);
    public override bool IsInteger() => false;
    public override bool IsNatural() => false;

    private Numeric(SerializationInfo info, StreamingContext context)
    {
        value = info.GetDouble("value");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("value", value);
    }
}