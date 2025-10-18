using System.Numerics;
using NRedberry.Apache.Commons.Math;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;
using Complex32 = System.Numerics.Complex;

namespace NRedberry.Core.Numbers;

public sealed class Complex : Tensor, INumber<Complex>
{
    public Complex32 ToComplex32() => new((float)Real.ToDouble(), (float)Imaginary.ToDouble());
    public static implicit operator Complex32(Complex c) => new((float)c.Real.ToDouble(), (float)c.Imaginary.ToDouble());

    public static readonly Complex ComplexNaN = new(Numeric.NaN, Numeric.NaN);
    public static readonly Complex RealPositiveInfinity = new(Numeric.PositiveInfinity, Numeric.Zero);
    public static readonly Complex RealNegativeInfinity = new(Numeric.NegativeInfinity, Numeric.Zero);
    public static readonly Complex ImaginaryPositiveInfinity = new(Numeric.Zero, Numeric.PositiveInfinity);
    public static readonly Complex ImaginaryNegativeInfinity = new(Numeric.Zero, Numeric.NegativeInfinity);
    public static readonly Complex ComplexNegativeInfinity = new(Numeric.NegativeInfinity, Numeric.NegativeInfinity);
    public static readonly Complex ComplexPositiveInfinity = new(Numeric.PositiveInfinity, Numeric.PositiveInfinity);
    public static readonly Complex ComplexInfinity = ComplexPositiveInfinity;
    public static readonly Complex Zero = new(Rational.Zero, Rational.Zero);
    public static readonly Complex One = new(Rational.One, Rational.Zero);
    public static readonly Complex OneHalf = new(Rational.OneHalf, Rational.Zero);
    public static readonly Complex Two = new(Rational.Two, Rational.Zero);
    public static readonly Complex Four = new(Rational.Four, Rational.Zero);
    public static readonly Complex MinusOne = new(Rational.MinusOne, Rational.Zero);
    public static readonly Complex MinusOneHalf = new(Rational.MinusOneHalf, Rational.Zero);
    public static readonly Complex MinusTwo = new(Rational.MinusTwo, Rational.Zero);
    public static readonly Complex ImaginaryOne = new(Rational.Zero, Rational.One);

    public Real Real { get; }
    public Real Imaginary { get; }

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

    public Complex(int real, int imaginary) : this(new Rational(real), new Rational(imaginary))
    {
    }

    public Complex(int real) : this(new Rational(real), Rational.Zero)
    {
    }

    public Complex(double real, double imaginary) : this(new Numeric(real), new Numeric(imaginary))
    {
    }

    public Complex(int real, double imaginary) : this(new Numeric(real), new Numeric(imaginary))
    {
    }

    public Complex(double real, int imaginary) : this(new Numeric(real), new Numeric(imaginary))
    {
    }

    public Complex(double real) : this(new Numeric(real), Numeric.Zero)
    {
    }

    public Complex(long real) : this(new Rational(real), Rational.Zero)
    {
    }

    public Complex(long real, long imaginary) : this(new Rational(real), new Rational(imaginary))
    {
    }

    public Complex(BigInteger real, BigInteger imaginary) : this(new Rational(real), new Rational(imaginary))
    {
    }

    public Complex(BigInteger real) : this(new Rational(real), Rational.Zero)
    {
    }

    public override Tensor this[int i] => throw new ArgumentOutOfRangeException(nameof(i));

    public override Indices.Indices Indices => IndicesFactory.EmptyIndices;

    public Real GetReal()
    {
        return Real;
    }

    public bool IsReal()
    {
        return Imaginary.IsZero();
    }

    public override int GetHashCode()
    {
        return 47 * (329 + Real.GetHashCode()) + Imaginary.GetHashCode();
    }

    public override int Size => 0;

    public override string ToString(OutputFormat outputFormat)
    {
        if (Real.IsZero())
        {
            if (Imaginary.IsZero())
                return "0";
            if (Imaginary.IsOne())
                return "I";
            if (Imaginary.IsMinusOne())
                return "-I";

            return Imaginary + "*I";
        }
        int @is = Imaginary.SigNum();
        if (@is == 0)
        {
            return Real.ToString();
        }
        Real abs = Imaginary.Abs();
        if (@is < 0)
        {
            return abs.IsOne() ? Real + "-I" : Real + "-I*" + Imaginary.Abs();
        }
        return abs.IsOne() ? Real + "+I" : Real + "+I*" + Imaginary.Abs();
    }

    protected override string ToString<T>(OutputFormat mode)
    {
        if (typeof(T) == typeof(Product) || typeof(T) == typeof(Power))
        {
            if (!Imaginary.IsZero() || Real.SigNum() < 0 || !Real.IsInteger())
                return "(" + ToString(mode) + ")";
        }
        return ToString(mode);
    }

    public override TensorBuilder GetBuilder() => new ComplexBuilder(this);
    public override TensorFactory? GetFactory() => null;

    public /*override*/ Complex Add(Complex a)
    {
        NumberUtils.CheckNotNull(a);
        return a.IsZero() ? (a.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Add(a.Real), Imaginary.Add(a.Imaginary));
    }

    public /*override*/ Complex Divide(Complex divisor)
    {
        NumberUtils.CheckNotNull(divisor);
        if (divisor.IsOne())
            return divisor.IsNumeric() ? GetNumericValue() : this;
        if (divisor.IsNaN() || IsNaN())
            return ComplexNaN;

        Real c = divisor.Real;
        Real d = divisor.Imaginary;

        if (c.Abs().CompareTo(d.Abs()) < 0)
        {
            Real q = c.Divide(d);
            Real denominator = c.Multiply(q).Add(d);
            return new Complex((Real.Multiply(q).Add(Imaginary)).Divide(denominator),
                    (Imaginary.Multiply(q).Subtract(Real)).Divide(denominator));
        }
        else
        {
            Real q = d.Divide(c);
            Real denominator = d.Multiply(q).Add(c);
            return new Complex((Imaginary.Multiply(q).Add(Real)).Divide(denominator),
                    (Imaginary.Subtract(Real.Multiply(q))).Divide(denominator));
        }
    }

    public /*override*/ IField<Complex> GetField()
    {
        return ComplexField.GetInstance();
    }

    public /*override*/ Complex Multiply(int n)
    {
        return n == 1 ? this : new Complex(Real.Multiply(n), Imaginary.Multiply(n));
    }

    public /*override*/ Complex Multiply(Complex factor)
    {
        NumberUtils.CheckNotNull(factor);
        if (factor.IsNaN())
            return ComplexNaN;
        return new Complex(Real.Multiply(factor.Real).Subtract(Imaginary.Multiply(factor.Imaginary)),
                Real.Multiply(factor.Imaginary).Add(Imaginary.Multiply(factor.Real)));
    }

    public /*override*/ Complex Negate()
    {
        return new Complex(Real.Negate(), Imaginary.Negate());
    }

    public /*override*/ Complex Reciprocal()
    {
        if (IsNaN())
            return ComplexNaN;
        if (Real.Abs().CompareTo(Imaginary.Abs()) < 0)
        {
            Real q = Real.Divide(Imaginary);
            Real scale = (Real.Multiply(q).Add(Imaginary)).Reciprocal();
            return new Complex(scale.Multiply(q), scale.Negate());
        }
        else
        {
            Real q = Imaginary.Divide(Real);
            Real scale = Imaginary.Multiply(q).Add(Real).Reciprocal();
            return new Complex(scale, scale.Multiply(q).Negate());
        }
    }

    public IField<Complex> Field => throw new NotImplementedException();

    public /*override*/ Complex Subtract(Complex a)
    {
        NumberUtils.CheckNotNull(a);
        return a.IsZero() ? (a.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Subtract(a.Real), Imaginary.Subtract(a.Imaginary));
    }

    public /*override*/ Complex GetNumericValue()
    {
        return IsNumeric() ? this : new Complex(Real.GetNumericValue(), Imaginary.GetNumericValue());
    }

    public /*override*/ Complex Abs()
    {
        if (IsZero() || IsOne() || IsInfinite() || IsNaN())
            return this;
        Real abs2 = Real.Multiply(Real).Add(Imaginary.Multiply(Imaginary));
        if (IsNumeric())
            return new Complex(abs2.Pow(0.5));
        Rational abs2r = (Rational) abs2;
        BigInteger num = abs2r.GetNumerator();
        BigInteger den = abs2r.GetDenominator();

        BigInteger nR = NumberUtils.Sqrt(num);
        if (!NumberUtils.IsSqrt(num, nR))
            throw new InvalidOperationException();

        BigInteger dR = NumberUtils.Sqrt(den);
        if (!NumberUtils.IsSqrt(den, dR))
            throw new InvalidOperationException();

        return new Complex(new Rational(nR, dR));
    }

    public double AbsNumeric()
    {
        if (IsNaN())
            return double.NaN;

        if (IsInfinite())
            return double.PositiveInfinity;

        double real = Real.ToDouble();
        double imaginary = Imaginary.ToDouble();

        return AbsNumeric(real, imaginary);
    }

    public static double AbsNumeric(double real, double imaginary)
    {
        if (Math.Abs(real) < Math.Abs(imaginary))
        {
            if (imaginary == 0.0)
            {
                return Math.Abs(real);
            }
            double q = real / imaginary;
            return Math.Abs(imaginary) * Math.Sqrt(1 + q * q);
        }
        else
        {
            if (real == 0.0)
            {
                return Math.Abs(imaginary);
            }
            double q = imaginary / real;
            return Math.Abs(real) * Math.Sqrt(1 + q * q);
        }
    }

    public /*override*/ Complex Add(Rationals.Rational fraction)
    {
        return fraction.CompareTo(Rationals.Rational.Zero) == 0 ? this : new Complex(Real.Add(fraction), Imaginary);
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

    public bool IsNegativeInteger() => Imaginary.IsZero() && Real.IsInteger() && Math.Sign(Real.DoubleValue()) < 0;
}