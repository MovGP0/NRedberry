using System.Numerics;
using NRedberry.Apache.Commons.Math;

namespace NRedberry;

public abstract class Real : INumber<Real>, IComparable<Real>
{
    public abstract Real Add(Real a);
    public abstract Real Add(Rationals.Rational fraction);
    public abstract Real Subtract(Real a);
    public abstract Real Negate();
    public abstract Real Multiply(int n);
    public abstract Real Multiply(Real a);
    public abstract Real Divide(Real a);
    public abstract Real Reciprocal();

    [Obsolete("Inject IField<Real> instead.")]
    public IField<Real> GetField()
    {
        return RealField.GetInstance();
    }

    public abstract int SigNum();
    public abstract int IntValue();
    public abstract long LongValue();
    public abstract double DoubleValue();
    public double ToDouble() => DoubleValue();
    public abstract float FloatValue();
    public abstract Real GetNumericValue();
    public abstract Real Abs();
    public abstract Real Add(double bg);
    public abstract Real Add(int i);
    public abstract Real Add(long l);
    public abstract Real Add(BigInteger bg);
    public abstract Real Add(Rational fraction);
    public abstract Real Subtract(double bg);
    public abstract Real Subtract(int i);
    public abstract Real Subtract(long l);
    public abstract Real Subtract(BigInteger bg);
    public abstract Real Subtract(Rational fraction);
    public abstract Real Divide(double d);
    public abstract Real Divide(int i);
    public abstract Real Divide(long l);
    public abstract Real Divide(BigInteger bg);
    public abstract Real Divide(Rational fraction);
    public abstract Real Multiply(double d);
    public abstract Real Multiply(long l);
    public abstract Real Multiply(BigInteger bg);
    public abstract Real Multiply(Rational fraction);
    public abstract Real Pow(double exponent);
    public abstract Real Pow(BigInteger exponent);
    public abstract Real Pow(long exponent);
    public abstract Real Pow(int exponent);
    public abstract bool IsInfinite();
    public abstract bool IsNaN();
    public abstract bool IsZero();
    public abstract bool IsOne();
    public abstract bool IsMinusOne();
    public abstract bool IsNumeric();
    public abstract bool IsInteger();
    public abstract bool IsNatural();
    public abstract int CompareTo(Real other);

    public IField<Real> Field => throw new NotImplementedException();
}
