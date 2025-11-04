using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// Immutable arbitrary-precision rational numbers. BigRational class based on
/// BigInteger and implementing the RingElem interface.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.BigRational
/// </remarks>
public sealed class BigRational : GcdRingElem<BigRational>, RingFactory<BigRational>, ICloneable
{
    /// <summary>
    /// Numerator part of the data structure.
    /// </summary>
    public readonly System.Numerics.BigInteger Num;

    /// <summary>
    /// Denominator part of the data structure.
    /// </summary>
    public readonly System.Numerics.BigInteger Den;

    private static readonly Random random = new();

    /// <summary>
    /// The Constant 0.
    /// </summary>
    public static readonly BigRational Zero = new(System.Numerics.BigInteger.Zero);

    /// <summary>
    /// The Constant 1.
    /// </summary>
    public static readonly BigRational One = new(System.Numerics.BigInteger.One);

    /// <summary>
    /// Constructor for a BigRational from math.BigIntegers.
    /// </summary>
    private BigRational(System.Numerics.BigInteger n, System.Numerics.BigInteger d)
    {
        Num = n;
        Den = d;
    }

    /// <summary>
    /// Constructor for a BigRational from math.BigIntegers.
    /// </summary>
    public BigRational(System.Numerics.BigInteger n)
    {
        Num = n;
        Den = System.Numerics.BigInteger.One;
    }

    /// <summary>
    /// Constructor for a BigRational from jas.arith.BigIntegers.
    /// </summary>
    public BigRational(BigInteger n)
        : this(n.GetVal())
    {
    }

    /// <summary>
    /// Constructor for a BigRational from jas.arith.BigIntegers.
    /// </summary>
    public BigRational(BigInteger n, BigInteger d)
    {
        System.Numerics.BigInteger nu = n.GetVal();
        System.Numerics.BigInteger de = d.GetVal();
        BigRational r = RNRED(nu, de);
        Num = r.Num;
        Den = r.Den;
    }

    /// <summary>
    /// Constructor for a BigRational from longs.
    /// </summary>
    public BigRational(long n, long d)
    {
        System.Numerics.BigInteger nu = new System.Numerics.BigInteger(n);
        System.Numerics.BigInteger de = new System.Numerics.BigInteger(d);
        BigRational r = RNRED(nu, de);
        Num = r.Num;
        Den = r.Den;
    }

    /// <summary>
    /// Constructor for a BigRational from longs.
    /// </summary>
    public BigRational(long n)
    {
        Num = new System.Numerics.BigInteger(n);
        Den = System.Numerics.BigInteger.One;
    }

    /// <summary>
    /// Constructor for a BigRational with no arguments.
    /// </summary>
    public BigRational()
    {
        Num = System.Numerics.BigInteger.Zero;
        Den = System.Numerics.BigInteger.One;
    }

    /// <summary>
    /// Constructor for a BigRational from String.
    /// </summary>
    public BigRational(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            Num = System.Numerics.BigInteger.Zero;
            Den = System.Numerics.BigInteger.One;
            return;
        }

        s = s.Trim();
        int i = s.IndexOf('/');
        if (i < 0)
        {
            i = s.IndexOf('.');
            if (i < 0)
            {
                Num = System.Numerics.BigInteger.Parse(s);
                Den = System.Numerics.BigInteger.One;
                return;
            }

            System.Numerics.BigInteger n;
            if (s[0] == '-')
            {
                n = System.Numerics.BigInteger.Parse(s.Substring(1, i - 1));
            }
            else
            {
                n = System.Numerics.BigInteger.Parse(s.Substring(0, i));
            }

            BigRational r = new BigRational(n);
            System.Numerics.BigInteger d = System.Numerics.BigInteger.Parse(s.Substring(i + 1));
            int j = s.Length - i - 1;
            BigRational z = new BigRational(1, 10);
            z = Power<BigRational>.PositivePower(z, j);
            BigRational f = new BigRational(d);
            f = f.Multiply(z);
            r = r.Sum(f);

            if (s[0] == '-')
            {
                Num = System.Numerics.BigInteger.Negate(r.Num);
            }
            else
            {
                Num = r.Num;
            }

            Den = r.Den;
        }
        else
        {
            System.Numerics.BigInteger n = System.Numerics.BigInteger.Parse(s.Substring(0, i));
            System.Numerics.BigInteger d = System.Numerics.BigInteger.Parse(s.Substring(i + 1));
            BigRational r = RNRED(n, d);
            Num = r.Num;
            Den = r.Den;
        }
    }

    /// <summary>
    /// Get the corresponding element factory.
    /// </summary>
    public BigRational Factory() => this;

    /// <summary>
    /// Get a list of the generating elements.
    /// </summary>
    public List<BigRational> Generators() => [One];

    /// <summary>
    /// Is this structure finite or infinite.
    /// </summary>
    public bool IsFinite() => false;

    /// <summary>
    /// Clone this.
    /// </summary>
    public BigRational Clone() => new(Num, Den);

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Copy BigRational element c.
    /// </summary>
    public static BigRational Clone(BigRational c) => new(c.Num, c.Den);

    /// <summary>
    /// Get the numerator.
    /// </summary>
    public System.Numerics.BigInteger Numerator() => Num;

    /// <summary>
    /// Get the denominator.
    /// </summary>
    public System.Numerics.BigInteger Denominator() => Den;

    /// <summary>
    /// Get the string representation.
    /// </summary>
    public override string ToString()
    {
        if (!Den.Equals(System.Numerics.BigInteger.One))
        {
            return Num + "/" + Den;
        }

        return Num.ToString();
    }

    /// <summary>
    /// Query if this ring is commutative.
    /// </summary>
    public bool IsCommutative() => true;

    /// <summary>
    /// Query if this ring is associative.
    /// </summary>
    public bool IsAssociative() => true;

    /// <summary>
    /// Query if this ring is a field.
    /// </summary>
    public bool IsField() => true;

    /// <summary>
    /// Characteristic of this ring.
    /// </summary>
    public System.Numerics.BigInteger Characteristic() => System.Numerics.BigInteger.Zero;

    /// <summary>
    /// Get a BigRational element from a math.BigInteger.
    /// </summary>
    public BigRational FromInteger(System.Numerics.BigInteger a) => new(a);

    /// <summary>
    /// Get a BigRational element from a arith.BigInteger.
    /// </summary>
    public BigRational FromInteger(BigInteger a) => new(a);

    /// <summary>
    /// Get a BigRational element from a math.BigInteger.
    /// </summary>
    public static BigRational ValueOf(System.Numerics.BigInteger a) => new(a);

    /// <summary>
    /// Get a BigRational element from a long.
    /// </summary>
    public BigRational FromInteger(long a) => new(a);

    /// <summary>
    /// Get a BigRational element from a long.
    /// </summary>
    public static BigRational ValueOf(long a) => new(a);

    /// <summary>
    /// Is BigRational zero.
    /// </summary>
    public bool IsZero() => Num.Equals(System.Numerics.BigInteger.Zero);

    /// <summary>
    /// Is BigRational one.
    /// </summary>
    public bool IsOne() => Num.Equals(Den);

    /// <summary>
    /// Is BigRational unit.
    /// </summary>
    public bool IsUnit() => !IsZero();

    /// <summary>
    /// Comparison with any other object.
    /// </summary>
    public override bool Equals(object? b)
    {
        if (b is not BigRational br)
        {
            return false;
        }

        return Num.Equals(br.Num) && Den.Equals(br.Den);
    }

    /// <summary>
    /// Hash code for this BigRational.
    /// </summary>
    public override int GetHashCode() => 37 * Num.GetHashCode() + Den.GetHashCode();

    /// <summary>
    /// Rational number reduction to lowest terms.
    /// </summary>
    /// <param name="n">BigInteger.</param>
    /// <param name="d">BigInteger.</param>
    /// <returns>a/b ~ n/d, gcd(a,b) = 1, b > 0.</returns>
    public static BigRational RNRED(System.Numerics.BigInteger n, System.Numerics.BigInteger d)
    {
        if (n.Equals(System.Numerics.BigInteger.Zero))
        {
            return new BigRational(n, System.Numerics.BigInteger.One);
        }

        System.Numerics.BigInteger C = System.Numerics.BigInteger.GreatestCommonDivisor(n, d);
        System.Numerics.BigInteger num = n / C;
        System.Numerics.BigInteger den = d / C;
        if (den.Sign < 0)
        {
            num = System.Numerics.BigInteger.Negate(num);
            den = System.Numerics.BigInteger.Negate(den);
        }

        return new BigRational(num, den);
    }

    /// <summary>
    /// Rational number reduction to lowest terms.
    /// </summary>
    public static BigRational Reduction(System.Numerics.BigInteger n, System.Numerics.BigInteger d) => RNRED(n, d);

    /// <summary>
    /// Rational number absolute value.
    /// </summary>
    public BigRational Abs() => Signum() >= 0 ? this : Negate();

    /// <summary>
    /// Rational number comparison.
    /// </summary>
    public int CompareTo(BigRational? S)
    {
        if (S == null)
            return 1;

        if (Equals(Zero))
        {
            return -S.Signum();
        }

        if (S.Equals(One))
        {
            return Signum();
        }

        int RL = Num.Sign;
        int SL = S.Num.Sign;
        int J1Y = RL - SL;
        int TL = J1Y / 2;
        if (TL != 0)
        {
            return TL;
        }

        System.Numerics.BigInteger J3Y = Num * S.Den;
        System.Numerics.BigInteger J2Y = Den * S.Num;
        return J3Y.CompareTo(J2Y);
    }

    /// <summary>
    /// Rational number difference.
    /// </summary>
    public BigRational Subtract(BigRational S) => Sum(S.Negate());

    /// <summary>
    /// Rational number inverse.
    /// </summary>
    public BigRational Inverse()
    {
        System.Numerics.BigInteger S1;
        System.Numerics.BigInteger S2;
        if (Num.Sign >= 0)
        {
            S1 = Den;
            S2 = Num;
        }
        else
        {
            S1 = System.Numerics.BigInteger.Negate(Den);
            S2 = System.Numerics.BigInteger.Negate(Num);
        }

        return new BigRational(S1, S2);
    }

    /// <summary>
    /// Rational number negative.
    /// </summary>
    public BigRational Negate() => new(System.Numerics.BigInteger.Negate(Num), Den);

    /// <summary>
    /// Rational number product.
    /// </summary>
    public BigRational Multiply(BigRational S)
    {
        if (Equals(Zero) || S.Equals(Zero))
        {
            return Zero;
        }

        System.Numerics.BigInteger R1 = Num;
        System.Numerics.BigInteger R2 = Den;
        System.Numerics.BigInteger S1 = S.Num;
        System.Numerics.BigInteger S2 = S.Den;

        if (R2.Equals(System.Numerics.BigInteger.One) && S2.Equals(System.Numerics.BigInteger.One))
        {
            return new BigRational(R1 * S1, System.Numerics.BigInteger.One);
        }

        if (R2.Equals(System.Numerics.BigInteger.One))
        {
            System.Numerics.BigInteger D1 = System.Numerics.BigInteger.GreatestCommonDivisor(R1, S2);
            System.Numerics.BigInteger RB1 = R1 / D1;
            System.Numerics.BigInteger SB2 = S2 / D1;
            return new BigRational(RB1 * S1, SB2);
        }

        if (S2.Equals(System.Numerics.BigInteger.One))
        {
            System.Numerics.BigInteger D2 = System.Numerics.BigInteger.GreatestCommonDivisor(S1, R2);
            System.Numerics.BigInteger SB1 = S1 / D2;
            System.Numerics.BigInteger RB2 = R2 / D2;
            return new BigRational(SB1 * R1, RB2);
        }

        System.Numerics.BigInteger D1g = System.Numerics.BigInteger.GreatestCommonDivisor(R1, S2);
        System.Numerics.BigInteger RB1g = R1 / D1g;
        System.Numerics.BigInteger SB2g = S2 / D1g;
        System.Numerics.BigInteger D2g = System.Numerics.BigInteger.GreatestCommonDivisor(S1, R2);
        System.Numerics.BigInteger SB1g = S1 / D2g;
        System.Numerics.BigInteger RB2g = R2 / D2g;
        return new BigRational(RB1g * SB1g, RB2g * SB2g);
    }

    /// <summary>
    /// Rational number quotient.
    /// </summary>
    public BigRational Divide(BigRational S) => Multiply(S.Inverse());

    /// <summary>
    /// Rational number remainder.
    /// </summary>
    public BigRational Remainder(BigRational S)
    {
        if (S.IsZero())
        {
            throw new ArithmeticException("division by zero");
        }

        return Zero;
    }

    /// <summary>
    /// Rational number, random.
    /// </summary>
    public BigRational Random(int n) => Random(n, random);

    /// <summary>
    /// Rational number, random.
    /// </summary>
    public BigRational Random(int n, Random rnd)
    {
        byte[] bytesA = new byte[(n + 7) / 8];
        byte[] bytesB = new byte[(n + 7) / 8];
        rnd.NextBytes(bytesA);
        rnd.NextBytes(bytesB);

        System.Numerics.BigInteger A = new System.Numerics.BigInteger(bytesA);
        if (rnd.Next(2) == 1)
        {
            A = System.Numerics.BigInteger.Negate(A);
        }

        System.Numerics.BigInteger B = new System.Numerics.BigInteger(bytesB);
        B = System.Numerics.BigInteger.Abs(B) + System.Numerics.BigInteger.One;
        return RNRED(A, B);
    }

    /// <summary>
    /// Rational number sign.
    /// </summary>
    public int Signum() => Num.Sign;

    /// <summary>
    /// Rational number sum.
    /// </summary>
    public BigRational Sum(BigRational S)
    {
        if (Equals(Zero))
        {
            return S;
        }

        if (S.Equals(Zero))
        {
            return this;
        }

        System.Numerics.BigInteger R1 = Num;
        System.Numerics.BigInteger R2 = Den;
        System.Numerics.BigInteger S1 = S.Num;
        System.Numerics.BigInteger S2 = S.Den;

        if (R2.Equals(System.Numerics.BigInteger.One) && S2.Equals(System.Numerics.BigInteger.One))
        {
            return new BigRational(R1 + S1, System.Numerics.BigInteger.One);
        }

        if (R2.Equals(System.Numerics.BigInteger.One))
        {
            System.Numerics.BigInteger T1a = R1 * S2 + S1;
            return new BigRational(T1a, S2);
        }

        if (S2.Equals(System.Numerics.BigInteger.One))
        {
            System.Numerics.BigInteger T1b = R2 * S1 + R1;
            return new BigRational(T1b, R2);
        }

        System.Numerics.BigInteger D = System.Numerics.BigInteger.GreatestCommonDivisor(R2, S2);
        System.Numerics.BigInteger RB2 = R2 / D;
        System.Numerics.BigInteger SB2 = S2 / D;
        System.Numerics.BigInteger T1 = R1 * SB2 + RB2 * S1;

        if (T1.Equals(System.Numerics.BigInteger.Zero))
        {
            return Zero;
        }

        if (!D.Equals(System.Numerics.BigInteger.One))
        {
            System.Numerics.BigInteger E = System.Numerics.BigInteger.GreatestCommonDivisor(T1, D);
            if (!E.Equals(System.Numerics.BigInteger.One))
            {
                T1 /= E;
                R2 /= E;
            }
        }

        System.Numerics.BigInteger T2 = R2 * SB2;
        return new BigRational(T1, T2);
    }

    /// <summary>
    /// Rational number greatest common divisor.
    /// </summary>
    public BigRational Gcd(BigRational S)
    {
        if (S.IsZero())
        {
            return this;
        }

        if (IsZero())
        {
            return S;
        }

        return One;
    }

    /// <summary>
    /// BigRational extended greatest common divisor.
    /// </summary>
    public BigRational[] Egcd(BigRational S)
    {
        BigRational[] ret = new BigRational[3];
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

        BigRational half = new BigRational(1, 2);
        ret[0] = One;
        ret[1] = Inverse().Multiply(half);
        ret[2] = S.Inverse().Multiply(half);
        return ret;
    }

    ElemFactory<BigRational> Element<BigRational>.Factory() => Factory();
}
