using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// ModInteger class with GcdRingElem interface. Objects of this class are immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModInteger
/// </remarks>
public sealed class ModInteger : GcdRingElem<ModInteger>, Modular
{
    /// <summary>
    /// ModIntegerRing reference.
    /// </summary>
    public readonly ModIntegerRing Ring;

    /// <summary>
    /// Value part of the element data structure.
    /// </summary>
    public readonly BigInteger Val;

    /// <summary>
    /// The constructor creates a ModInteger object from a ModIntegerRing and a value part.
    /// </summary>
    public ModInteger(ModIntegerRing m, BigInteger a)
    {
        ArgumentNullException.ThrowIfNull(m);
        ArgumentNullException.ThrowIfNull(a);
        Ring = m;
        Val = Normalize(a, Ring.Modul);
    }

    /// <summary>
    /// The constructor creates a ModInteger object from a ModIntegerRing and a long value part.
    /// </summary>
    public ModInteger(ModIntegerRing m, long a)
        : this(m, new BigInteger(a))
    {
    }

    /// <summary>
    /// The constructor creates a ModInteger object from a ModIntegerRing and a String value part.
    /// </summary>
    public ModInteger(ModIntegerRing m, string s)
        : this(m, BigInteger.Parse(s.Trim()))
    {
    }

    /// <summary>
    /// The constructor creates a 0 ModInteger object from a given ModIntegerRing.
    /// </summary>
    public ModInteger(ModIntegerRing m)
        : this(m, BigInteger.Zero)
    {
    }

    /// <summary>
    /// Get the value part.
    /// </summary>
    public BigInteger GetVal() => Val;

    /// <summary>
    /// Get the corresponding element factory.
    /// </summary>
    public ModIntegerRing Factory() => Ring;

    /// <summary>
    /// Return a symmetric BigInteger from this Element.
    /// </summary>
    public BigInteger GetSymmetricInteger()
    {
        BigInteger v = Val;
        if ((Val + Val).CompareTo(Ring.Modul) > 0)
        {
            v = Val - Ring.Modul;
        }

        return new BigInteger(v);
    }

    /// <summary>
    /// Clone this.
    /// </summary>
    public ModInteger Clone() => new(Ring, Val);

    /// <summary>
    /// Is ModInteger number zero.
    /// </summary>
    public bool IsZero() => Val.Equals(BigInteger.Zero);

    /// <summary>
    /// Is ModInteger number one.
    /// </summary>
    public bool IsOne() => Val.Equals(BigInteger.One);

    /// <summary>
    /// Is ModInteger number a unit.
    /// </summary>
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

        BigInteger g = BigInteger.Abs(
            BigInteger.GreatestCommonDivisor(Ring.Modul, new(Val)));
        return g.Equals(BigInteger.One);
    }

    /// <summary>
    /// Get the String representation.
    /// </summary>
    public override string ToString() => Val.ToString();

    /// <summary>
    /// ModInteger comparison.
    /// </summary>
    public int CompareTo(ModInteger? b)
    {
        if (b == null)
            return 1;
        BigInteger v = b.Val;
        if (Ring != b.Ring)
        {
            v = Normalize(v, Ring.Modul);
        }

        return Val.CompareTo(v);
    }

    /// <summary>
    /// Comparison with any other object.
    /// </summary>
    public override bool Equals(object? b)
    {
        if (b is not ModInteger mi)
        {
            return false;
        }

        return CompareTo(mi) == 0;
    }

    /// <summary>
    /// Hash code for this ModInteger.
    /// </summary>
    public override int GetHashCode() => Val.GetHashCode();

    /// <summary>
    /// ModInteger absolute value.
    /// </summary>
    public ModInteger Abs() => new(Ring, BigInteger.Abs(Val));

    /// <summary>
    /// ModInteger negative.
    /// </summary>
    public ModInteger Negate() => new(Ring, BigInteger.Negate(Val));

    /// <summary>
    /// ModInteger signum.
    /// </summary>
    public int Signum() => Val.Sign();

    /// <summary>
    /// ModInteger subtraction.
    /// </summary>
    public ModInteger Subtract(ModInteger S)
    {
        ArgumentNullException.ThrowIfNull(S);
        return new ModInteger(Ring, Val - S.Val);
    }

    /// <summary>
    /// ModInteger divide.
    /// </summary>
    public ModInteger Divide(ModInteger S)
    {
        ArgumentNullException.ThrowIfNull(S);
        try
        {
            return Multiply(S.Inverse());
        }
        catch (NotInvertibleException e)
        {
            try
            {
                BigInteger remainder = Val.Remainder(S.Val);
                if (remainder.IsZero())
                {
                    return new ModInteger(Ring, Val / S.Val);
                }

                throw new NotInvertibleException(e.Message, e);
            }
            catch (ArithmeticException a)
            {
                throw new NotInvertibleException(a.Message, a);
            }
        }
    }

    /// <summary>
    /// ModInteger inverse.
    /// </summary>
    public ModInteger Inverse()
    {
        try
        {
            BigInteger inv = ModInverse(Val, Ring.Modul);
            return new ModInteger(Ring, inv);
        }
        catch (ArithmeticException e)
        {
            BigInteger g = BigInteger.GreatestCommonDivisor(Val, Ring.Modul);
            BigInteger f = Ring.Modul / g;
            throw new ModularNotInvertibleException(e, new BigInteger(Ring.Modul), g, f);
        }
    }

    /// <summary>
    /// ModInteger remainder.
    /// </summary>
    public ModInteger Remainder(ModInteger S)
    {
        ArgumentNullException.ThrowIfNull(S);
        if (S.IsZero())
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

        BigInteger remainder = BigInteger.Remainder(Val, S.Val);
        return new ModInteger(Ring, remainder);
    }

    /// <summary>
    /// ModInteger multiply.
    /// </summary>
    public ModInteger Multiply(ModInteger S)
    {
        ArgumentNullException.ThrowIfNull(S);
        return new ModInteger(Ring, Val * S.Val);
    }

    /// <summary>
    /// ModInteger summation.
    /// </summary>
    public ModInteger Sum(ModInteger S)
    {
        ArgumentNullException.ThrowIfNull(S);
        return new ModInteger(Ring, Val + S.Val);
    }

    /// <summary>
    /// ModInteger greatest common divisor.
    /// </summary>
    public ModInteger Gcd(ModInteger S)
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

        return new ModInteger(Ring, BigInteger.GreatestCommonDivisor(Val, S.Val));
    }

    /// <summary>
    /// ModInteger extended greatest common divisor.
    /// </summary>
    public ModInteger[] Egcd(ModInteger S)
    {
        ModInteger[] ret = new ModInteger[3];
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

        if (IsUnit() || S.IsUnit())
        {
            ret[0] = Ring.One;
            if (IsUnit() && S.IsUnit())
            {
                ret[1] = Ring.One;
                ModInteger x = ret[0].Subtract(ret[1].Multiply(this));
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

        BigInteger q = Val;
        BigInteger r = S.Val;
        BigInteger c1 = BigInteger.One;
        BigInteger d1 = BigInteger.Zero;
        BigInteger c2 = BigInteger.Zero;
        BigInteger d2 = BigInteger.One;

        while (!r.IsZero())
        {
            BigInteger a = q / r;
            BigInteger b = q % r;

            BigInteger x1 = c1 - a * d1;
            BigInteger x2 = c2 - a * d2;

            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;

            q = r;
            r = b;
        }

        ret[0] = new ModInteger(Ring, q);
        ret[1] = new ModInteger(Ring, c1);
        ret[2] = new ModInteger(Ring, c2);
        return ret;
    }

    /// <summary>
    /// BigInteger modular inverse.
    /// </summary>
    private static BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        if (m.IsZero())
        {
            throw new NotInvertibleException("modulus zero is not invertible");
        }

        BigInteger value = Normalize(a, m);
        if (value.IsZero())
        {
            throw new NotInvertibleException("zero is not invertible");
        }

        BigInteger x;
        BigInteger y;
        BigInteger g = Gcd(value, m, out x, out y);

        if (!g.Equals(BigInteger.One))
        {
            throw new NotInvertibleException("numbers are not relatively prime");
        }

        BigInteger result = x % m;
        if (result.Sign() < 0)
        {
            result += m;
        }

        return result;
    }

    private static BigInteger Gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (a.IsZero())
        {
            x = BigInteger.Zero;
            y = BigInteger.One;
            return b;
        }

        BigInteger g = Gcd(b % a, a, out var x1, out var y1);

        x = y1 - (b / a) * x1;
        y = x1;

        return g;
    }

    private static BigInteger Normalize(BigInteger value, BigInteger modulus)
    {
        if (modulus.IsZero())
        {
            throw new ArgumentException("modulus must be non-zero", nameof(modulus));
        }

        BigInteger positiveModulus = modulus.Sign() < 0 ? BigInteger.Abs(modulus) : modulus;
        BigInteger remainder = value % positiveModulus;
        if (remainder.Sign() < 0)
        {
            remainder += positiveModulus;
        }

        return remainder;
    }

    ElemFactory<ModInteger> Element<ModInteger>.Factory() => Factory();
}
