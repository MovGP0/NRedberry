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
public class AlgebraicNumber<C> : GcdRingElem<AlgebraicNumber<C>>, ICloneable, IEquatable<AlgebraicNumber<C>>
    where C : RingElem<C>
{
    private int _isUnit = -1;

    public AlgebraicNumberRing<C> Ring { get; }

    public GenPolynomial<C> Val { get; }

    /// <summary>
    /// Creates an algebraic number from the specified ring context and polynomial representative.
    /// </summary>
    /// <param name="ring">Ring that owns this algebraic number.</param>
    /// <param name="value">Polynomial to be reduced modulo the ring's modulus.</param>
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

    /// <summary>
    /// Creates the additive identity algebraic number for the provided ring.
    /// </summary>
    /// <param name="ring">Ring that owns this algebraic number.</param>
    public AlgebraicNumber(AlgebraicNumberRing<C> ring)
        : this(ring, GenPolynomialRing<C>.Zero)
    {
    }

    /// <summary>
    /// Gets the polynomial representative stored in this algebraic number.
    /// </summary>
    /// <returns>Polynomial value reduced modulo the modulus.</returns>
    public GenPolynomial<C> GetVal()
    {
        return Val;
    }

    /// <summary>
    /// Returns the owning algebraic number ring factory.
    /// </summary>
    /// <returns>The ring that created this number.</returns>
    public AlgebraicNumberRing<C> Factory()
    {
        return Ring;
    }

    /// <summary>
    /// Returns an identical algebraic number instance.
    /// </summary>
    /// <returns>A new object sharing the same ring and polynomial.</returns>
    public AlgebraicNumber<C> Clone()
    {
        return new AlgebraicNumber<C>(Ring, Val);
    }

    public AlgebraicNumber<C> Copy() => Clone();

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Determines whether this element represents zero.
    /// </summary>
    public bool IsZero()
    {
        return Val.IsZero();
    }

    /// <summary>
    /// Determines whether this element represents one.
    /// </summary>
    public bool IsOne()
    {
        return Val.IsOne();
    }

    /// <summary>
    /// Determines whether this element is a unit (invertible) within its ring.
    /// </summary>
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

    /// <summary>
    /// Formats the algebraic number using the ring variables (if available).
    /// </summary>
    public override string ToString()
    {
        string[]? variables = Ring.Ring.GetVars();
        return Val.ToString(variables ?? []);
    }

    /// <summary>
    /// Compares two algebraic numbers first by their modulus and then by their polynomial value.
    /// </summary>
    /// <param name="other">Other algebraic number.</param>
    /// <returns>Sign of the comparison.</returns>
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

    /// <summary>
    /// Determines reference and value equality within the same ring context.
    /// </summary>
    public bool Equals(AlgebraicNumber<C>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        if (!Ring.Equals(other.Ring))
        {
            return false;
        }

        return CompareTo(other) == 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is AlgebraicNumber<C> other && Equals(other);
    }

    /// <summary>
    /// Hashes the polynomial and ring components.
    /// </summary>
    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Val);
        hashCode.Add(Ring);
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Returns the absolute value of the embedded polynomial.
    /// </summary>
    public AlgebraicNumber<C> Abs()
    {
        return new AlgebraicNumber<C>(Ring, Val.Abs());
    }

    public static bool operator ==(AlgebraicNumber<C>? left, AlgebraicNumber<C>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(AlgebraicNumber<C>? left, AlgebraicNumber<C>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the negation of this algebraic number.
    /// </summary>
    public AlgebraicNumber<C> Negate()
    {
        return new AlgebraicNumber<C>(Ring, Val.Negate());
    }

    /// <summary>
    /// Signum of the polynomial part.
    /// </summary>
    public int Signum()
    {
        return Val.Signum();
    }

    /// <summary>
    /// Subtracts another algebraic number.
    /// </summary>
    /// <param name="other">Operand.</param>
    /// <returns>Result of subtraction.</returns>
    public AlgebraicNumber<C> Subtract(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return new AlgebraicNumber<C>(Ring, Val.Subtract(other.Val));
    }

    /// <summary>
    /// Adds a polynomial to this algebraic number.
    /// </summary>
    /// <param name="value">Polynomial to add.</param>
    public AlgebraicNumber<C> Sum(GenPolynomial<C> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new AlgebraicNumber<C>(Ring, Val.Sum(value));
    }

    /// <summary>
    /// Adds a coefficient to this algebraic number.
    /// </summary>
    /// <param name="value">Coefficient to add.</param>
    public AlgebraicNumber<C> Sum(C value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new AlgebraicNumber<C>(Ring, Val.Sum(value));
    }

    /// <summary>
    /// Divides by another algebraic number (throws if not invertible).
    /// </summary>
    /// <param name="other">Divisor.</param>
    public AlgebraicNumber<C> Divide(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Multiply(other.Inverse());
    }

    /// <summary>
    /// Computes the multiplicative inverse using polynomial modular inversion.
    /// </summary>
    /// <exception cref="AlgebraicNotInvertibleException">Thrown when the modulus shares a non-unit gcd.</exception>
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

    /// <summary>
    /// Computes the remainder when dividing by another algebraic number.
    /// </summary>
    /// <param name="other">Divisor.</param>
    /// <returns>Polynomial remainder.</returns>
    public AlgebraicNumber<C> Remainder(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.IsZero())
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

    /// <summary>
    /// Computes the greatest common divisor with another algebraic number.
    /// </summary>
    /// <param name="other">Other operand.</param>
    /// <returns>GCD result.</returns>
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

    /// <summary>
    /// Extended GCD algorithm returning [g, s, t] such that s*this + t*other = g.
    /// </summary>
    /// <param name="other">Other operand.</param>
    public AlgebraicNumber<C>[] Egcd(AlgebraicNumber<C> other)
    {
        AlgebraicNumber<C>[] result = new AlgebraicNumber<C>[3];

        if (other.IsZero())
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
            GenPolynomial<C>[] quotientRemainder = q.QuotientRemainder(r);
            GenPolynomial<C> quotient = quotientRemainder[0];

            GenPolynomial<C> x1 = c1.Subtract(quotient.Multiply(d1));
            GenPolynomial<C> x2 = c2.Subtract(quotient.Multiply(d2));

            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;
            q = r;
            r = quotientRemainder[1];
        }

        result[0] = new AlgebraicNumber<C>(Ring, q);
        result[1] = new AlgebraicNumber<C>(Ring, c1);
        result[2] = new AlgebraicNumber<C>(Ring, c2);
        return result;
    }

    /// <summary>
    /// Multiplies two algebraic numbers.
    /// </summary>
    /// <param name="other">Operand to multiply.</param>
    public AlgebraicNumber<C> Multiply(AlgebraicNumber<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        GenPolynomial<C> product = Val.Multiply(other.Val);
        return new AlgebraicNumber<C>(Ring, product);
    }

    /// <summary>
    /// Multiplies this algebraic number by a coefficient.
    /// </summary>
    /// <param name="value">Coefficient multiplier.</param>
    public AlgebraicNumber<C> Multiply(C value)
    {
        ArgumentNullException.ThrowIfNull(value);
        GenPolynomial<C> product = Val.Multiply(value);
        return new AlgebraicNumber<C>(Ring, product);
    }

    /// <summary>
    /// Multiplies this algebraic number by a polynomial.
    /// </summary>
    /// <param name="value">Polynomial multiplier.</param>
    public AlgebraicNumber<C> Multiply(GenPolynomial<C> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        GenPolynomial<C> product = Val.Multiply(value);
        return new AlgebraicNumber<C>(Ring, product);
    }

    /// <summary>
    /// Returns the monic version of this algebraic number.
    /// </summary>
    public AlgebraicNumber<C> Monic()
    {
        return new AlgebraicNumber<C>(Ring, Val.Monic());
    }

    /// <summary>
    /// Adds two algebraic numbers.
    /// </summary>
    /// <param name="other">Operand to add.</param>
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
