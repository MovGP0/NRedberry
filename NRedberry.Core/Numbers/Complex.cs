using System;
using System.Numerics;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Numbers;

public sealed class Complex : Tensor, INumber<Complex>
{
    public static Complex Zero = new(Rational.Zero, Rational.Zero);
    public static Complex One = new(Rational.One, Rational.One);

    private Real Real { get; }
    private Real Imaginary { get; }

    public Complex(Real real, Real imaginary)
    {
        if (real is Numeric || imaginary is Numeric)
        {
            Real = real.GetNumericValue();
            Imaginary = imaginary.GetNumericValue();
        }
        else
        {
            Real = real;
            Imaginary = imaginary;
        }
    }

    public Complex(Real real)
    {
        if (real is Numeric)
        {
            Real = real.GetNumericValue();
            Imaginary = Numeric.Zero;
        }
        else
        {
            Real = real;
            Imaginary = Rational.Zero;
        }
    }

    public Real GetReal()
    {
        return Real;
    }

    public bool IsReal()
    {
        return Imaginary.IsZero();
    }

    protected override int Hash()
    {
        throw new NotImplementedException();
    }

    public override IIndices Indices => throw new NotImplementedException();

    public override Tensor this[int i] => throw new NotImplementedException();

    public override int Size { get; }

    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override ITensorBuilder GetBuilder()
    {
        throw new NotImplementedException();
    }

    public override ITensorFactory GetFactory()
    {
        throw new NotImplementedException();
    }

    public Complex Add(Complex a)
    {
        throw new NotImplementedException();
    }

    public Complex Subtract(Complex a)
    {
        throw new NotImplementedException();
    }

    public Complex Negate()
    {
        throw new NotImplementedException();
    }

    public Complex Multiply(int n)
    {
        throw new NotImplementedException();
    }

    public Complex Multiply(Complex a)
    {
        throw new NotImplementedException();
    }

    public Complex Divide(Complex a)
    {
        throw new NotImplementedException();
    }

    public Complex Reciprocal()
    {
        throw new NotImplementedException();
    }

    public IField<Complex> GetField()
    {
        throw new NotImplementedException();
    }

    public int IntValue()
    {
        throw new NotImplementedException();
    }

    public long LongValue()
    {
        throw new NotImplementedException();
    }

    public double DoubleValue()
    {
        throw new NotImplementedException();
    }

    public float FloatValue()
    {
        throw new NotImplementedException();
    }

    public Complex GetNumericValue()
    {
        throw new NotImplementedException();
    }

    public Complex Abs()
    {
        throw new NotImplementedException();
    }

    public Complex Add(double bg)
    {
        throw new NotImplementedException();
    }

    public Complex Add(int i)
    {
        throw new NotImplementedException();
    }

    public Complex Add(long l)
    {
        throw new NotImplementedException();
    }

    public Complex Add(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public Complex Add(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public Complex Subtract(double bg)
    {
        throw new NotImplementedException();
    }

    public Complex Subtract(int i)
    {
        throw new NotImplementedException();
    }

    public Complex Subtract(long l)
    {
        throw new NotImplementedException();
    }

    public Complex Subtract(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public Complex Subtract(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public Complex Divide(double d)
    {
        throw new NotImplementedException();
    }

    public Complex Divide(int i)
    {
        throw new NotImplementedException();
    }

    public Complex Divide(long l)
    {
        throw new NotImplementedException();
    }

    public Complex Divide(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public Complex Divide(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public Complex Multiply(double d)
    {
        throw new NotImplementedException();
    }

    public Complex Multiply(long l)
    {
        throw new NotImplementedException();
    }

    public Complex Multiply(BigInteger bg)
    {
        throw new NotImplementedException();
    }

    public Complex Multiply(Rational fraction)
    {
        throw new NotImplementedException();
    }

    public Complex Pow(double exponent)
    {
        throw new NotImplementedException();
    }

    public Complex Pow(BigInteger exponent)
    {
        throw new NotImplementedException();
    }

    public Complex Pow(long exponent)
    {
        throw new NotImplementedException();
    }

    public Complex Pow(int exponent)
    {
        throw new NotImplementedException();
    }

    public bool IsInfinite()
    {
        throw new NotImplementedException();
    }

    public bool IsNaN()
    {
        throw new NotImplementedException();
    }

    public bool IsZero()
    {
        throw new NotImplementedException();
    }

    public bool IsOne()
    {
        throw new NotImplementedException();
    }

    public bool IsMinusOne()
    {
        throw new NotImplementedException();
    }

    public bool IsNumeric()
    {
        throw new NotImplementedException();
    }

    public bool IsInteger()
    {
        throw new NotImplementedException();
    }

    public bool IsNatural()
    {
        throw new NotImplementedException();
    }
}