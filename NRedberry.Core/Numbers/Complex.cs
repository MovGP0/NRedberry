using System.Numerics;
using NRedberry.Apache.Commons.Math;
using NRedberry.Indices;
using NRedberry.Tensors;
using Complex32 = System.Numerics.Complex;

namespace NRedberry.Numbers;

public sealed class Complex : Tensor, NRedberry.INumber<Complex>
{
    public static readonly Complex ComplexNaN = new(NRedberry.Numeric.NaN, NRedberry.Numeric.NaN);
    public static readonly Complex RealPositiveInfinity = new(NRedberry.Numeric.PositiveInfinity, NRedberry.Numeric.Zero);
    public static readonly Complex RealNegativeInfinity = new(NRedberry.Numeric.NegativeInfinity, NRedberry.Numeric.Zero);
    public static readonly Complex ImaginaryPositiveInfinity = new(NRedberry.Numeric.Zero, NRedberry.Numeric.PositiveInfinity);
    public static readonly Complex ImaginaryNegativeInfinity = new(NRedberry.Numeric.Zero, NRedberry.Numeric.NegativeInfinity);
    public static readonly Complex ComplexNegativeInfinity = new(NRedberry.Numeric.NegativeInfinity, NRedberry.Numeric.NegativeInfinity);
    public static readonly Complex ComplexPositiveInfinity = new(NRedberry.Numeric.PositiveInfinity, NRedberry.Numeric.PositiveInfinity);
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
    public static readonly Complex ImaginaryMinusOne = new(Rational.Zero, Rational.MinusOne);

    public Real Real { get; }

    public Real Imaginary { get; }

    public Complex32 ToComplex32()
    {
        return new Complex32((float)Real.ToDouble(), (float)Imaginary.ToDouble());
    }

    public static implicit operator Complex32(Complex c)
    {
        return new Complex32((float)c.Real.ToDouble(), (float)c.Imaginary.ToDouble());
    }

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
            Imaginary = NRedberry.Numeric.Zero;
        }
        else
        {
            Real = real;
            Imaginary = Rational.Zero;
        }
    }

    public Complex(Complex32 complex)
        : this(complex.Real, complex.Imaginary)
    {
    }

    public Complex(int real, int imaginary)
        : this(new Rational(real), new Rational(imaginary))
    {
    }

    public Complex(int real)
        : this(new Rational(real), Rational.Zero)
    {
    }

    public Complex(double real, double imaginary)
        : this(new NRedberry.Numeric(real), new NRedberry.Numeric(imaginary))
    {
    }

    public Complex(int real, double imaginary)
        : this(new NRedberry.Numeric(real), new NRedberry.Numeric(imaginary))
    {
    }

    public Complex(double real, int imaginary)
        : this(new NRedberry.Numeric(real), new NRedberry.Numeric(imaginary))
    {
    }

    public Complex(double real)
        : this(new NRedberry.Numeric(real), NRedberry.Numeric.Zero)
    {
    }

    public Complex(long real)
        : this(new Rational(real), Rational.Zero)
    {
    }

    public Complex(long real, long imaginary)
        : this(new Rational(real), new Rational(imaginary))
    {
    }

    public Complex(BigInteger real, BigInteger imaginary)
        : this(new Rational(real), new Rational(imaginary))
    {
    }

    public Complex(BigInteger real)
        : this(new Rational(real), Rational.Zero)
    {
    }

    public override Tensor this[int i]
    {
        get { throw new ArgumentOutOfRangeException(nameof(i)); }
    }

    public override Indices.Indices Indices
    {
        get { return IndicesFactory.EmptyIndices; }
    }

    public Real GetReal()
    {
        return Real;
    }

    public Real GetImaginary()
    {
        return Imaginary;
    }

    public bool IsReal()
    {
        return Imaginary.IsZero();
    }

    public bool IsImaginary()
    {
        return Real.IsZero();
    }

    public override int GetHashCode()
    {
        int hash = HashWithSign();
        unchecked
        {
            return hash * hash;
        }
    }

    /// <summary>
    /// The hash codes of opposite numbers are also opposite.
    /// </summary>
    public int HashWithSign()
    {
        unchecked
        {
            return Real.GetHashCode() + (329 * Imaginary.GetHashCode());
        }
    }

    public override int Size
    {
        get { return 0; }
    }

    public override string ToString(OutputFormat outputFormat)
    {
        if (Real.IsZero())
        {
            if (Imaginary.IsZero())
            {
                return "0";
            }

            if (Imaginary.IsOne())
            {
                return "I";
            }

            if (Imaginary.IsMinusOne())
            {
                return "-I";
            }

            return Imaginary + "*I";
        }

        int sign = Imaginary.SigNum();
        if (sign == 0)
        {
            return Real.ToString();
        }

        Real abs = Imaginary.Abs();
        if (sign < 0)
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
            {
                return "(" + ToString(mode) + ")";
            }
        }

        return ToString(mode);
    }

    public override TensorBuilder GetBuilder()
    {
        return new ComplexBuilder(this);
    }

    public override TensorFactory? GetFactory()
    {
        return null;
    }

    public Complex GetImaginaryPart()
    {
        return new Complex(Imaginary.Field.Zero, Imaginary);
    }

    public Complex GetRealPart()
    {
        return new Complex(Real);
    }

    public Complex GetImaginaryAsComplex()
    {
        if (Imaginary.IsOne())
        {
            return One;
        }

        if (Imaginary.IsZero())
        {
            return Zero;
        }

        return new Complex(Rational.Zero, Imaginary);
    }

    public Complex GetRealAsComplex()
    {
        if (Real.IsOne())
        {
            return One;
        }

        if (Real.IsZero())
        {
            return Zero;
        }

        return new Complex(Real);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        Complex other = (Complex)obj;
        return Real.Equals(other.Real) && Imaginary.Equals(other.Imaginary);
    }

    public Complex Conjugate()
    {
        if (Imaginary.IsZero())
        {
            return this;
        }

        return new Complex(Real, Imaginary.Negate());
    }

    public Complex Add(Complex a)
    {
        NumberUtils.CheckNotNull(a);
        return a.IsZero() ? (a.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Add(a.Real), Imaginary.Add(a.Imaginary));
    }

    public Complex Divide(Complex divisor)
    {
        NumberUtils.CheckNotNull(divisor);
        if (divisor.IsOne())
        {
            return divisor.IsNumeric() ? GetNumericValue() : this;
        }

        if (divisor.IsNaN() || IsNaN())
        {
            return ComplexNaN;
        }

        Real c = divisor.Real;
        Real d = divisor.Imaginary;

        if (c.Abs().CompareTo(d.Abs()) < 0)
        {
            Real q = c.Divide(d);
            Real denominator = c.Multiply(q).Add(d);
            return new Complex(
                (Real.Multiply(q).Add(Imaginary)).Divide(denominator),
                (Imaginary.Multiply(q).Subtract(Real)).Divide(denominator));
        }

        Real q2 = d.Divide(c);
        Real denominator2 = d.Multiply(q2).Add(c);
        return new Complex(
            (Imaginary.Multiply(q2).Add(Real)).Divide(denominator2),
            (Imaginary.Subtract(Real.Multiply(q2))).Divide(denominator2));
    }

    public IField<Complex> GetField()
    {
        return ComplexField.GetInstance();
    }

    public Complex Multiply(int n)
    {
        return n == 1 ? this : new Complex(Real.Multiply(n), Imaginary.Multiply(n));
    }

    public Complex Multiply(Complex factor)
    {
        NumberUtils.CheckNotNull(factor);
        if (factor.IsNaN())
        {
            return ComplexNaN;
        }

        return new Complex(
            Real.Multiply(factor.Real).Subtract(Imaginary.Multiply(factor.Imaginary)),
            Real.Multiply(factor.Imaginary).Add(Imaginary.Multiply(factor.Real)));
    }

    public Complex Negate()
    {
        return new Complex(Real.Negate(), Imaginary.Negate());
    }

    public Complex Reciprocal()
    {
        if (IsNaN())
        {
            return ComplexNaN;
        }

        if (Real.Abs().CompareTo(Imaginary.Abs()) < 0)
        {
            Real q = Real.Divide(Imaginary);
            Real scale = (Real.Multiply(q).Add(Imaginary)).Reciprocal();
            return new Complex(scale.Multiply(q), scale.Negate());
        }

        Real q2 = Imaginary.Divide(Real);
        Real scale2 = Imaginary.Multiply(q2).Add(Real).Reciprocal();
        return new Complex(scale2, scale2.Multiply(q2).Negate());
    }

    public IField<Complex> Field
    {
        get { return ComplexField.GetInstance(); }
    }

    public Complex Subtract(Complex a)
    {
        NumberUtils.CheckNotNull(a);
        return a.IsZero() ? (a.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Subtract(a.Real), Imaginary.Subtract(a.Imaginary));
    }

    public Complex GetNumericValue()
    {
        return IsNumeric() ? this : new Complex(Real.GetNumericValue(), Imaginary.GetNumericValue());
    }

    public Complex Abs()
    {
        if (IsZero() || IsOne() || IsInfinite() || IsNaN())
        {
            return this;
        }

        Real abs2 = Real.Multiply(Real).Add(Imaginary.Multiply(Imaginary));
        if (IsNumeric())
        {
            return new Complex(abs2.Pow(0.5));
        }

        Rational abs2r = (Rational)abs2;
        BigInteger num = abs2r.Numerator;
        BigInteger den = abs2r.Denominator;

        BigInteger nR = NumberUtils.Sqrt(num);
        if (!NumberUtils.IsSqrt(num, nR))
        {
            throw new InvalidOperationException();
        }

        BigInteger dR = NumberUtils.Sqrt(den);
        if (!NumberUtils.IsSqrt(den, dR))
        {
            throw new InvalidOperationException();
        }

        return new Complex(new Rational(nR, dR));
    }

    public double AbsNumeric()
    {
        if (IsNaN())
        {
            return double.NaN;
        }

        if (IsInfinite())
        {
            return double.PositiveInfinity;
        }

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

        if (real == 0.0)
        {
            return Math.Abs(imaginary);
        }

        double q2 = imaginary / real;
        return Math.Abs(real) * Math.Sqrt(1 + q2 * q2);
    }

    public Complex Add(Rationals.Rational fraction)
    {
        return fraction.CompareTo(Rationals.Rational.Zero) == 0 ? this : new Complex(Real.Add(fraction), Imaginary);
    }

    public int IntValue()
    {
        return Real.IntValue();
    }

    public long LongValue()
    {
        return Real.LongValue();
    }

    public double DoubleValue()
    {
        return Real.DoubleValue();
    }

    public float FloatValue()
    {
        return Real.FloatValue();
    }

    public Complex Add(double bg)
    {
        return bg == 0.0 ? GetNumericValue() : new Complex(Real.Add(bg), Imaginary);
    }

    public Complex Add(int i)
    {
        return i == 0 ? this : new Complex(Real.Add(i), Imaginary);
    }

    public Complex Add(long l)
    {
        return l == 0 ? this : new Complex(Real.Add(l), Imaginary);
    }

    public Complex Add(BigInteger bg)
    {
        NumberUtils.CheckNotNull(bg);
        return bg.Equals(BigInteger.Zero) ? this : new Complex(Real.Add(bg), Imaginary);
    }

    public Complex Add(Rational fraction)
    {
        return fraction.CompareTo(Rational.Zero) == 0 ? this : new Complex(Real.Add(fraction), Imaginary);
    }

    public Complex Subtract(double bg)
    {
        return bg == 0.0 ? GetNumericValue() : new Complex(Real.Subtract(bg), Imaginary);
    }

    public Complex Subtract(int i)
    {
        return i == 0 ? this : new Complex(Real.Subtract(i), Imaginary);
    }

    public Complex Subtract(long l)
    {
        return l == 0 ? this : new Complex(Real.Subtract(l), Imaginary);
    }

    public Complex Subtract(BigInteger bg)
    {
        NumberUtils.CheckNotNull(bg);
        return bg.Equals(BigInteger.Zero) ? this : new Complex(Real.Subtract(bg), Imaginary);
    }

    public Complex Subtract(Rational fraction)
    {
        return fraction.CompareTo(Rational.Zero) == 0 ? this : new Complex(Real.Subtract(fraction), Imaginary);
    }

    public Complex Divide(double d)
    {
        return d == 1.0 ? this : new Complex(Real.Divide(d), Imaginary.Divide(d));
    }

    public Complex Divide(int i)
    {
        return i == 1 ? this : new Complex(Real.Divide(i), Imaginary.Divide(i));
    }

    public Complex Divide(long l)
    {
        return l == 1 ? this : new Complex(Real.Divide(l), Imaginary.Divide(l));
    }

    public Complex Divide(BigInteger bg)
    {
        return bg.CompareTo(BigInteger.One) == 0 ? this : new Complex(Real.Divide(bg), Imaginary.Divide(bg));
    }

    public Complex Divide(Rational fraction)
    {
        return fraction.CompareTo(Rational.One) == 0 ? this : new Complex(Real.Divide(fraction), Imaginary.Divide(fraction));
    }

    public Complex Multiply(double d)
    {
        return d == 1.0 ? GetNumericValue() : double.IsNaN(d) ? ComplexNaN : new Complex(Real.Multiply(d), Imaginary.Multiply(d));
    }

    public Complex Multiply(long l)
    {
        return l == 1 ? this : new Complex(Real.Multiply(l), Imaginary.Multiply(l));
    }

    public Complex Multiply(BigInteger bg)
    {
        return bg.CompareTo(BigInteger.One) == 0 ? this : new Complex(Real.Multiply(bg), Imaginary.Multiply(bg));
    }

    public Complex Multiply(Rational fraction)
    {
        return fraction.CompareTo(Rational.One) == 0 ? this : new Complex(Real.Multiply(fraction), Imaginary.Multiply(fraction));
    }

    public Complex Add(Real d)
    {
        NumberUtils.CheckNotNull(d);
        return d.IsZero() ? (d.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Add(d), Imaginary.Add(d));
    }

    public Complex Subtract(Real d)
    {
        NumberUtils.CheckNotNull(d);
        return d.IsZero() ? (d.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Subtract(d), Imaginary.Add(d));
    }

    public Complex Multiply(Real d)
    {
        NumberUtils.CheckNotNull(d);
        return d.IsOne() ? (d.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Multiply(d), Imaginary.Multiply(d));
    }

    public Complex Divide(Real d)
    {
        NumberUtils.CheckNotNull(d);
        return d.IsOne() ? (d.IsNumeric() ? GetNumericValue() : this) : new Complex(Real.Divide(d), Imaginary.Divide(d));
    }

    public Complex LogNumeric()
    {
        if (IsNaN())
        {
            return ComplexNaN;
        }

        double real = Real.ToDouble();
        double imaginary = Imaginary.ToDouble();

        return new Complex(Math.Log(AbsNumeric(real, imaginary)), Math.Atan2(imaginary, real));
    }

    public Complex Pow(double exponent)
    {
        return LogNumeric().Multiply(exponent).ExpNumeric();
    }

    public Complex Pow(BigInteger exponent)
    {
        if (exponent.CompareTo(BigInteger.Zero) < 0)
        {
            return Reciprocal().Pow(BigInteger.Negate(exponent));
        }

        Complex result = One;
        Complex k2p = this;
        while (!BigInteger.Zero.Equals(exponent))
        {
            if ((exponent & BigInteger.One) != BigInteger.Zero)
            {
                result = result.Multiply(k2p);
            }

            k2p = k2p.Multiply(k2p);
            exponent >>= 1;
        }

        return result;
    }

    public Complex Pow(long exponent)
    {
        if (exponent < 0)
        {
            return Reciprocal().Pow(-exponent);
        }

        Complex result = One;
        Complex k2p = this;
        while (exponent != 0)
        {
            if (exponent != 0)
            {
                result = result.Multiply(k2p);
            }

            k2p = k2p.Multiply(k2p);
            exponent >>= 1;
        }

        return result;
    }

    public Complex Pow(int exponent)
    {
        if (exponent < 0)
        {
            return Reciprocal().Pow(-exponent);
        }

        Complex result = One;
        Complex k2p = this;
        while (exponent != 0)
        {
            if ((exponent & 0x1) != 0)
            {
                result = result.Multiply(k2p);
            }

            k2p = k2p.Multiply(k2p);
            exponent >>= 1;
        }

        return result;
    }

    public Complex ExpNumeric()
    {
        if (IsNaN())
        {
            return ComplexNaN;
        }

        double real = Real.ToDouble();
        double imaginary = Imaginary.ToDouble();

        double expReal = Math.Exp(real);
        return new Complex(expReal * Math.Cos(imaginary), expReal * Math.Sin(imaginary));
    }

    public Complex PowNumeric(Complex exponent)
    {
        return LogNumeric().Multiply(exponent).ExpNumeric();
    }

    public bool IsInfinite()
    {
        return Real.IsInfinite() || Imaginary.IsInfinite();
    }

    public bool IsNaN()
    {
        return Real.IsNaN() || Imaginary.IsNaN();
    }

    public bool IsZero()
    {
        return Real.IsZero() && Imaginary.IsZero();
    }

    public bool IsOne()
    {
        return Real.IsOne() && Imaginary.IsZero();
    }

    public bool IsMinusOne()
    {
        return Real.IsMinusOne() && Imaginary.IsZero();
    }

    public bool IsOneOrMinusOne()
    {
        return Imaginary.IsZero() && (Real.IsOne() || Real.IsMinusOne());
    }

    public bool IsImaginaryOneOrImaginaryMinusOne()
    {
        return Real.IsZero() && (Imaginary.IsOne() || Imaginary.IsMinusOne());
    }

    public bool IsNumeric()
    {
        return Real.IsNumeric();
    }

    public bool IsInteger()
    {
        return Imaginary.IsZero() && Real.IsInteger();
    }

    public bool IsNatural()
    {
        return Imaginary.IsZero() && Real.IsNatural();
    }

    public bool IsNegativeInteger()
    {
        return Imaginary.IsZero() && Real.IsInteger() && Math.Sign(Real.DoubleValue()) < 0;
    }

    public bool IsPositiveNatural()
    {
        return Imaginary.IsZero() && Real.IsInteger() && Math.Sign(Real.DoubleValue()) >= 0;
    }
}
