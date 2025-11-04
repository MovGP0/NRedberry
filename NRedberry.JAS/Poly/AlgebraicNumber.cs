using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Algebraic number class based on <see cref="GenPolynomial{C}"/> with <see cref="RingElem{T}"/> interface.
/// Objects of this class are immutable.
/// </summary>
/// <typeparam name="C">Coefficient type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.AlgebraicNumber
/// </remarks>
public class AlgebraicNumber<C> : GcdRingElem<AlgebraicNumber<C>> where C : RingElem<C>
{
    private int _isUnit = -1;

    public AlgebraicNumberRing<C> Ring { get; }

    public GenPolynomial<C> Val { get; }

    public AlgebraicNumber(AlgebraicNumberRing<C> ring, GenPolynomial<C> value)
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(value);

        Ring = ring;
        Val = value.Remainder(ring.Modul);

        if (Val.IsZero())
        {
            _isUnit = 0;
        }

        if (Ring.IsField())
        {
            _isUnit = 1;
        }
    }

    public AlgebraicNumber(AlgebraicNumberRing<C> ring)
        : this(ring, GenPolynomialRing<C>.Zero)
    {
    }

    public GenPolynomial<C> GetVal()
    {
        return Val;
    }

    public AlgebraicNumberRing<C> Factory()
    {
        return Ring;
    }

    public AlgebraicNumber<C> Clone()
    {
        return new AlgebraicNumber<C>(Ring, Val);
    }

    public bool IsZero()
    {
        return Val.IsZero();
    }

    public bool IsOne()
    {
        return Val.IsOne();
    }

    public bool IsUnit()
    {
        if (_isUnit > 0)
        {
            return true;
        }

        if (_isUnit == 0)
        {
            return false;
        }

        if (Val.IsZero())
        {
            _isUnit = 0;
            return false;
        }

        if (Ring.IsField())
        {
            _isUnit = 1;
            return true;
        }

        bool unit = Val.Gcd(Ring.Modul).IsUnit();
        _isUnit = unit ? 1 : 0;
        return unit;
    }

    public override string ToString()
    {
        string[]? variables = Ring.Ring.GetVars();
        return Val.ToString(variables ?? Array.Empty<string>());
    }

    public int CompareTo(AlgebraicNumber<C>? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (!ReferenceEquals(Ring.Modul, other.Ring.Modul))
        {
            int comparison = Ring.Modul.CompareTo(other.Ring.Modul);
            if (comparison != 0)
            {
                return comparison;
            }
        }

        return Val.CompareTo(other.Val);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not AlgebraicNumber<C> other)
        {
            return false;
        }

        if (!Ring.Equals(other.Ring))
        {
            return false;
        }

        return CompareTo(other) == 0;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Val);
        hashCode.Add(Ring);
        return hashCode.ToHashCode();
    }

    public AlgebraicNumber<C> Abs()
    {
        return new AlgebraicNumber<C>(Ring, Val.Abs());
    }

    public AlgebraicNumber<C> Negate()
    {
        return new AlgebraicNumber<C>(Ring, Val.Negate());
    }

    public int Signum()
    {
        return Val.Signum();
    }

    public AlgebraicNumber<C> Subtract(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return new AlgebraicNumber<C>(Ring, Val.Subtract(other.Val));
    }

    public AlgebraicNumber<C> Divide(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Multiply(other.Inverse());
    }

    public AlgebraicNumber<C> Inverse()
    {
        try
        {
            GenPolynomial<C> inverse = Val.ModInverse(Ring.Modul);
            return new AlgebraicNumber<C>(Ring, inverse);
        }
        catch (AlgebraicNotInvertibleException)
        {
            throw;
        }
        catch (NotInvertibleException ex)
        {
            GenPolynomial<C> greatestCommonDivisor = Val.Gcd(Ring.Modul);
            throw new AlgebraicNotInvertibleException(
                $"{ex}, val = {Val}, modul = {Ring.Modul}, gcd = {greatestCommonDivisor}",
                ex,
                Val,
                Ring.Modul,
                greatestCommonDivisor);
        }
    }

    public AlgebraicNumber<C> Remainder(AlgebraicNumber<C> other)
    {
        if (other is null || other.IsZero())
        {
            throw new ArithmeticException("division by zero");
        }

        if (other.IsOne())
        {
            return Ring.GetZeroElement();
        }

        if (other.IsUnit())
        {
            return Ring.GetZeroElement();
        }

        GenPolynomial<C> remainder = Val.Remainder(other.Val);
        return new AlgebraicNumber<C>(Ring, remainder);
    }

    public AlgebraicNumber<C> Gcd(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.IsZero())
        {
            return this;
        }

        if (IsZero())
        {
            return other;
        }

        if (IsUnit() || other.IsUnit())
        {
            return Ring.GetOneElement();
        }

        GenPolynomial<C> gcd = Val.Gcd(other.Val);
        return new AlgebraicNumber<C>(Ring, gcd);
    }

    public AlgebraicNumber<C>[] Egcd(AlgebraicNumber<C> other)
    {
        AlgebraicNumber<C>[] result = new AlgebraicNumber<C>[3];

        if (other is null || other.IsZero())
        {
            result[0] = this;
            return result;
        }

        if (IsZero())
        {
            result[0] = other;
            return result;
        }

        if (IsUnit() || other.IsUnit())
        {
            result[0] = Ring.GetOneElement();
            if (IsUnit() && other.IsUnit())
            {
                AlgebraicNumber<C> half = Ring.FromInteger(2).Inverse();
                result[1] = Inverse().Multiply(half);
                result[2] = other.Inverse().Multiply(half);
                return result;
            }

            if (IsUnit())
            {
                result[1] = Inverse();
                result[2] = Ring.GetZeroElement();
                return result;
            }

            result[1] = Ring.GetZeroElement();
            result[2] = other.Inverse();
            return result;
        }

        GenPolynomial<C> q = Val;
        GenPolynomial<C> r = other.Val;
        GenPolynomial<C> c1 = GenPolynomialRing<C>.One;
        GenPolynomial<C> d1 = GenPolynomialRing<C>.Zero;
        GenPolynomial<C> c2 = GenPolynomialRing<C>.Zero;
        GenPolynomial<C> d2 = GenPolynomialRing<C>.One;

        while (!r.IsZero())
        {
            GenPolynomial<C> quotient = q.Divide(r);
            GenPolynomial<C> remainder = q.Remainder(r);

            GenPolynomial<C> x1 = c1.Subtract(quotient.Multiply(d1));
            GenPolynomial<C> x2 = c2.Subtract(quotient.Multiply(d2));

            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;
            q = r;
            r = remainder;
        }

        result[0] = new AlgebraicNumber<C>(Ring, q);
        result[1] = new AlgebraicNumber<C>(Ring, c1);
        result[2] = new AlgebraicNumber<C>(Ring, c2);
        return result;
    }

    public AlgebraicNumber<C> Multiply(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        GenPolynomial<C> product = Val.Multiply(other.Val);
        return new AlgebraicNumber<C>(Ring, product);
    }

    public AlgebraicNumber<C> Sum(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        GenPolynomial<C> sum = Val.Sum(other.Val);
        return new AlgebraicNumber<C>(Ring, sum);
    }

    ElemFactory<AlgebraicNumber<C>> Element<AlgebraicNumber<C>>.Factory()
    {
        return Factory();
    }
}


