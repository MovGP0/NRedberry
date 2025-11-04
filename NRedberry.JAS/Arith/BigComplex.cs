using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// BigComplex class based on BigRational implementing the RingElem respectively
/// the StarRingElem interface. Objects of this class are immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.BigComplex
/// </remarks>
public sealed class BigComplex : GcdRingElem<BigComplex>, RingFactory<BigComplex>, ICloneable
{
    /// <summary>
    /// Real part of the data structure.
    /// </summary>
    public readonly BigRational Re;

    /// <summary>
    /// Imaginary part of the data structure.
    /// </summary>
    public readonly BigRational Im;

    private static readonly Random random = new();

    /// <summary>
    /// The constant 0.
    /// </summary>
    public static readonly BigComplex ZERO = new();

    /// <summary>
    /// The constant 1.
    /// </summary>
    public static readonly BigComplex ONE = new(BigRational.One);

    /// <summary>
    /// The constant i.
    /// </summary>
    public static readonly BigComplex I = new(BigRational.Zero, BigRational.One);

    /// <summary>
    /// The constructor creates a BigComplex object from two BigRational objects
    /// real and imaginary part.
    /// </summary>
    /// <param name="r">real part.</param>
    /// <param name="i">imaginary part.</param>
    public BigComplex(BigRational r, BigRational i)
    {
        Re = r;
        Im = i;
    }

    /// <summary>
    /// The constructor creates a BigComplex object from a BigRational object as
    /// real part, the imaginary part is set to 0.
    /// </summary>
    /// <param name="r">real part.</param>
    public BigComplex(BigRational r)
        : this(r, BigRational.Zero)
    {
    }

    /// <summary>
    /// The constructor creates a BigComplex object with real part 0 and
    /// imaginary part 0.
    /// </summary>
    public BigComplex()
        : this(BigRational.Zero)
    {
    }

    /// <summary>
    /// Get the corresponding element factory.
    /// </summary>
    /// <returns>factory for this Element.</returns>
    public BigComplex Factory()
    {
        return this;
    }

    /// <summary>
    /// Get a list of the generating elements.
    /// </summary>
    /// <returns>list of generators for the algebraic structure.</returns>
    public List<BigComplex> Generators()
    {
        return [One, Imag];
    }

    /// <summary>
    /// Is this structure finite or infinite.
    /// </summary>
    /// <returns>true if this structure is finite, else false.</returns>
    public bool IsFinite()
    {
        return false;
    }

    /// <summary>
    /// Clone this.
    /// </summary>
    public BigComplex Clone() => new(Re, Im);

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Copy BigComplex element c.
    /// </summary>
    /// <param name="c">BigComplex.</param>
    /// <returns>a copy of c.</returns>
    public static BigComplex Clone(BigComplex c) => new(c.Re, c.Im);

    /// <summary>
    /// Get the zero element.
    /// </summary>
    /// <returns>0 as BigComplex.</returns>
    public BigComplex Zero => ZERO;

    /// <summary>
    /// Get the one element.
    /// </summary>
    /// <returns>1 as BigComplex.</returns>
    public BigComplex One => ONE;

    /// <summary>
    /// Get the i element.
    /// </summary>
    /// <returns>i as BigComplex.</returns>
    public BigComplex Imag => I;

    /// <summary>
    /// Query if this ring is commutative.
    /// </summary>
    /// <returns>true.</returns>
    public bool IsCommutative()
    {
        return true;
    }

    /// <summary>
    /// Query if this ring is associative.
    /// </summary>
    /// <returns>true.</returns>
    public bool IsAssociative() => true;

    /// <summary>
    /// Query if this ring is a field.
    /// </summary>
    /// <returns>true.</returns>
    public bool IsField() => true;

    /// <summary>
    /// Characteristic of this ring.
    /// </summary>
    /// <returns>characteristic of this ring.</returns>
    public System.Numerics.BigInteger Characteristic() => System.Numerics.BigInteger.Zero;

    /// <summary>
    /// Get a BigComplex element from a BigInteger.
    /// </summary>
    /// <param name="a">BigInteger.</param>
    /// <returns>a BigComplex.</returns>
    public BigComplex FromInteger(BigInteger a) => new(new BigRational(a));

    /// <summary>
    /// Get a BigComplex element from a long.
    /// </summary>
    /// <param name="a">long.</param>
    /// <returns>a BigComplex.</returns>
    public BigComplex FromInteger(long a) => new(new BigRational(a));

    /// <summary>
    /// Get the String representation.
    /// </summary>
    public override string ToString()
    {
        string s = string.Empty + Re;
        int i = Im.CompareTo(BigRational.Zero);
        if (i == 0)
            return s;
        s += "i" + Im;
        return s;
    }

    /// <summary>
    /// Is Complex number zero.
    /// </summary>
    /// <returns>If this is 0 then true is returned, else false.</returns>
    public bool IsZero() => Re.Equals(BigRational.Zero) && Im.Equals(BigRational.Zero);

    /// <summary>
    /// Is Complex number one.
    /// </summary>
    /// <returns>If this is 1 then true is returned, else false.</returns>
    public bool IsOne() => Re.Equals(BigRational.One) && Im.Equals(BigRational.Zero);

    /// <summary>
    /// Is Complex unit element.
    /// </summary>
    /// <returns>If this is a unit then true is returned, else false.</returns>
    public bool IsUnit() => !IsZero();

    /// <summary>
    /// Comparison with any other object.
    /// </summary>
    public override bool Equals(object? b)
    {
        if (b is not BigComplex bc)
        {
            return false;
        }

        return Re.Equals(bc.Re) && Im.Equals(bc.Im);
    }

    /// <summary>
    /// Hash code for this BigComplex.
    /// </summary>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Re);
        hashCode.Add(Im);
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Since complex numbers are unordered, we use lexicographical order of re
    /// and im.
    /// </summary>
    /// <returns>0 if this is equal to b; 1 if re > b.re, or re == b.re and im >
    /// b.im; -1 if re &lt; b.re, or re == b.re and im &lt; b.im</returns>
    public int CompareTo(BigComplex? b)
    {
        if (b is null)
            return 1;
        int s = Re.CompareTo(b.Re);
        if (s != 0)
        {
            return s;
        }

        return Im.CompareTo(b.Im);
    }

    /// <summary>
    /// Since complex numbers are unordered, we use lexicographical order of re
    /// and im.
    /// </summary>
    /// <returns>0 if this is equal to 0; 1 if re > 0, or re == 0 and im > 0; -1
    /// if re &lt; 0, or re == 0 and im &lt; 0</returns>
    public int Signum()
    {
        int s = Re.Signum();
        if (s != 0)
        {
            return s;
        }

        return Im.Signum();
    }

    /// <summary>
    /// Complex number summation.
    /// </summary>
    /// <param name="B">a BigComplex number.</param>
    /// <returns>this+B.</returns>
    public BigComplex Sum(BigComplex B)
    {
        return new BigComplex(Re.Sum(B.Re), Im.Sum(B.Im));
    }

    /// <summary>
    /// Complex number subtract.
    /// </summary>
    /// <param name="S">a BigComplex number.</param>
    /// <returns>this-S.</returns>
    public BigComplex Subtract(BigComplex S)
    {
        return new BigComplex(Re.Subtract(S.Re), Im.Subtract(S.Im));
    }

    /// <summary>
    /// Complex number negative.
    /// </summary>
    /// <returns>-this.</returns>
    public BigComplex Negate()
    {
        return new BigComplex(Re.Negate(), Im.Negate());
    }

    /// <summary>
    /// Complex number conjugate.
    /// </summary>
    /// <returns>the complex conjugate of this.</returns>
    public BigComplex Conjugate()
    {
        return new BigComplex(Re, Im.Negate());
    }

    /// <summary>
    /// Complex number norm.
    /// </summary>
    /// <returns>||this||.</returns>
    public BigComplex Norm()
    {
        BigRational v = Re.Multiply(Re);
        v = v.Sum(Im.Multiply(Im));
        return new BigComplex(v);
    }

    /// <summary>
    /// Complex number absolute value.
    /// </summary>
    /// <returns>|this|^2. Note: The square root is not yet implemented.</returns>
    public BigComplex Abs() => Norm();

    /// <summary>
    /// Complex number product.
    /// </summary>
    /// <param name="B">is a complex number.</param>
    /// <returns>this*B.</returns>
    public BigComplex Multiply(BigComplex B)
    {
        return new BigComplex(
            Re.Multiply(B.Re).Subtract(Im.Multiply(B.Im)),
            Re.Multiply(B.Im).Sum(Im.Multiply(B.Re)));
    }

    /// <summary>
    /// Complex number inverse.
    /// </summary>
    /// <returns>S with S*this = 1.</returns>
    public BigComplex Inverse()
    {
        BigRational a = Norm().Re.Inverse();
        return new BigComplex(Re.Multiply(a), Im.Multiply(a.Negate()));
    }

    /// <summary>
    /// Complex number remainder.
    /// </summary>
    /// <param name="S">is a complex number.</param>
    /// <returns>0.</returns>
    public BigComplex Remainder(BigComplex S)
    {
        if (S.IsZero())
        {
            throw new ArithmeticException("division by zero");
        }

        return ZERO;
    }

    /// <summary>
    /// Complex number divide.
    /// </summary>
    /// <param name="B">is a complex number, non-zero.</param>
    /// <returns>this/B.</returns>
    public BigComplex Divide(BigComplex B)
    {
        return Multiply(B.Inverse());
    }

    /// <summary>
    /// Complex number, random. Random rational numbers A and B are generated
    /// using random(n). Then R is the complex number with real part A and
    /// imaginary part B.
    /// </summary>
    /// <param name="n">such that 0 ≤ A, B ≤ (2^n-1).</param>
    /// <returns>R.</returns>
    public BigComplex Random(int n)
    {
        return Random(n, random);
    }

    /// <summary>
    /// Complex number, random. Random rational numbers A and B are generated
    /// using random(n). Then R is the complex number with real part A and
    /// imaginary part B.
    /// </summary>
    /// <param name="n">such that 0 ≤ A, B ≤ (2^n-1).</param>
    /// <param name="rnd">is a source for random bits.</param>
    /// <returns>R.</returns>
    public BigComplex Random(int n, Random rnd)
    {
        BigRational r = BigRational.One.Random(n, rnd);
        BigRational i = BigRational.One.Random(n, rnd);
        return new BigComplex(r, i);
    }

    /// <summary>
    /// Complex number greatest common divisor.
    /// </summary>
    /// <param name="S">BigComplex.</param>
    /// <returns>gcd(this, S).</returns>
    public BigComplex Gcd(BigComplex S)
    {
        if (S.IsZero())
        {
            return this;
        }

        if (IsZero())
        {
            return S;
        }

        return ONE;
    }

    /// <summary>
    /// BigComplex extended greatest common divisor.
    /// </summary>
    /// <param name="S">BigComplex.</param>
    /// <returns>[ gcd(this,S), a, b ] with a*this + b*S = gcd(this,S).</returns>
    public BigComplex[] Egcd(BigComplex? S)
    {
        BigComplex[] ret = new BigComplex[3];
        ret[0] = null!;
        ret[1] = null!;
        ret[2] = null!;
        if (S.IsZero())
        {
            ret[0] = this;
            return ret;
        }

        if (IsZero())
        {
            ret[0] = S;
            return ret;
        }

        BigComplex half = new BigComplex(new BigRational(1, 2));
        ret[0] = ONE;
        ret[1] = Inverse().Multiply(half);
        ret[2] = S.Inverse().Multiply(half);
        return ret;
    }

    BigComplex ElemFactory<BigComplex>.FromInteger(System.Numerics.BigInteger a)
    {
        return FromInteger(new BigInteger(a));
    }

    ElemFactory<BigComplex> Element<BigComplex>.Factory() => Factory();
}
