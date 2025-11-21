using System.Numerics;
using NRedberry.Apache.Commons.Math;

namespace NRedberry.Numbers;

public interface INumber<T> : IFieldElement<T>
{
    BigInteger BigIntValue();
    int IntValue();
    long LongValue();
    double DoubleValue();
    float FloatValue();

    T GetNumericValue();

    T Abs();

    T Add(double bg);
    T Add(int i);
    T Add(long l);
    T Add(BigInteger bg);
    T Add(BigFraction fraction);

    T Subtract(double bg);
    T Subtract(int i);
    T Subtract(long l);
    T Subtract(BigInteger bg);
    T Subtract(BigFraction fraction);

    T Divide(double d);
    T Divide(int i);
    T Divide(long l);
    T Divide(BigInteger bg);
    T Divide(BigFraction fraction);

    T Multiply(double d);
    T Multiply(long l);
    T Multiply(BigInteger bg);
    T Multiply(BigFraction fraction);

    T Pow(double exponent);
    T Pow(BigInteger exponent);
    T Pow(long exponent);
    T Pow(int exponent);

    bool IsInfinite();
    bool IsNaN();
    bool IsZero();
    bool IsOne();
    bool IsMinusOne();
    bool IsNumeric();
    bool IsInteger();
    bool IsNatural();
}
