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
    public new GenSolvablePolynomial<C> ZERO { get; }
    public new GenSolvablePolynomial<C> ONE { get; }

    public GenSolvablePolynomialRing(RingFactory<C> coefficientFactory, int variables, TermOrder order, string[]? variableNames = null)
        : this(coefficientFactory, variables, order, variableNames, null)
    {
    }

    public GenSolvablePolynomialRing(RingFactory<C> coefficientFactory, int variables, TermOrder order, string[]? variableNames, RelationTable<C>? relationTable)
        : base(coefficientFactory, variables, order, variableNames ?? Array.Empty<string>())
    {
        Table = relationTable ?? new RelationTable<C>(this);
        ZERO = new GenSolvablePolynomial<C>(this);
        ONE = new GenSolvablePolynomial<C>(this, CoFac.FromInteger(1), Evzero);
    }

    public override string ToString()
    {
        string baseDescription = base.ToString();
        string relations = Table.ToString(vars);
        return $"{baseDescription}\n{relations}";
    }

    public override bool Equals(object? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is not GenSolvablePolynomialRing<C> ring || !base.Equals(other))
        {
            return false;
        }

        return Table.Equals(ring.Table);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Table);
    }

    public new GenSolvablePolynomial<C> GetZERO()
    {
        return ZERO;
    }

    public new GenSolvablePolynomial<C> GetONE()
    {
        return ONE;
    }

    public new GenSolvablePolynomial<C> FromInteger(long value)
    {
        return new GenSolvablePolynomial<C>(this, CoFac.FromInteger(value), Evzero);
    }

    public new GenSolvablePolynomial<C> FromInteger(BigInteger value)
    {
        return new GenSolvablePolynomial<C>(this, CoFac.FromInteger(value), Evzero);
    }

    public new GenSolvablePolynomial<C> Univariate(int index)
    {
        return FromBase(base.Univariate(index));
    }

    public new GenSolvablePolynomial<C> Univariate(int index, long exponent)
    {
        return FromBase(base.Univariate(index, exponent));
    }

    public new GenSolvablePolynomial<C> Univariate(int moduleVariables, int index, long exponent)
    {
        return FromBase(base.Univariate(moduleVariables, index, exponent));
    }

    public new List<GenSolvablePolynomial<C>> UnivariateList()
    {
        return base.UnivariateList().Select(FromBase).ToList();
    }

    public new List<GenSolvablePolynomial<C>> UnivariateList(int moduleVariables, long exponent)
    {
        return base.UnivariateList(moduleVariables, exponent).Select(FromBase).ToList();
    }

    private GenSolvablePolynomial<C> FromBase(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return polynomial is GenSolvablePolynomial<C> solvable
            ? solvable
            : new GenSolvablePolynomial<C>(this, polynomial.CloneTerms(), copy: false);
    }
}
