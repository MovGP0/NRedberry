using System.Numerics;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Power helper that implements binary exponentiation for <see cref="RingElem{C}"/> and <see cref="MonoidElem{C}"/>,
/// including positive exponent routines, factory-aware power methods, and modular exponentiation/backward-compatible conveniences.
/// </summary>
/// <typeparam name="C">ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.Power
/// </remarks>
public class Power<C>(RingFactory<C>? fac)
    where C : RingElem<C>
{
    public Power()
        : this(null)
    {
    }

    public static C PositivePower(C a, long n)
    {
        ArgumentNullException.ThrowIfNull(a);
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), n, "only positive n allowed");
        }

        if (a.IsZero() || a.IsOne())
        {
            return a;
        }

        C baseValue = a;
        long exponent = n - 1;
        C result = baseValue;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                result = result.Multiply(baseValue);
            }

            exponent >>= 1;
            if (exponent > 0)
            {
                baseValue = baseValue.Multiply(baseValue);
            }
        }

        return result;
    }

    public static C PositivePower(C a, BigInteger n)
    {
        ArgumentNullException.ThrowIfNull(a);
        if (n.Sign <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), n, "only positive n allowed");
        }

        if (a.IsZero() || a.IsOne())
        {
            return a;
        }

        C baseValue = a;
        if (n == BigInteger.One)
        {
            return baseValue;
        }

        C result = a;
        BigInteger exponent = n - BigInteger.One;
        while (exponent.Sign > 0)
        {
            if (!exponent.IsEven)
            {
                result = result.Multiply(baseValue);
            }

            exponent >>= 1;
            if (exponent.Sign > 0)
            {
                baseValue = baseValue.Multiply(baseValue);
            }
        }

        return result;
    }

    public static C PowerMethod(RingFactory<C>? fac, C a, long n)
    {
        ArgumentNullException.ThrowIfNull(a);
        if (a.IsZero())
        {
            return a;
        }

        return PowerMethod<C>(fac, a, n);
    }

    public static TMonoid PowerMethod<TMonoid>(MonoidFactory<TMonoid>? fac, TMonoid a, long n) where TMonoid : MonoidElem<TMonoid>
    {
        ArgumentNullException.ThrowIfNull(a);

        if (n == 0)
        {
            if (fac == null)
            {
                throw new ArgumentException("fac may not be null for a^0", nameof(fac));
            }

            return MonoidFactory<TMonoid>.One;
        }

        if (a.IsOne())
        {
            return a;
        }

        TMonoid baseValue = a;
        long exponent = n;
        if (exponent < 0)
        {
            baseValue = a.Inverse();
            exponent = -exponent;
        }

        if (exponent == 1)
        {
            return baseValue;
        }

        if (fac == null)
        {
            throw new ArgumentNullException(nameof(fac), "fac may not be null");
        }

        TMonoid result = MonoidFactory<TMonoid>.One;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                result = result.Multiply(baseValue);
            }

            exponent >>= 1;
            if (exponent > 0)
            {
                baseValue = baseValue.Multiply(baseValue);
            }
        }

        return result;
    }

    public static TMonoid ModPower<TMonoid>(
        MonoidFactory<TMonoid>? fac,
        TMonoid a,
        BigInteger n,
        TMonoid m)
        where TMonoid : MonoidElem<TMonoid>
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(m);

        if (n.IsZero)
        {
            if (fac == null)
            {
                throw new ArgumentException("fac may not be null for a^0", nameof(fac));
            }

            return MonoidFactory<TMonoid>.One;
        }

        if (a.IsOne())
        {
            return a;
        }

        TMonoid baseValue = a.Remainder(m);
        BigInteger exponent = n;
        if (exponent.Sign < 0)
        {
            baseValue = a.Inverse().Remainder(m);
            exponent = BigInteger.Negate(exponent);
        }

        if (exponent == BigInteger.One)
        {
            return baseValue;
        }

        if (fac == null)
        {
            throw new ArgumentNullException(nameof(fac), "fac may not be null");
        }

        TMonoid result = MonoidFactory<TMonoid>.One;
        while (exponent.Sign > 0)
        {
            if (!exponent.IsEven)
            {
                result = result.Multiply(baseValue).Remainder(m);
            }

            exponent >>= 1;
            if (exponent.Sign > 0)
            {
                baseValue = baseValue.Multiply(baseValue).Remainder(m);
            }
        }

        return result;
    }

    public C PowerMethod(C a, long n)
    {
        return PowerMethod(fac, a, n);
    }

    public C ModPower(C a, BigInteger n, C m)
    {
        return ModPower(fac, a, n, m);
    }

    public static long Logarithm(C p, C a)
    {
        ArgumentNullException.ThrowIfNull(p);
        ArgumentNullException.ThrowIfNull(a);

        long k = 1;
        C current = p;
        while (current.CompareTo(a) < 0)
        {
            current = current.Multiply(p);
            k++;
        }

        return k;
    }

    public static C Multiply(RingFactory<C> fac, List<C> A)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return Multiply<C>(fac, A);
    }

    public static TMonoid Multiply<TMonoid>(
        MonoidFactory<TMonoid>? fac,
        List<TMonoid>? A)
        where TMonoid : MonoidElem<TMonoid>
    {
        if (fac == null)
        {
            throw new ArgumentNullException(nameof(fac), "fac may not be null for empty list");
        }

        TMonoid result = MonoidFactory<TMonoid>.One;
        if (A == null || A.Count == 0)
        {
            return result;
        }

        foreach (TMonoid element in A)
        {
            result = result.Multiply(element);
        }

        return result;
    }

    public static C Sum(RingFactory<C> fac, List<C> A)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return Sum<C>(fac, A);
    }

    public static TAbelian Sum<TAbelian>(AbelianGroupFactory<TAbelian>? fac, List<TAbelian>? A)
        where TAbelian : AbelianGroupElem<TAbelian>
    {
        if (fac == null)
        {
            throw new ArgumentNullException(nameof(fac), "fac may not be null for empty list");
        }

        TAbelian result = AbelianGroupFactory<TAbelian>.Zero;
        if (A == null || A.Count == 0)
        {
            return result;
        }

        foreach (TAbelian element in A)
        {
            result = result.Sum(element);
        }

        return result;
    }
}
