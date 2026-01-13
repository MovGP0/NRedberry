using System.Globalization;
using System.Numerics;

namespace NRedberry;

public sealed class Numeric : Real, IEquatable<Numeric>
{
    /// <summary>
    /// A constant holding 0 value.
    /// </summary>
    public static readonly Numeric Zero = new(0);

    /// <summary>
    /// A constant holding 1 value.
    /// </summary>
    public static readonly Numeric One = new(1);

    /// <summary>
    /// A constant holding -1 value.
    /// </summary>
    public static readonly Numeric MinusOne = new(-1);

    /// <summary>
    /// A constant holding the positive infinity of type double.
    /// </summary>
    public static readonly Numeric PositiveInfinity = new(double.PositiveInfinity);

    /// <summary>
    /// A constant holding the negative infinity of type double.
    /// </summary>
    public static readonly Numeric NegativeInfinity = new(double.NegativeInfinity);

    /// <summary>
    /// A constant holding a Not-a-Number (NaN) value of type double.
    /// </summary>
    public static readonly Numeric NaN = new(double.NaN);

    /// <summary>
    /// A constant holding the largest positive finite value of type double, (2-2^-52)*2^1023.
    /// </summary>
    public static readonly Numeric MaxValue = new(double.MaxValue);

    /// <summary>
    /// A constant holding the smallest positive normal value of type double, 2^-1022.
    /// </summary>
    public static readonly Numeric MinNormal = new(BitConverter.Int64BitsToDouble(0x0010000000000000));

    /// <summary>
    /// A constant holding the smallest positive nonzero value of type double, 2^-1074.
    /// </summary>
    public static readonly Numeric MinValue = new(double.Epsilon);

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
        ArgumentNullException.ThrowIfNull(value);
        return CreateNumeric(value.DoubleValue());
    }

    public override Real Add(Real a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return Add(a.DoubleValue());
    }

    public override Real Add(Rationals.Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return Add((double)fraction);
    }

    public override Real Subtract(Real a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return Subtract(a.DoubleValue());
    }

    public override Real Negate()
    {
        return CreateNumeric(-Value);
    }

    public override Real Multiply(int n)
    {
        return n == 1 ? this : CreateNumeric(Value * n);
    }

    public override Real Multiply(Real a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return Multiply(a.DoubleValue());
    }

    public override Real Divide(Real a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return Divide(a.DoubleValue());
    }

    public override Real Reciprocal()
    {
        return CreateNumeric(1.0 / Value);
    }

    public override int SigNum()
    {
        return Value > 0 ? 1 : Value == 0 ? 0 : -1;
    }

    public override int IntValue()
    {
        return (int)Value;
    }

    public BigInteger BigIntValue()
    {
        return new BigInteger(Value);
    }

    public override long LongValue()
    {
        return (long)Value;
    }

    public override double DoubleValue()
    {
        return Value;
    }

    public override float FloatValue()
    {
        return (float)Value;
    }

    public override Real GetNumericValue()
    {
        return this;
    }

    public override Real Abs()
    {
        return Value >= 0 ? this : Negate();
    }

    public override Real Add(double bg)
    {
        return bg == 0.0 ? this : CreateNumeric(Value + bg);
    }

    public override Real Add(int i)
    {
        return Add((double)i);
    }

    public override Real Add(long l)
    {
        return Add((double)l);
    }

    public override Real Add(BigInteger bg)
    {
        return Add((double)bg);
    }

    public override Real Add(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return Add(fraction.DoubleValue());
    }

    public override Real Subtract(double bg)
    {
        return Add(-bg);
    }

    public override Real Subtract(int i)
    {
        return Add(-(double)i);
    }

    public override Real Subtract(long l)
    {
        return Add(-(double)l);
    }

    public override Real Subtract(BigInteger bg)
    {
        return Add(-(double)bg);
    }

    public override Real Subtract(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return Add(-fraction.DoubleValue());
    }

    public override Real Divide(double d)
    {
        return d == 1.0 ? this : CreateNumeric(Value / d);
    }

    public override Real Divide(int i)
    {
        return Divide((double)i);
    }

    public override Real Divide(long l)
    {
        return Divide((double)l);
    }

    public override Real Divide(BigInteger bg)
    {
        return Divide((double)bg);
    }

    public override Real Divide(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return Divide(fraction.DoubleValue());
    }

    public override Real Multiply(double d)
    {
        return d == 1.0 ? this : CreateNumeric(Value * d);
    }

    public override Real Multiply(long l)
    {
        return Multiply((double)l);
    }

    public override Real Multiply(BigInteger bg)
    {
        return Multiply((double)bg);
    }

    public override Real Multiply(Rational fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        return Multiply(fraction.DoubleValue());
    }

    public override Real Pow(double exponent)
    {
        return CreateNumeric(Math.Pow(Value, exponent));
    }

    public override Real Pow(BigInteger exponent)
    {
        return Pow((double)exponent);
    }

    public override Real Pow(long exponent)
    {
        return Pow((double)exponent);
    }

    public override Real Pow(int exponent)
    {
        return Pow((double)exponent);
    }

    public override bool IsInfinite()
    {
        return double.IsInfinity(Value);
    }

    public override bool IsNaN()
    {
        return double.IsNaN(Value);
    }

    public override bool IsZero()
    {
        return Value == 0.0;
    }

    public override bool IsOne()
    {
        return Value == 1.0;
    }

    public override bool IsMinusOne()
    {
        return Value == -1.0;
    }

    public override bool IsNumeric()
    {
        return true;
    }

    public override bool IsInteger()
    {
        return false;
    }

    public override bool IsNatural()
    {
        return false;
    }

    public override int CompareTo(Real? other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Value.CompareTo(other.DoubleValue());
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null)
            return false;
        if (obj is Numeric numeric)
            return Equals(numeric);
        if (obj is INumber number)
            return BitConverter.DoubleToInt64Bits(Value) == BitConverter.DoubleToInt64Bits(number.DoubleValue());
        return false;
    }

    public bool Equals(Numeric? other)
    {
        if (other is null)
            return false;
        return BitConverter.DoubleToInt64Bits(Value) == BitConverter.DoubleToInt64Bits(other.Value);
    }

    public override int GetHashCode()
    {
        if (Value == One.Value)
            return 1;
        if (Value == MinusOne.Value)
            return -1;

        var bits = BitConverter.DoubleToInt64Bits(Value * Value);
        var hash = (int)(bits ^ (bits >> 32));
        return Value > 0.0 ? hash : -hash;
    }

    public static bool operator ==(Numeric? left, Numeric? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(Numeric? left, Numeric? right)
    {
        return !(left == right);
    }

    private static Numeric CreateNumeric(double value)
    {
        if (value == 0.0)
            return Zero;
        if (value == 1.0)
            return One;
        if (value == -1.0)
            return MinusOne;
        if (double.IsPositiveInfinity(value))
            return PositiveInfinity;
        if (double.IsNegativeInfinity(value))
            return NegativeInfinity;
        if (double.IsNaN(value))
            return NaN;
        return new Numeric(value);
    }
}
