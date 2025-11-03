using System.Collections;
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
public class GenPolynomial<C> : RingElem<GenPolynomial<C>>, IEnumerable<Monomial<C>> where C : RingElem<C>
{
    internal readonly GenPolynomialRing<C> Ring;
    internal readonly SortedDictionary<ExpVector, C> Terms;

    private IComparer<ExpVector> Comparator => Ring.Tord.GetDescendComparator();

    public GenPolynomial(GenPolynomialRing<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        Ring = ring;
        Terms = CreateDictionary();
    }

    public GenPolynomial(GenPolynomialRing<C> ring, C coefficient)
        : this(ring)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        if (!coefficient.IsZero())
        {
            Terms.Add(Ring.Evzero, coefficient);
        }
    }

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

    public GenPolynomial<C> Clone()
    {
        return new GenPolynomial<C>(Ring, Terms);
    }

    public bool IsZero()
    {
        return Terms.Count == 0;
    }

    public bool IsOne()
    {
        if (Terms.Count != 1)
        {
            return false;
        }

        KeyValuePair<ExpVector, C> term = Terms.First();
        return term.Key.Equals(Ring.Evzero) && term.Value.IsOne();
    }

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

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not GenPolynomial<C> other)
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

        foreach (KeyValuePair<ExpVector, C> pair in Terms)
        {
            if (!other.Terms.TryGetValue(pair.Key, out C value))
            {
                return false;
            }

            if (!pair.Value.Equals(value))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        HashCode hash = new ();
        hash.Add(Ring);
        foreach (KeyValuePair<ExpVector, C> pair in Terms)
        {
            hash.Add(pair.Key);
            hash.Add(pair.Value);
        }

        return hash.ToHashCode();
    }

    public GenPolynomial<C> Abs()
    {
        C leading = LeadingBaseCoefficient();
        if (leading == null)
        {
            return this;
        }

        if (leading.Signum() < 0)
        {
            return Negate();
        }

        return this;
    }

    public GenPolynomial<C> Negate()
    {
        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            result.Add(term.Key, term.Value.Negate());
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    public int Signum()
    {
        if (IsZero())
        {
            return 0;
        }

        C leading = LeadingBaseCoefficient();
        return leading.Signum();
    }

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

    public GenPolynomial<C> Sum(C coefficient, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        SortedDictionary<ExpVector, C> result = CloneTerms();
        AddTerm(result, exponent, coefficient);
        return new GenPolynomial<C>(Ring, result, false);
    }

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

    public GenPolynomial<C> Multiply(GenPolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (IsZero() || other.IsZero())
        {
            return Ring.ZERO;
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

    public GenPolynomial<C> Multiply(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        if (coefficient.IsZero() || IsZero())
        {
            return Ring.ZERO;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            result.Add(term.Key, term.Value.Multiply(coefficient));
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    public GenPolynomial<C> Multiply(C coefficient, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        if (coefficient.IsZero() || IsZero())
        {
            return Ring.ZERO;
        }

        SortedDictionary<ExpVector, C> result = CreateDictionary();
        foreach (KeyValuePair<ExpVector, C> term in Terms)
        {
            ExpVector newExponent = term.Key.Sum(exponent);
            result.Add(newExponent, term.Value.Multiply(coefficient));
        }

        return new GenPolynomial<C>(Ring, result, false);
    }

    public GenPolynomial<C> Divide(GenPolynomial<C> other)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Inverse()
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Remainder(GenPolynomial<C> other)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Gcd(GenPolynomial<C> other)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C>[] Egcd(GenPolynomial<C> other)
    {
        throw new NotImplementedException();
    }

    public ElemFactory<GenPolynomial<C>> Factory()
    {
        return Ring;
    }

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

    public ExpVector? leadingExpVector()
    {
        if (Terms.Count == 0)
        {
            return null;
        }

        return Terms.First().Key;
    }

    public C LeadingBaseCoefficient()
    {
        if (Terms.Count == 0)
        {
            return Ring.CoFac.FromInteger(0);
        }

        return Terms.First().Value;
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
}
