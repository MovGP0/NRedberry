using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// ModLong class with RingElem interface. Objects of this class are immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModLong
/// </remarks>
public sealed class ModLong : GcdRingElem<ModLong>, Modular
{
    /// <summary>
    /// ModLongRing reference.
    /// </summary>
    public readonly ModLongRing Ring;

    /// <summary>
    /// Value part of the element data structure.
    /// </summary>
    public readonly long Val;

    /// <summary>
    /// The constructor creates a ModLong object from a ModLongRing and a value part.
    /// </summary>
    /// <param name="m">ModLongRing.</param>
    /// <param name="a">System.Numerics.BigInteger.</param>
    public ModLong(ModLongRing m, System.Numerics.BigInteger a)
        : this(m, (long)(a % new System.Numerics.BigInteger(m.Modul)))
    {
    }

    /// <summary>
    /// The constructor creates a ModLong object from a ModLongRing and a long value part.
    /// </summary>
    /// <param name="m">ModLongRing.</param>
    /// <param name="a">long.</param>
    public ModLong(ModLongRing m, long a)
    {
        Ring = m;
        long v = a % Ring.Modul;
        Val = v >= 0L ? v : v + Ring.Modul;
    }

    /// <summary>
    /// Get the value part.
    /// </summary>
    /// <returns>val.</returns>
    public long GetVal() => Val;

    /// <summary>
    /// Get the corresponding element factory.
    /// </summary>
    /// <returns>factory for this Element.</returns>
    public ModLongRing Factory() => Ring;

    /// <summary>
    /// Return a symmetric BigInteger from this Element.
    /// </summary>
    /// <returns>a symmetric BigInteger of this.</returns>
    public BigInteger GetSymmetricInteger()
    {
        long v = Val;
        if ((Val + Val) > Ring.Modul)
        {
            v = Val - Ring.Modul;
        }
        return new BigInteger(v);
    }

    /// <summary>
    /// Clone this.
    /// </summary>
    public ModLong Clone() => new(Ring, Val);

    /// <summary>
    /// Is ModLong number zero.
    /// </summary>
    /// <returns>If this is 0 then true is returned, else false.</returns>
    public bool IsZero() => Val == 0L;

    /// <summary>
    /// Is ModLong number one.
    /// </summary>
    /// <returns>If this is 1 then true is returned, else false.</returns>
    public bool IsOne() => Val == 1L;

    /// <summary>
    /// Is ModLong number a unit.
    /// </summary>
    /// <returns>If this is a unit then true is returned, else false.</returns>
    public bool IsUnit()
    {
        if (IsZero())
        {
            return false;
        }
        if (Ring.IsField())
        {
            return true;
        }
        long g = Gcd(Ring.Modul, Val);
        return g == 1L || g == -1L;
    }

    /// <summary>
    /// Get the String representation.
    /// </summary>
    public override string ToString() => Val.ToString();

    /// <summary>
    /// ModLong comparison.
    /// </summary>
    /// <param name="b">ModLong.</param>
    /// <returns>sign(this-b).</returns>
    public int CompareTo(ModLong? b)
    {
        if (b == null)
            return 1;
        long v = b.Val;
        if (Ring != b.Ring)
        {
            v = v % Ring.Modul;
        }
        if (Val > v)
        {
            return 1;
        }
        return Val < v ? -1 : 0;
    }

    /// <summary>
    /// Comparison with any other object.
    /// </summary>
    public override bool Equals(object? b)
    {
        if (b is not ModLong ml)
        {
            return false;
        }
        return CompareTo(ml) == 0;
    }

    /// <summary>
    /// Hash code for this ModLong.
    /// </summary>
    public override int GetHashCode() => (int)Val;

    /// <summary>
    /// ModLong absolute value.
    /// </summary>
    /// <returns>the absolute value of this.</returns>
    public ModLong Abs() => new(Ring, Val < 0 ? -Val : Val);

    /// <summary>
    /// ModLong negative.
    /// </summary>
    /// <returns>-this.</returns>
    public ModLong Negate() => new(Ring, -Val);

    /// <summary>
    /// ModLong signum.
    /// </summary>
    /// <returns>signum(this).</returns>
    public int Signum()
    {
        if (Val > 0L)
        {
            return 1;
        }
        return Val < 0L ? -1 : 0;
    }

    /// <summary>
    /// ModLong subtraction.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>this-S.</returns>
    public ModLong Subtract(ModLong S) => new(Ring, Val - S.Val);

    /// <summary>
    /// ModLong divide.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>this/S.</returns>
    public ModLong Divide(ModLong S)
    {
        try
        {
            return Multiply(S.Inverse());
        }
        catch (NotInvertibleException e)
        {
            try
            {
                if ((Val % S.Val) == 0L)
                {
                    return new ModLong(Ring, Val / S.Val);
                }
                throw new NotInvertibleException(e.Message, e.InnerException);
            }
            catch (ArithmeticException a)
            {
                throw new NotInvertibleException(a.Message, a.InnerException);
            }
        }
    }

    /// <summary>
    /// ModLong inverse.
    /// </summary>
    /// <returns>S with S=1/this if defined.</returns>
    /// <exception cref="NotInvertibleException">if the element is not invertible.</exception>
    public ModLong Inverse()
    {
        try
        {
            return new ModLong(Ring, ModInverse(Val, Ring.Modul));
        }
        catch (ArithmeticException e)
        {
            long g = Gcd(Val, Ring.Modul);
            long f = Ring.Modul / g;
            throw new ModularNotInvertibleException(e, Ring.Modul, g, f);
        }
    }

    /// <summary>
    /// ModLong remainder.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>remainder(this, S).</returns>
    public ModLong Remainder(ModLong S)
    {
        if (S == null || S.IsZero())
        {
            throw new ArithmeticException("division by zero");
        }
        if (S.IsOne())
        {
            return Ring.Zero;
        }
        if (S.IsUnit())
        {
            return Ring.Zero;
        }
        return new ModLong(Ring, Val % S.Val);
    }

    /// <summary>
    /// ModLong multiply.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>this*S.</returns>
    public ModLong Multiply(ModLong S) => new(Ring, Val * S.Val);

    /// <summary>
    /// ModLong summation.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>this+S.</returns>
    public ModLong Sum(ModLong S) => new(Ring, Val + S.Val);

    /// <summary>
    /// ModLong greatest common divisor.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>gcd(this, S).</returns>
    public ModLong Gcd(ModLong S)
    {
        if (S.IsZero())
        {
            return this;
        }
        if (IsZero())
        {
            return S;
        }
        if (IsUnit() || S.IsUnit())
        {
            return Ring.One;
        }
        return new ModLong(Ring, Gcd(Val, S.Val));
    }

    /// <summary>
    /// ModLong extended greatest common divisor.
    /// </summary>
    /// <param name="S">ModLong.</param>
    /// <returns>[ gcd(this,S), a, b ] with a*this + b*S = gcd(this,S).</returns>
    public ModLong[] Egcd(ModLong S)
    {
        ModLong[] ret = new ModLong[3];
        ret[0] = null!;
        ret[1] = null!;
        ret[2] = null!;
        if (S == null || S.IsZero())
        {
            ret[0] = this;
            return ret;
        }
        if (IsZero())
        {
            ret[0] = S;
            return ret;
        }
        if (IsUnit() || S.IsUnit())
        {
            ret[0] = Ring.One;
            if (IsUnit() && S.IsUnit())
            {
                ret[1] = Ring.One;
                ModLong x = ret[0].Subtract(ret[1].Multiply(this));
                ret[2] = x.Divide(S);
                return ret;
            }
            if (IsUnit())
            {
                ret[1] = Inverse();
                ret[2] = Ring.Zero;
                return ret;
            }
            ret[1] = Ring.Zero;
            ret[2] = S.Inverse();
            return ret;
        }
        long q = Val;
        long r = S.Val;
        long c1 = 1L;
        long d1 = 0L;
        long c2 = 0L;
        long d2 = 1L;
        long x1;
        long x2;
        while (r != 0L)
        {
            long a = q / r;
            long b = q % r;
            q = a;
            x1 = c1 - q * d1;
            x2 = c2 - q * d2;
            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;
            q = r;
            r = b;
        }
        ret[0] = new ModLong(Ring, q);
        ret[1] = new ModLong(Ring, c1);
        ret[2] = new ModLong(Ring, c2);
        return ret;
    }

    /// <summary>
    /// Long greatest common divisor.
    /// </summary>
    /// <param name="T">long.</param>
    /// <param name="S">long.</param>
    /// <returns>gcd(T, S).</returns>
    public long Gcd(long T, long S)
    {
        if (S == 0L)
        {
            return T;
        }
        if (T == 0L)
        {
            return S;
        }
        long a = T;
        long b = S;
        while (b != 0L)
        {
            long r = a % b;
            a = b;
            b = r;
        }
        return a;
    }

    /// <summary>
    /// Long half extended greatest common divisor.
    /// </summary>
    /// <param name="T">long.</param>
    /// <param name="S">long.</param>
    /// <returns>[ gcd(T,S), a ] with a*T + b*S = gcd(T,S).</returns>
    public long[] Hegcd(long T, long S)
    {
        long[] ret = new long[2];
        if (S == 0L)
        {
            ret[0] = T;
            ret[1] = 1L;
            return ret;
        }
        if (T == 0L)
        {
            ret[0] = S;
            ret[1] = 0L;
            return ret;
        }
        long a = T;
        long b = S;
        long a1 = 1L;
        long b1 = 0L;
        while (b != 0L)
        {
            long q = a / b;
            long r = a % b;
            a = b;
            b = r;
            long r1 = a1 - q * b1;
            a1 = b1;
            b1 = r1;
        }
        if (a1 < 0L)
        {
            a1 += S;
        }
        ret[0] = a;
        ret[1] = a1;
        return ret;
    }

    /// <summary>
    /// Long modular inverse.
    /// </summary>
    /// <param name="T">long.</param>
    /// <param name="m">long.</param>
    /// <returns>a with a*T = 1 mod m.</returns>
    public long ModInverse(long T, long m)
    {
        if (T == 0L)
        {
            throw new NotInvertibleException("zero is not invertible");
        }
        long[] hegcd = Hegcd(T, m);
        long a = hegcd[0];
        if (!(a == 1L || a == -1L))
        {
            throw new ModularNotInvertibleException("element not invertible, gcd != 1", m, a, m / a);
        }
        long b = hegcd[1];
        if (b == 0L)
        {
            throw new NotInvertibleException("element not invertible, divisible by modul");
        }
        if (b < 0L)
        {
            b += m;
        }
        return b;
    }

    ElemFactory<ModLong> Element<ModLong>.Factory() => Factory();
}
