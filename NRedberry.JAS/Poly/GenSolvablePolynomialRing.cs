using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenSolvablePolynomialRing generic solvable polynomial factory implementing RingFactory.
/// Factory for n-variate ordered solvable polynomials over C.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenSolvablePolynomialRing
/// </remarks>
public class GenSolvablePolynomialRing<C> : GenPolynomialRing<C> where C : RingElem<C>
{
    public RelationTable<C> Table { get; }

    private static GenSolvablePolynomial<C> ZERO = default!;

    private static GenSolvablePolynomial<C> ONE = default!;

    /// <summary>
    /// Zero solvable polynomial singleton.
    /// </summary>
    public new static GenSolvablePolynomial<C> Zero => ZERO;

    /// <summary>
    /// One solvable polynomial singleton.
    /// </summary>
    public new static GenSolvablePolynomial<C> One => ONE;

    /// <summary>
    /// Constructs a solvable polynomial ring with optional relation table.
    /// </summary>
    public GenSolvablePolynomialRing(RingFactory<C> coefficientFactory, int variables, TermOrder order, string[]? variableNames = null)
        : this(coefficientFactory, variables, order, variableNames, null)
    {
    }

    /// <summary>
    /// Constructs a solvable polynomial ring with explicit relation table support.
    /// </summary>
    public GenSolvablePolynomialRing(RingFactory<C> coefficientFactory, int variables, TermOrder order, string[]? variableNames, RelationTable<C>? relationTable)
        : base(coefficientFactory, variables, order, variableNames)
    {
        Table = relationTable ?? new RelationTable<C>(this);
        ZERO = new GenSolvablePolynomial<C>(this);
        ONE = new GenSolvablePolynomial<C>(this, CoFac.FromInteger(1), Evzero);
    }

    /// <summary>
    /// Formats the ring description plus relation table state.
    /// </summary>
    public override string ToString()
    {
        string baseDescription = base.ToString();
        string relations = Table.ToString(vars);
        return $"{baseDescription}\n{relations}";
    }

    /// <summary>
    /// Equality includes the relation table.
    /// </summary>
    public override bool Equals(object? other)
    {
        return ReferenceEquals(this, other)
            || (other is GenSolvablePolynomialRing<C> && base.Equals(other));
    }

    /// <summary>
    /// Hash combines the base ring and relation table.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Table);
    }

    /// <summary>
    /// Embeds a long integer into the solvable polynomial ring.
    /// </summary>
    public new GenSolvablePolynomial<C> FromInteger(long value)
    {
        return new GenSolvablePolynomial<C>(this, CoFac.FromInteger(value), Evzero);
    }

    /// <summary>
    /// Embeds a <see cref="BigInteger"/> into the solvable polynomial ring.
    /// </summary>
    public new GenSolvablePolynomial<C> FromInteger(BigInteger value)
    {
        return new GenSolvablePolynomial<C>(this, CoFac.FromInteger(value), Evzero);
    }

    /// <summary>
    /// Copies a solvable polynomial into this ring.
    /// </summary>
    public GenSolvablePolynomial<C> Copy(GenSolvablePolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return new GenSolvablePolynomial<C>(this, polynomial.CloneTerms(), copy: false);
    }

    /// <summary>
    /// Returns the solvable univariate generator for the given index.
    /// </summary>
    public new GenSolvablePolynomial<C> Univariate(int index)
    {
        return FromBase(base.Univariate(index));
    }

    /// <summary>
    /// Returns the monomial in the specified index and exponent.
    /// </summary>
    public new GenSolvablePolynomial<C> Univariate(int index, long exponent)
    {
        return FromBase(base.Univariate(index, exponent));
    }

    /// <summary>
    /// Returns the monomial after embedding additional module variables.
    /// </summary>
    public new GenSolvablePolynomial<C> Univariate(int moduleVariables, int index, long exponent)
    {
        return FromBase(base.Univariate(moduleVariables, index, exponent));
    }

    /// <summary>
    /// Gets a list of solvable univariate generators.
    /// </summary>
    public new List<GenSolvablePolynomial<C>> UnivariateList()
    {
        return base.UnivariateList().ConvertAll(FromBase);
    }

    /// <summary>
    /// Gets solvable univariate generators in the extended module space.
    /// </summary>
    public new List<GenSolvablePolynomial<C>> UnivariateList(int moduleVariables, long exponent)
    {
        return base.UnivariateList(moduleVariables, exponent).Select(FromBase).ToList();
    }

    /// <summary>
    /// Query if this ring is commutative.
    /// </summary>
    public new bool IsCommutative()
    {
        if (Table.Size() == 0)
        {
            return base.IsCommutative();
        }

        return false;
    }

    /// <summary>
    /// Query if this ring is associative.
    /// </summary>
    public new bool IsAssociative()
    {
        for (int i = 0; i < Nvar; i++)
        {
            GenSolvablePolynomial<C> xi = Univariate(i);
            for (int j = i + 1; j < Nvar; j++)
            {
                GenSolvablePolynomial<C> xj = Univariate(j);
                for (int k = j + 1; k < Nvar; k++)
                {
                    GenSolvablePolynomial<C> xk = Univariate(k);
                    GenSolvablePolynomial<C> left = xk.Multiply(xj).Multiply(xi);
                    GenSolvablePolynomial<C> right = xk.Multiply(xj.Multiply(xi));
                    if (!left.Equals(right))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Extends the number of variables by the given count.
    /// </summary>
    public new GenSolvablePolynomialRing<C> Extend(int count)
    {
        GenPolynomialRing<C> ring = base.Extend(count);
        GenSolvablePolynomialRing<C> extended = new (ring.CoFac, ring.Nvar, ring.Tord, ring.GetVars());
        extended.Table.Extend(Table);
        return extended;
    }

    /// <summary>
    /// Contracts the number of variables by the given count.
    /// </summary>
    public new GenSolvablePolynomialRing<C> Contract(int count)
    {
        GenPolynomialRing<C> ring = base.Contract(count);
        GenSolvablePolynomialRing<C> contracted = new (ring.CoFac, ring.Nvar, ring.Tord, ring.GetVars());
        contracted.Table.Contract(Table);
        return contracted;
    }

    /// <summary>
    /// Reverses the variable order.
    /// </summary>
    public new GenSolvablePolynomialRing<C> Reverse()
    {
        return Reverse(false);
    }

    /// <summary>
    /// Reverses the variable order.
    /// </summary>
    public new GenSolvablePolynomialRing<C> Reverse(bool partialReverse)
    {
        GenPolynomialRing<C> ring = base.Reverse(partialReverse);
        GenSolvablePolynomialRing<C> reversed = new (ring.CoFac, ring.Nvar, ring.Tord, ring.GetVars());
        reversed.partial = partialReverse;
        reversed.Table.Reverse(Table);
        return reversed;
    }

    /// <summary>
    /// Wraps a base polynomial into the solvable polynomial type.
    /// </summary>
    private GenSolvablePolynomial<C> FromBase(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return polynomial is GenSolvablePolynomial<C> solvable
            ? solvable
            : new GenSolvablePolynomial<C>(this, polynomial.CloneTerms(), copy: false);
    }
}
