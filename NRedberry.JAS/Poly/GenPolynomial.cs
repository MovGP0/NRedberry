using System.Collections;
using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenPolynomial generic polynomials implementing RingElem. n-variate ordered polynomials over C.
/// Objects of this class are intended to be immutable. The implementation is based on SortedDictionary
/// from exponents to coefficients. Only the coefficients are modeled with generic types, the exponents
/// are fixed to ExpVector with long entries.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenPolynomial
/// </remarks>
public class GenPolynomial<C> : RingElem<GenPolynomial<C>>, IEnumerable<Monomial<C>>, ICloneable, IEquatable<GenPolynomial<C>>
    where C : RingElem<C>
{
    internal readonly GenPolynomialRing<C> Ring;
    internal readonly SortedDictionary<ExpVector, C> Terms;

    private IComparer<ExpVector> Comparator => Ring.Tord.GetDescendComparator();

    /// <summary>
    /// Constructs the zero polynomial for the supplied ring.
    /// </summary>
    /// <param name="ring">Polynomial ring factory.</param>
    public GenPolynomial(GenPolynomialRing<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        Ring = ring;
        Terms = CreateDictionary();
    }

    /// <summary>
    /// Constructs the constant polynomial <c>coefficient</c>.
    /// </summary>
    /// <param name="ring">Polynomial ring factory.</param>
    /// <param name="coefficient">Coefficient placed at the zero exponent.</param>
    public GenPolynomial(GenPolynomialRing<C> ring, C coefficient)
        : this(ring)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        if (!coefficient.IsZero())
        {
            Terms.Add(Ring.Evzero, coefficient);
        }
    }

    /// <summary>
    /// Constructs the monomial <c>coefficient * x^exponent</c>.
    /// </summary>
    /// <param name="ring">Polynomial ring factory.</param>
    /// <param name="coefficient">Coefficient placed at <paramref name="exponent"/>.</param>
    /// <param name="exponent">Exponent vector.</param>
    public GenPolynomial(GenPolynomialRing<C> ring, C coefficient, ExpVector exponent)
        : this(ring)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        if (!coefficient.IsZero())
        {
            Terms.Add(exponent, coefficient);
        }
    }

    internal GenPolynomial(GenPolynomialRing<C> ring, IDictionary<ExpVector, C> source)
        : this(ring, source, true)
    {
    }

    internal GenPolynomial(GenPolynomialRing<C> ring, IDictionary<ExpVector, C> source, bool copy)
        : this(ring)
    {
        ArgumentNullException.ThrowIfNull(source);
        foreach (KeyValuePair<ExpVector, C> pair in source)
        {
            AddTerm(Terms, pair.Key, copy ? pair.Value : pair.Value);
        }
    }

    /// <summary>
    /// Creates a deep copy of this polynomial, including the term map.
    /// </summary>
    public GenPolynomial<C> Clone()
    {
        return new GenPolynomial<C>(Ring, Terms);
    }

    public GenPolynomial<C> Copy() => Clone();

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Formats the polynomial with its ring information and listed terms.
    /// </summary>
    public override string ToString()
    {
        string[]? variables = Ring.GetVars();
        if (variables != null)
        {
            return ToString(variables);
        }

        StringBuilder builder = new();
        builder.Append(GetType().Name);
        builder.Append(":");
        builder.Append(Ring.CoFac.GetType().Name);

        try
        {
            System.Numerics.BigInteger characteristic = Ring.Characteristic();
            if (characteristic.Sign != 0)
            {
                builder.Append('(');
                builder.Append(characteristic);
                builder.Append(')');
            }
        }
        catch (NotSupportedException)
        {
            // Some coefficient factories may not support characteristic queries.
        }

        builder.Append("[ ");
        bool first = true;
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            if (!first)
            {
                builder.Append(", ");
            }

            first = false;
            builder.Append(term.Value);
            builder.Append(' ');
            builder.Append(term.Key);
        }

        builder.Append(" ]");
        return builder.ToString();
    }

    /// <summary>
    /// Returns the number of non-zero terms stored in this polynomial.
    /// </summary>
    public int Length()
    {
        return Terms.Count;
    }

    /// <summary>
    /// Returns a cloned map of exponents to coefficients for external inspection.
    /// </summary>
    public SortedDictionary<ExpVector, C> GetMap()
    {
        SortedDictionary<ExpVector, C> copy = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            copy.Add(term.Key, term.Value);
        }

        return copy;
    }

    /// <summary>
    /// Inserts or removes a coefficient in the internal term map.
    /// </summary>
    /// <param name="exponent">Exponent to update.</param>
    /// <param name="coefficient">Coefficient to store (zero removes the term).</param>
    public void DoPutToMap(ExpVector exponent, C coefficient)
    {
        ArgumentNullException.ThrowIfNull(exponent);
        ArgumentNullException.ThrowIfNull(coefficient);

        if (coefficient.IsZero())
        {
            Terms.Remove(exponent);
            return;
        }

        Terms[exponent] = coefficient;
    }

    /// <summary>
    /// Determines whether the polynomial has no terms.
    /// </summary>
    public bool IsZero()
    {
        return Terms.Count == 0;
    }

    /// <summary>
    /// Determines whether the polynomial equals one.
    /// </summary>
    public bool IsOne()
    {
        if (Terms.Count != 1)
        {
            return false;
        }

        KeyValuePair<ExpVector, C> term = Terms.First();
        return term.Key.Equals(Ring.Evzero) && term.Value.IsOne();
    }

    /// <summary>
    /// Determines whether the polynomial is a unit (constant invertible coefficient).
    /// </summary>
    public bool IsUnit()
    {
        if (Terms.Count != 1)
        {
            return false;
        }

        if (!Terms.TryGetValue(Ring.Evzero, out C coefficient))
        {
            return false;
        }

        return coefficient.IsUnit();
    }

    /// <summary>
    /// Determines whether the polynomial is constant (only zero exponent).
    /// </summary>
    public bool IsConstant()
    {
        if (Terms.Count != 1)
        {
            return false;
        }

        return Terms.ContainsKey(Ring.Evzero);
    }

    /// <summary>
    /// Lexicographically compares two sorted polynomial term sequences.
    /// </summary>
    /// <param name="other">Polynomial to compare against.</param>
    /// <returns>Sign of the comparison.</returns>
    public int CompareTo(GenPolynomial<C>? other)
    {
        if (other is null)
        {
            return 1;
        }

        using IEnumerator<KeyValuePair<ExpVector, C>> left = Terms.GetEnumerator();
        using IEnumerator<KeyValuePair<ExpVector, C>> right = other.Terms.GetEnumerator();
        bool hasLeft = left.MoveNext();
        bool hasRight = right.MoveNext();
        while (hasLeft && hasRight)
        {
            int exponentCompare = Comparator.Compare(left.Current.Key, right.Current.Key);
            if (exponentCompare != 0)
            {
                return exponentCompare;
            }

            int coefficientCompare = left.Current.Value.CompareTo(right.Current.Value);
            if (coefficientCompare != 0)
            {
                return coefficientCompare;
            }

            hasLeft = left.MoveNext();
            hasRight = right.MoveNext();
        }

        if (hasLeft)
        {
            return 1;
        }

        if (hasRight)
        {
            return -1;
        }

        return 0;
    }

    /// <summary>
    /// Compares by polynomial value using <see cref="CompareTo"/>.
    /// </summary>
    public bool Equals(GenPolynomial<C>? other)
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

        if (Terms.Count != other.Terms.Count)
        {
            return false;
        }

        foreach (var (expVector, c) in Terms)
        {
            if (!other.Terms.TryGetValue(expVector, out C value))
            {
                return false;
            }

            if (!c.Equals(value))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj is GenPolynomial<C> other && Equals(other);
    }

    /// <summary>
    /// Combines the hash of the ring and term map for dictionary lookups.
    /// </summary>
    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Ring);
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            hashCode.Add(term.Key);
            hashCode.Add(term.Value);
        }

        return hashCode.ToHashCode();
    }

    public static bool operator ==(GenPolynomial<C>? left, GenPolynomial<C>? right)
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

    public static bool operator !=(GenPolynomial<C>? left, GenPolynomial<C>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the polynomial with every coefficient replaced by its absolute value.
    /// </summary>
    public GenPolynomial<C> Abs()
    {
        if (IsZero())
        {
            return this;
        }

        C leading = LeadingBaseCoefficient();
        if (leading.Signum() < 0)
        {
            return Negate();
        }

        return this;
    }

    /// <summary>
    /// Returns the negation of this polynomial.
    /// </summary>
    public GenPolynomial<C> Negate()
    {
        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            result.Add(term.Key, term.Value.Negate());
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Returns the signum of the polynomial representation.
    /// </summary>
    public int Signum()
    {
        if (IsZero())
        {
            return 0;
        }

        C leading = LeadingBaseCoefficient();
        return leading.Signum();
    }

    /// <summary>
    /// Returns the number of variables in the underlying ring.
    /// </summary>
    public int NumberOfVariables()
    {
        return Ring.Nvar;
    }

    /// <summary>
    /// Adds another polynomial to this one.
    /// </summary>
    /// <param name="other">Polynomial to add.</param>
    public GenPolynomial<C> Sum(GenPolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        SortedDictionary<ExpVector, C> result = CloneTerms();
        foreach (KeyValuePair<ExpVector, C> term in other.Terms)
        {
            AddTerm(result, term.Key, term.Value);
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Adds a monomial to the polynomial.
    /// </summary>
    /// <param name="coefficient">Coefficient of the monomial.</param>
    /// <param name="exponent">Exponent vector.</param>
    public GenPolynomial<C> Sum(C coefficient, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        SortedDictionary<ExpVector, C> result = CloneTerms();
        AddTerm(result, exponent, coefficient);
        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Adds a constant term to the polynomial.
    /// </summary>
    /// <param name="coefficient">Constant coefficient.</param>
    public GenPolynomial<C> Sum(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        return Sum(coefficient, Ring.Evzero);
    }

    /// <summary>
    /// Subtracts another polynomial from this one.
    /// </summary>
    /// <param name="other">Polynomial to subtract.</param>
    public GenPolynomial<C> Subtract(GenPolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        SortedDictionary<ExpVector, C> result = CloneTerms();
        foreach (KeyValuePair<ExpVector, C> term in other.Terms)
        {
            AddTerm(result, term.Key, term.Value.Negate());
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Subtracts a monomial from the polynomial.
    /// </summary>
    /// <param name="coefficient">Coefficient of the monomial.</param>
    /// <param name="exponent">Exponent vector.</param>
    public GenPolynomial<C> Subtract(C coefficient, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        return Sum(coefficient.Negate(), exponent);
    }

    /// <summary>
    /// Subtracts a constant term from the polynomial.
    /// </summary>
    /// <param name="coefficient">Constant coefficient.</param>
    public GenPolynomial<C> Subtract(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        return Sum(coefficient.Negate());
    }

    /// <summary>
    /// Multiplies two polynomials.
    /// </summary>
    /// <param name="other">Polynomial to multiply.</param>
    public GenPolynomial<C> Multiply(GenPolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (IsZero() || other.IsZero())
        {
            return GenPolynomialRing<C>.Zero;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> left in Terms)
        {
            foreach (KeyValuePair<ExpVector, C> right in other.Terms)
            {
                ExpVector exponent = left.Key.Sum(right.Key);
                C coefficient = left.Value.Multiply(right.Value);
                AddTerm(result, exponent, coefficient);
            }
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Returns the monic version of this polynomial by dividing by the leading coefficient.
    /// </summary>
    public GenPolynomial<C> Monic()
    {
        if (IsZero())
        {
            return this;
        }

        C leading = LeadingBaseCoefficient();
        if (leading.IsOne())
        {
            return this;
        }

        return Multiply(leading.Inverse());
    }

    /// <summary>
    /// Multiplies every term by the given coefficient.
    /// </summary>
    /// <param name="coefficient">Coefficient multiplier.</param>
    public GenPolynomial<C> Multiply(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        if (coefficient.IsZero() || IsZero())
        {
            return GenPolynomialRing<C>.Zero;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            result.Add(term.Key, term.Value.Multiply(coefficient));
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Multiplies each term and shifts exponents by the specified vector.
    /// </summary>
    /// <param name="coefficient">Coefficient multiplier.</param>
    /// <param name="exponent">Exponent shift.</param>
    public GenPolynomial<C> Multiply(C coefficient, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        if (coefficient.IsZero() || IsZero())
        {
            return GenPolynomialRing<C>.Zero;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector newExponent = term.Key.Sum(exponent);
            result.Add(newExponent, term.Value.Multiply(coefficient));
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Shifts exponents by the provided vector without changing coefficients.
    /// </summary>
    /// <param name="exponent">Exponent shift.</param>
    public GenPolynomial<C> Multiply(ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(exponent);
        if (IsZero() || exponent.IsZero())
        {
            return this;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector newExponent = term.Key.Sum(exponent);
            result.Add(newExponent, term.Value);
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Divides every coefficient by the provided field element.
    /// </summary>
    /// <param name="divisor">Coefficient divisor.</param>
    public GenPolynomial<C> Divide(C divisor)
    {
        ArgumentNullException.ThrowIfNull(divisor);
        if (divisor.IsZero())
        {
            throw new ArithmeticException("division by zero");
        }

        if (IsZero())
        {
            return this;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            C quotient = term.Value.Divide(divisor);
            if (quotient.IsZero())
            {
                throw new ArithmeticException($"no exact division: {term.Value}/{divisor}, in {this}");
            }

            result.Add(term.Key, quotient);
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    /// <summary>
    /// Computes the quotient and remainder of division by <paramref name="divisor"/>.
    /// </summary>
    /// <param name="divisor">Polynomial divisor.</param>
    /// <returns>Array containing quotient and remainder.</returns>
    public GenPolynomial<C>[] QuotientRemainder(GenPolynomial<C> divisor)
    {
        ArgumentNullException.ThrowIfNull(divisor);
        if (divisor.IsZero())
        {
            throw new ArithmeticException("division by zero");
        }

        C leadingCoefficient = divisor.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsUnit())
        {
            throw new ArithmeticException($"leading coefficient not invertible {leadingCoefficient}");
        }

        C leadingInverse = leadingCoefficient.Inverse();
        var quotient = new GenPolynomial<C>(Ring);
        GenPolynomial<C> remainder = Clone();
        ExpVector? divisorLeadingExponent = divisor.LeadingExpVector();

        while (!remainder.IsZero())
        {
            ExpVector? remainderLeadingExponent = remainder.LeadingExpVector();
            if (divisorLeadingExponent is null || remainderLeadingExponent?.MultipleOf(divisorLeadingExponent) != true)
            {
                break;
            }

            C remainderLeadingCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = remainderLeadingExponent.Subtract(divisorLeadingExponent);
            C factor = remainderLeadingCoefficient.Multiply(leadingInverse);

            quotient = quotient.Sum(factor, exponentDifference);
            GenPolynomial<C> product = divisor.Multiply(factor, exponentDifference);
            remainder = remainder.Subtract(product);
        }

        return
        [
            quotient,
            remainder
        ];
    }

    /// <summary>
    /// Computes the modular inverse of this polynomial modulo <paramref name="modulus"/>.
    /// </summary>
    /// <param name="modulus">Modulus polynomial.</param>
    public GenPolynomial<C> ModInverse(GenPolynomial<C> modulus)
    {
        ArgumentNullException.ThrowIfNull(modulus);
        if (IsZero())
        {
            throw new NotInvertibleException("zero is not invertible");
        }

        GenPolynomial<C>[] result = Hegcd(modulus);
        GenPolynomial<C> gcd = result[0];
        if (!gcd.IsUnit())
        {
            GenPolynomial<C> factor = modulus.Divide(gcd);
            throw new AlgebraicNotInvertibleException("element not invertible, gcd != 1", modulus, gcd, factor);
        }

        GenPolynomial<C> inverse = result[1];
        if (inverse.IsZero())
        {
            throw new NotInvertibleException("element not invertible, divisible by modul");
        }

        return inverse;
    }

    /// <summary>
    /// Divides this polynomial by another, returning an exact quotient (throws otherwise).
    /// </summary>
    /// <param name="other">Divisor polynomial.</param>
    public GenPolynomial<C> Divide(GenPolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return QuotientRemainder(other)[0];
    }

    /// <summary>
    /// Computes the multiplicative inverse if this is unit constant; otherwise throws.
    /// </summary>
    public GenPolynomial<C> Inverse()
    {
        if (IsUnit())
        {
            C inverse = LeadingBaseCoefficient().Inverse();
            return GenPolynomialRing<C>.One.Multiply(inverse);
        }

        throw new NotInvertibleException($"element not invertible {this} :: {Ring}");
    }

    /// <summary>
    /// Computes the remainder when dividing by another polynomial.
    /// </summary>
    /// <param name="other">Divisor polynomial.</param>
    public GenPolynomial<C> Remainder(GenPolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return QuotientRemainder(other)[1];
    }

    /// <summary>
    /// Computes the greatest common divisor with another polynomial.
    /// </summary>
    public GenPolynomial<C> Gcd(GenPolynomial<C> other)
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

        if (Ring.Nvar != 1)
        {
            throw new ArgumentException("not univariate polynomials", nameof(other));
        }

        GenPolynomial<C> a = this;
        GenPolynomial<C> b = other;
        while (!b.IsZero())
        {
            GenPolynomial<C> remainder = a.Remainder(b);
            a = b;
            b = remainder;
        }

        return a.Monic();
    }

    /// <summary>
    /// Extended GCD performing Bézout coefficients.
    /// </summary>
    public GenPolynomial<C>[] Egcd(GenPolynomial<C> other)
    {
        GenPolynomial<C>[] result = new GenPolynomial<C>[3];

        if (other.IsZero())
        {
            result[0] = this;
            result[1] = GenPolynomialRing<C>.One;
            result[2] = GenPolynomialRing<C>.Zero;
            return result;
        }

        if (IsZero())
        {
            result[0] = other;
            result[1] = GenPolynomialRing<C>.Zero;
            result[2] = GenPolynomialRing<C>.One;
            return result;
        }

        if (Ring.Nvar != 1)
        {
            throw new ArgumentException("not univariate polynomials", nameof(other));
        }

        if (IsConstant() && other.IsConstant())
        {
            C a = LeadingBaseCoefficient();
            C b = other.LeadingBaseCoefficient();
            C[] gcd = a.Egcd(b);
            GenPolynomial<C> zero = GenPolynomialRing<C>.Zero;
            result[0] = zero.Sum(gcd[0]);
            result[1] = zero.Sum(gcd[1]);
            result[2] = zero.Sum(gcd[2]);
            return result;
        }

        GenPolynomial<C> q = this;
        GenPolynomial<C> r = other;
        GenPolynomial<C> c1 = GenPolynomialRing<C>.One.Clone();
        GenPolynomial<C> d1 = GenPolynomialRing<C>.Zero.Clone();
        GenPolynomial<C> c2 = GenPolynomialRing<C>.Zero.Clone();
        GenPolynomial<C> d2 = GenPolynomialRing<C>.One.Clone();

        while (!r.IsZero())
        {
            GenPolynomial<C>[] qr = q.QuotientRemainder(r);
            GenPolynomial<C> quotient = qr[0];
            GenPolynomial<C> remainder = qr[1];

            GenPolynomial<C> x1 = c1.Subtract(quotient.Multiply(d1));
            GenPolynomial<C> x2 = c2.Subtract(quotient.Multiply(d2));

            c1 = d1;
            c2 = d2;
            d1 = x1;
            d2 = x2;
            q = r;
            r = remainder;
        }

        C leading = q.LeadingBaseCoefficient();
        if (leading.IsUnit())
        {
            C inverse = leading.Inverse();
            q = q.Multiply(inverse);
            c1 = c1.Multiply(inverse);
            c2 = c2.Multiply(inverse);
        }

        result[0] = q;
        result[1] = c1;
        result[2] = c2;
        return result;
    }

    /// <summary>
    /// Half extended GCD variant returning [g, s] with s*this = g.
    /// </summary>
    public GenPolynomial<C>[] Hegcd(GenPolynomial<C> other)
    {
        GenPolynomial<C>[] result = new GenPolynomial<C>[2];

        if (other.IsZero())
        {
            result[0] = this;
            result[1] = GenPolynomialRing<C>.One;
            return result;
        }

        if (IsZero())
        {
            result[0] = other;
            result[1] = GenPolynomialRing<C>.Zero;
            return result;
        }

        if (Ring.Nvar != 1)
        {
            throw new ArgumentException("not univariate polynomials", nameof(other));
        }

        GenPolynomial<C> q = this;
        GenPolynomial<C> r = other;
        GenPolynomial<C> c1 = GenPolynomialRing<C>.One.Clone();
        GenPolynomial<C> d1 = GenPolynomialRing<C>.Zero.Clone();

        while (!r.IsZero())
        {
            GenPolynomial<C>[] qr = q.QuotientRemainder(r);
            GenPolynomial<C> quotient = qr[0];
            GenPolynomial<C> remainder = qr[1];

            GenPolynomial<C> x1 = c1.Subtract(quotient.Multiply(d1));
            c1 = d1;
            d1 = x1;
            q = r;
            r = remainder;
        }

        C leading = q.LeadingBaseCoefficient();
        if (leading.IsUnit())
        {
            C inverse = leading.Inverse();
            q = q.Multiply(inverse);
            c1 = c1.Multiply(inverse);
        }

        result[0] = q;
        result[1] = c1;
        return result;
    }

    /// <summary>
    /// Returns the polynomial ring factory that owns this element.
    /// </summary>
    public ElemFactory<GenPolynomial<C>> Factory()
    {
        return Ring;
    }

    /// <summary>
    /// Enumerates the terms (monomials) in descending order.
    /// </summary>
    public IEnumerator<Monomial<C>> GetEnumerator()
    {
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            yield return new Monomial<C>(term.Key, term.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns the leading monomial according to the ring's term order.
    /// </summary>
    public Monomial<C>? LeadingMonomial()
    {
        if (Terms.Count == 0)
        {
            return null;
        }

        KeyValuePair<ExpVector, C> term = Terms.First();
        return new Monomial<C>(term.Key, term.Value);
    }

    /// <summary>
    /// Returns the exponent vector of the leading term.
    /// </summary>
    public ExpVector? LeadingExpVector()
    {
        if (Terms.Count == 0)
        {
            return null;
        }

        return Terms.First().Key;
    }

    /// <summary>
    /// Returns the coefficient of the leading term.
    /// </summary>
    public C LeadingBaseCoefficient()
    {
        if (Terms.Count == 0)
        {
            return Ring.CoFac.FromInteger(0);
        }

        return Terms.First().Value;
    }

    /// <summary>
    /// Returns the exponent vector of the trailing term.
    /// </summary>
    public ExpVector? TrailingExpVector()
    {
        if (Terms.Count == 0)
        {
            return null;
        }

        return Terms.Last().Key;
    }

    /// <summary>
    /// Returns the coefficient of the trailing term.
    /// </summary>
    public C TrailingBaseCoefficient()
    {
        if (Terms.Count == 0)
        {
            return Ring.CoFac.FromInteger(0);
        }

        return Terms.Last().Value;
    }

    /// <summary>
    /// Retrieves the coefficient for the specified exponent.
    /// </summary>
    public C Coefficient(ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(exponent);
        return Terms.TryGetValue(exponent, out C? value) ? value : Ring.CoFac.FromInteger(0);
    }

    /// <summary>
    /// Returns the degree in the specified variable.
    /// </summary>
    public long Degree(int index)
    {
        if (Terms.Count == 0)
        {
            return 0;
        }

        int position = index >= 0 ? Ring.Nvar - 1 - index : Ring.Nvar + index;
        if (position < 0 || position >= Ring.Nvar)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Invalid variable index.");
        }

        long degree = 0;
        foreach (ExpVector exponent in Terms.Keys)
        {
            long value = exponent.GetVal(position);
            if (value > degree)
            {
                degree = value;
            }
        }

        return degree;
    }

    /// <summary>
    /// Returns the total degree (sum of all exponents) for the leading term.
    /// </summary>
    public long Degree()
    {
        if (Terms.Count == 0)
        {
            return 0;
        }

        long degree = 0;
        foreach (ExpVector exponent in Terms.Keys)
        {
            long value = exponent.MaxDeg();
            if (value > degree)
            {
                degree = value;
            }
        }

        return degree;
    }

    /// <summary>
    /// Returns the exponent vector of the term with maximal lexicographical order.
    /// </summary>
    public ExpVector DegreeVector()
    {
        if (Terms.Count == 0)
        {
            return Ring.Evzero;
        }

        long[] degrees = new long[Ring.Nvar];
        foreach (ExpVector exponent in Terms.Keys)
        {
            long[] values = exponent.GetVal();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > degrees[i])
                {
                    degrees[i] = values[i];
                }
            }
        }

        return ExpVector.Create(degrees);
    }

    /// <summary>
    /// Returns the maximal norm among all coefficients.
    /// </summary>
    public C MaxNorm()
    {
        C norm = Ring.CoFac.FromInteger(0);
        foreach (C coefficient in Terms.Values)
        {
            C absolute = coefficient.Abs();
            if (norm.CompareTo(absolute) < 0)
            {
                norm = absolute;
            }
        }

        return norm;
    }

    /// <summary>
    /// Returns the sum of absolute values of coefficients.
    /// </summary>
    public C SumNorm()
    {
        C norm = Ring.CoFac.FromInteger(0);
        foreach (C coefficient in Terms.Values)
        {
            norm = norm.Sum(coefficient.Abs());
        }

        return norm;
    }

    /// <summary>
    /// Embeds the polynomial into an extended ring by adding new variables at the front.
    /// </summary>
    public GenPolynomial<C> Extend(GenPolynomialRing<C> extendedRing, int index, long exponent)
    {
        ArgumentNullException.ThrowIfNull(extendedRing);
        if (Ring.Equals(extendedRing))
        {
            return this;
        }

        GenPolynomial<C> result = GenPolynomialRing<C>.Zero.Clone();
        if (IsZero())
        {
            return result;
        }

        int shift = extendedRing.Nvar - Ring.Nvar;
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector extendedExponent = term.Key.Extend(shift, index, exponent);
            result.DoPutToMap(extendedExponent, term.Value);
        }

        return result;
    }

    /// <summary>
    /// Embeds the polynomial into an extended ring by adding lower-order variables.
    /// </summary>
    public GenPolynomial<C> ExtendLower(GenPolynomialRing<C> extendedRing, int index, long exponent)
    {
        ArgumentNullException.ThrowIfNull(extendedRing);
        if (Ring.Equals(extendedRing))
        {
            return this;
        }

        GenPolynomial<C> result = GenPolynomialRing<C>.Zero.Clone();
        if (IsZero())
        {
            return result;
        }

        int shift = extendedRing.Nvar - Ring.Nvar;
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector extendedExponent = term.Key.ExtendLower(shift, index, exponent);
            result.DoPutToMap(extendedExponent, term.Value);
        }

        return result;
    }

    /// <summary>
    /// Contracts the polynomial by removing leading variables.
    /// </summary>
    public SortedDictionary<ExpVector, GenPolynomial<C>> Contract(GenPolynomialRing<C> contractedRing)
    {
        ArgumentNullException.ThrowIfNull(contractedRing);
        var order = new TermOrder(TermOrder.INVLEX);
        SortedDictionary<ExpVector, GenPolynomial<C>> contracted = new(order.GetAscendComparator());

        if (IsZero())
        {
            return contracted;
        }

        int shift = Ring.Nvar - contractedRing.Nvar;
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector exponent = term.Key;
            ExpVector leading = exponent.Contract(0, shift);
            ExpVector remainder = exponent.Contract(shift, exponent.Length() - shift);

            if (!contracted.TryGetValue(leading, out GenPolynomial<C>? polynomial))
            {
                polynomial = GenPolynomialRing<C>.Zero.Clone();
            }

            polynomial = polynomial.Sum(term.Value, remainder);
            contracted[leading] = polynomial;
        }

        return contracted;
    }

    /// <summary>
    /// Reverses the variable order according to the provided ring.
    /// </summary>
    public GenPolynomial<C> Reverse(GenPolynomialRing<C> reversedRing)
    {
        ArgumentNullException.ThrowIfNull(reversedRing);
        GenPolynomial<C> result = GenPolynomialRing<C>.Zero.Clone();
        if (IsZero())
        {
            return result;
        }

        int split = -1;
        if (reversedRing.Tord.GetEvord2() != 0 && reversedRing.IsPartial)
        {
            split = reversedRing.Tord.GetSplit();
        }

        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector reversedExponent = split >= 0 ? term.Key.Reverse(split) : term.Key.Reverse();
            result.DoPutToMap(reversedExponent, term.Value);
        }

        return result;
    }

    internal SortedDictionary<ExpVector, C> CloneTerms()
    {
        SortedDictionary<ExpVector, C> cloned = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            cloned.Add(term.Key, term.Value);
        }

        return cloned;
    }

    private SortedDictionary<ExpVector, C> CreateDictionary()
    {
        return new SortedDictionary<ExpVector, C>(Comparator);
    }

    private void AddTerm(SortedDictionary<ExpVector, C> terms, ExpVector exponent, C coefficient)
    {
        if (coefficient.IsZero())
        {
            return;
        }

        if (terms.TryGetValue(exponent, out C existing))
        {
            C sum = existing.Sum(coefficient);
            if (sum.IsZero())
            {
                terms.Remove(exponent);
            }
            else
            {
                terms[exponent] = sum;
            }
        }
        else
        {
            terms.Add(exponent, coefficient);
        }
    }

    /// <summary>
    /// Formats the polynomial using the given variable names.
    /// </summary>
    /// <param name="variables">Names matching the ring variables.</param>
    public string ToString(string[] variables)
    {
        ArgumentNullException.ThrowIfNull(variables);
        if (Terms.Count == 0)
        {
            return "0";
        }

        StringBuilder builder = new();
        bool first = true;
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            C coefficient = term.Value;
            int sign = coefficient.Signum();
            if (first)
            {
                first = false;
                if (sign < 0)
                {
                    builder.Append("- ");
                    coefficient = coefficient.Negate();
                }
            }
            else if (sign < 0)
            {
                builder.Append(" - ");
                coefficient = coefficient.Negate();
            }
            else
            {
                builder.Append(" + ");
            }

            bool printCoefficient = !coefficient.IsOne() || term.Key.IsZero();
            if (printCoefficient)
            {
                string coefficientString = coefficient.ToString();
                bool wrap = coefficientString.IndexOfAny([' ', '+', '-']) >= 0 && !coefficientString.StartsWith("(");
                if (wrap)
                {
                    builder.Append("( ");
                    builder.Append(coefficientString);
                    builder.Append(" )");
                }
                else
                {
                    builder.Append(coefficientString);
                }

                if (!term.Key.IsZero())
                {
                    builder.Append(' ');
                }
            }

            if (!term.Key.IsZero())
            {
                builder.Append(term.Key.ToString(variables));
            }
            else if (!printCoefficient)
            {
                builder.Append('1');
            }
        }

        return builder.ToString();
    }
}
