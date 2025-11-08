using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// BigInteger class to make System.Numerics.BigInteger available with RingElem respectively the GcdRingElem interface.
/// Objects of this class are immutable. The SAC2 static methods are also provided.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.BigInteger
/// </remarks>
public sealed class BigInteger : GcdRingElem<BigInteger>, RingFactory<BigInteger>, IEnumerable<BigInteger>, ICloneable
{
    /// <summary>
    /// The data structure.
    /// </summary>
    public readonly System.Numerics.BigInteger Val;

    private static readonly Random random = new();
    private bool nonNegative = true;

    /// <summary>
    /// The constant 0.
    /// </summary>
    public static readonly BigInteger Zero = new (System.Numerics.BigInteger.Zero);

    /// <summary>
    /// The constant 1.
    /// </summary>
    public static readonly BigInteger One = new (System.Numerics.BigInteger.One);

    public static BigInteger Parse(string s) => new(System.Numerics.BigInteger.Parse(s));

    /// <summary>
    /// Constructor for BigInteger from math.BigInteger.
    /// </summary>
    /// <param name="a">System.Numerics.BigInteger.</param>
    public BigInteger(System.Numerics.BigInteger a) => Val = a;

    public BigInteger(BigInteger a) => Val = a.Val;

    /// <summary>
    /// Constructor for BigInteger from long.
    /// </summary>
    /// <param name="a">long.</param>
    public BigInteger(long a)
    {
        Val = new System.Numerics.BigInteger(a);
    }

    /// <summary>
    /// Constructor for BigInteger from String.
    /// </summary>
    /// <param name="s">String.</param>
    public BigInteger(string s)
    {
        Val = System.Numerics.BigInteger.Parse(s.Trim());
    }

    /// <summary>
    /// Constructor for BigInteger without parameters.
    /// </summary>
    public BigInteger()
    {
        Val = System.Numerics.BigInteger.Zero;
    }

    /// <summary>
    /// Get the value.
    /// </summary>
    /// <returns>val System.Numerics.BigInteger.</returns>
    public System.Numerics.BigInteger GetVal() => Val;

    /// <summary>
    /// Get the value as long.
    /// </summary>
    /// <returns>val as long.</returns>
    public long LongValue() => (long)Val;

    /// <summary>
    /// Get the corresponding element factory.
    /// </summary>
    /// <returns>factory for this Element.</returns>
    public BigInteger Factory() => this;

    /// <summary>
    /// Get a list of the generating elements.
    /// </summary>
    /// <returns>list of generators for the algebraic structure.</returns>
    public List<BigInteger> Generators()
    {
        return [One];
    }

    /// <summary>
    /// Is this structure finite or infinite.
    /// </summary>
    /// <returns>true if this structure is finite, else false.</returns>
    public bool IsFinite()
    {
        return false;
    }

    public BigInteger Clone() => new(Val);

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Copy BigInteger element c.
    /// </summary>
    /// <param name="c">BigInteger.</param>
    /// <returns>a copy of c.</returns>
    public static BigInteger Clone(BigInteger c) => new(c.Val);

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
    public bool IsAssociative()
    {
        return true;
    }

    /// <summary>
    /// Query if this ring is a field.
    /// </summary>
    /// <returns>false.</returns>
    public bool IsField()
    {
        return false;
    }

    /// <summary>
    /// Characteristic of this ring.
    /// </summary>
    /// <returns>characteristic of this ring.</returns>
    public System.Numerics.BigInteger Characteristic()
    {
        return System.Numerics.BigInteger.Zero;
    }

    /// <summary>
    /// Get a BigInteger element from a math.BigInteger.
    /// </summary>
    /// <param name="a">math.BigInteger.</param>
    /// <returns>a as BigInteger.</returns>
    public BigInteger FromInteger(System.Numerics.BigInteger a)
    {
        return new BigInteger(a);
    }

    /// <summary>
    /// Get a BigInteger element from a math.BigInteger.
    /// </summary>
    /// <param name="a">math.BigInteger.</param>
    /// <returns>a as BigInteger.</returns>
    public static BigInteger ValueOf(System.Numerics.BigInteger a)
    {
        return new BigInteger(a);
    }

    /// <summary>
    /// Get a BigInteger element from long.
    /// </summary>
    /// <param name="a">long.</param>
    /// <returns>a as BigInteger.</returns>
    public BigInteger FromInteger(long a)
    {
        return new BigInteger(a);
    }

    /// <summary>
    /// Get a BigInteger element from long.
    /// </summary>
    /// <param name="a">long.</param>
    /// <returns>a as BigInteger.</returns>
    public static BigInteger ValueOf(long a)
    {
        return new BigInteger(a);
    }

    /// <summary>
    /// Is BigInteger number zero.
    /// </summary>
    /// <returns>If this is 0 then true is returned, else false.</returns>
    public bool IsZero()
    {
        return Val.Equals(System.Numerics.BigInteger.Zero);
    }

    /// <summary>
    /// Is BigInteger number one.
    /// </summary>
    public bool IsOne()
    {
        return Val.Equals(System.Numerics.BigInteger.One);
    }

    /// <summary>
    /// Is BigInteger number unit.
    /// </summary>
    public bool IsUnit()
    {
        return IsOne() || Negate().IsOne();
    }

    /// <summary>
    /// Get the String representation.
    /// </summary>
    public override string ToString()
    {
        return Val.ToString();
    }

    /// <summary>
    /// Compare to BigInteger b.
    /// </summary>
    /// <param name="b">BigInteger.</param>
    /// <returns>0 if this == b, 1 if this > b, -1 if this &lt; b.</returns>
    public int CompareTo(BigInteger? b)
    {
        if (b == null)
            return 1;
        return Val.CompareTo(b.Val);
    }

    /// <summary>
    /// Comparison with any other object.
    /// </summary>
    public override bool Equals(object? b)
    {
        if (b is not BigInteger bi)
        {
            return false;
        }

        return Val.Equals(bi.Val);
    }

    /// <summary>
    /// Hash code for this BigInteger.
    /// </summary>
    public override int GetHashCode()
    {
        return Val.GetHashCode();
    }

    public static BigInteger operator +(BigInteger a, BigInteger b) => new(a.Val + b.Val);
    public static BigInteger operator -(BigInteger a, BigInteger b) => new(a.Val - b.Val);
    public static BigInteger operator *(BigInteger a, BigInteger b) => new(a.Val * b.Val);
    public static BigInteger operator /(BigInteger a, BigInteger b) => new(a.Val / b.Val);
    public static BigInteger operator %(BigInteger a, BigInteger b) => new(a.Val % b.Val);

    public static implicit operator BigInteger(System.Numerics.BigInteger val) => new(val);
    public static explicit operator System.Numerics.BigInteger(BigInteger val) => val.Val;

    /// <summary>
    /// Absolute value of this.
    /// </summary>
    public BigInteger Abs()
    {
        return new BigInteger(System.Numerics.BigInteger.Abs(Val));
    }

    public static BigInteger Abs(BigInteger s) => s.Abs();

    /// <summary>
    /// Negative value of this.
    /// </summary>
    public BigInteger Negate()
    {
        return new BigInteger(System.Numerics.BigInteger.Negate(Val));
    }

    public static BigInteger Negate(BigInteger s) => s.Negate();

    /// <summary>
    /// signum.
    /// </summary>
    public int Signum()
    {
        return Val.Sign;
    }

    public int Sign() => Signum();

    /// <summary>
    /// BigInteger subtract.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>this-S.</returns>
    public BigInteger Subtract(BigInteger S)
    {
        return new BigInteger(Val - S.Val);
    }

    /// <summary>
    /// BigInteger divide.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>this/S.</returns>
    public BigInteger Divide(BigInteger S)
    {
        return new BigInteger(Val / S.Val);
    }

    /// <summary>
    /// Integer inverse. R is a non-zero integer. S=1/R if defined else 0.
    /// </summary>
    public BigInteger Inverse()
    {
        if (IsOne() || Negate().IsOne())
        {
            return this;
        }

        return Zero;
    }

    /// <summary>
    /// BigInteger remainder.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>this - (this/S)*S.</returns>
    public BigInteger Remainder(BigInteger S)
    {
        return new BigInteger(Val % S.Val);
    }

    public BigInteger Mod(BigInteger S) => Remainder(S);
    public static BigInteger Remainder(BigInteger l, BigInteger k) => new(l.Val % k.Val);

    /// <summary>
    /// BigInteger compute quotient and remainder. Throws an exception, if S == 0.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>BigInteger[] { q, r } with this = q S + r and 0 ≤ r &lt; |S|.</returns>
    public BigInteger[] QuotientRemainder(BigInteger S)
    {
        BigInteger[] qr = new BigInteger[2];
        System.Numerics.BigInteger quotient = System.Numerics.BigInteger.DivRem(Val, S.Val, out System.Numerics.BigInteger remainder);
        qr[0] = new BigInteger(quotient);
        qr[1] = new BigInteger(remainder);
        return qr;
    }

    /// <summary>
    /// BigInteger greatest common divisor.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>gcd(this, S).</returns>
    public BigInteger Gcd(BigInteger S)
    {
        return new BigInteger(System.Numerics.BigInteger.GreatestCommonDivisor(Val, S.Val));
    }

    public static BigInteger GreatestCommonDivisor(BigInteger l, BigInteger k)
        => new(System.Numerics.BigInteger.GreatestCommonDivisor(l.Val, k.Val));

    public static BigInteger Gcd(BigInteger l, BigInteger k) => GreatestCommonDivisor(l, k);

    /// <summary>
    /// BigInteger extended greatest common divisor.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>[ gcd(this,S), a, b ] with a*this + b*S = gcd(this,S).</returns>
    public BigInteger[] Egcd(BigInteger S)
    {
        BigInteger[] ret = new BigInteger[3];
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

        BigInteger[] qr;
        BigInteger q = this;
        BigInteger r = S;
        BigInteger c1 = One;
        BigInteger d1 = Zero;
        BigInteger c2 = Zero;
        BigInteger d2 = One;
        BigInteger x1;
        BigInteger x2;
        while (!r.IsZero())
        {
            qr = q.QuotientRemainder(r);
            q = qr[0];
            x1 = c1.Subtract(q.Multiply(d1));
            x2 = c2.Subtract(q.Multiply(d2));
            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;
            q = r;
            r = qr[1];
        }

        if (q.Signum() < 0)
        {
            q = q.Negate();
            c1 = c1.Negate();
            c2 = c2.Negate();
        }

        ret[0] = q;
        ret[1] = c1;
        ret[2] = c2;
        return ret;
    }

    /// <summary>
    /// BigInteger random.
    /// </summary>
    /// <param name="n">such that 0 ≤ r ≤ (2^n-1).</param>
    /// <returns>r, a random BigInteger.</returns>
    public BigInteger Random(int n)
    {
        return Random(n, random);
    }

    /// <summary>
    /// BigInteger random.
    /// </summary>
    /// <param name="n">such that 0 ≤ r ≤ (2^n-1).</param>
    /// <param name="rnd">is a source for random bits.</param>
    /// <returns>r, a random BigInteger.</returns>
    public BigInteger Random(int n, Random rnd)
    {
        byte[] bytes = new byte[(n + 7) / 8];
        rnd.NextBytes(bytes);
        System.Numerics.BigInteger r = new System.Numerics.BigInteger(bytes);
        if (rnd.Next(2) == 1)
        {
            r = System.Numerics.BigInteger.Negate(r);
        }

        return new BigInteger(r);
    }

    /// <summary>
    /// BigInteger multiply.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>this*S.</returns>
    public BigInteger Multiply(BigInteger S)
    {
        return new BigInteger(Val * S.Val);
    }

    /// <summary>
    /// BigInteger summation.
    /// </summary>
    /// <param name="S">BigInteger.</param>
    /// <returns>this+S.</returns>
    public BigInteger Sum(BigInteger S)
    {
        return new BigInteger(Val + S.Val);
    }

    /// <summary>
    /// Set the iteration algorithm to all elements.
    /// </summary>
    public void SetAllIterator()
    {
        nonNegative = false;
    }

    /// <summary>
    /// Set the iteration algorithm to non-negative elements.
    /// </summary>
    public void SetNonNegativeIterator()
    {
        nonNegative = true;
    }

    /// <summary>
    /// Get a BigInteger iterator.
    /// </summary>
    /// <returns>a iterator over all integers.</returns>
    public IEnumerator<BigInteger> GetEnumerator()
    {
        return new BigIntegerIterator(nonNegative);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    ElemFactory<BigInteger> Element<BigInteger>.Factory() => Factory();
}

/// <summary>
/// Big integer iterator.
/// </summary>
internal class BigIntegerIterator : IEnumerator<BigInteger>
{
    private System.Numerics.BigInteger curr;
    private readonly bool nonNegative;
    private BigInteger? current;

    /// <summary>
    /// BigInteger iterator constructor.
    /// </summary>
    /// <param name="nn">true for an iterator over non-negative longs, false for all elements iterator.</param>
    public BigIntegerIterator(bool nn)
    {
        curr = System.Numerics.BigInteger.Zero;
        nonNegative = nn;
    }

    public BigInteger Current => current!;

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        current = new BigInteger(curr);
        if (nonNegative)
        {
            curr += System.Numerics.BigInteger.One;
        }
        else if (curr.Sign > 0)
        {
            curr = System.Numerics.BigInteger.Negate(curr);
        }
        else
        {
            curr = System.Numerics.BigInteger.Negate(curr) + System.Numerics.BigInteger.One;
        }

        return true;
    }

    public void Reset()
    {
        curr = System.Numerics.BigInteger.Zero;
        current = null;
    }

    public void Dispose()
    {
    }
}
