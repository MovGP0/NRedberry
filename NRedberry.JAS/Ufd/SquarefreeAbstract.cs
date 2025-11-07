using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Abstract squarefree decomposition class.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeAbstract
/// </remarks>
public abstract class SquarefreeAbstract<C> : Squarefree<C> where C : GcdRingElem<C>
{
    protected readonly GreatestCommonDivisorAbstract<C> engine;

    protected SquarefreeAbstract(GreatestCommonDivisorAbstract<C> engine)
    {
        ArgumentNullException.ThrowIfNull(engine);
        this.engine = engine;
    }

    public abstract GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> polynomial);

    public abstract SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> polynomial);

    public abstract GenPolynomial<GenPolynomial<C>> RecursiveUnivariateSquarefreePart(
        GenPolynomial<GenPolynomial<C>> polynomial);

    public abstract SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> RecursiveUnivariateSquarefreeFactors(
        GenPolynomial<GenPolynomial<C>> polynomial);

    public abstract GenPolynomial<C> SquarefreePart(GenPolynomial<C> polynomial);

    public virtual bool IsSquarefree(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomial<C> squarefree = SquarefreePart(polynomial);
        GenPolynomial<C> normalized = polynomial;
        if (polynomial.Ring.CoFac.IsField())
        {
            normalized = normalized.Monic();
        }
        else
        {
            normalized = engine.BasePrimitivePart(normalized);
        }

        return normalized.Equals(squarefree);
    }

    public abstract SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> polynomial);

    public SortedDictionary<GenPolynomial<C>, long>? NormalizeFactorization(
        SortedDictionary<GenPolynomial<C>, long>? factors)
    {
        if (factors == null || factors.Count <= 1)
        {
            return factors;
        }

        List<GenPolynomial<C>> entries = new(factors.Keys);
        GenPolynomial<C> first = entries[0];
        if (first.Ring.Characteristic().Sign != 0)
        {
            // Coefficients are ordered (positive characteristic), no normalization required.
            return factors;
        }

        long firstExponent = factors[first];
        SortedDictionary<GenPolynomial<C>, long> normalized = new(factors.Comparer);

        for (int i = 1; i < entries.Count; i++)
        {
            GenPolynomial<C> factor = entries[i];
            long exponent = factors[factor];
            if (factor.Signum() < 0)
            {
                factor = factor.Negate();
                if ((exponent & 1L) != 0)
                {
                    first = first.Negate();
                }
            }

            normalized[factor] = exponent;
        }

        if (!first.IsOne())
        {
            normalized[first] = firstExponent;
        }

        return normalized;
    }

    public bool IsFactorization(GenPolynomial<C> polynomial, List<GenPolynomial<C>> factors)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(factors);

        GenPolynomial<C> product = CreateUnit(polynomial.Ring);
        foreach (GenPolynomial<C> factor in factors)
        {
            product = product.Multiply(factor);
        }

        return IsEqualUpToSign(polynomial, product);
    }

    public bool IsFactorization(GenPolynomial<C> polynomial, SortedDictionary<GenPolynomial<C>, long> factors)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(factors);

        if (polynomial.IsZero())
        {
            return factors.Count == 0;
        }

        GenPolynomial<C> product = CreateUnit(polynomial.Ring);
        foreach (KeyValuePair<GenPolynomial<C>, long> entry in factors)
        {
            long exponent = entry.Value;
            if (exponent == 0)
            {
                continue;
            }

            GenPolynomial<C> term = Power<GenPolynomial<C>>.PositivePower(entry.Key, exponent);
            product = product.Multiply(term);
        }

        if (IsEqualUpToSign(polynomial, product))
        {
            return true;
        }

        // Normalize to monic representatives if possible.
        GenPolynomial<C> monicPolynomial = polynomial.Monic();
        GenPolynomial<C> monicProduct = product.Monic();
        return IsEqualUpToSign(monicPolynomial, monicProduct);
    }

    public abstract SortedDictionary<C, long> SquarefreeFactors(C coefficient);

    private static GenPolynomial<C> CreateUnit(GenPolynomialRing<C> ring)
    {
        C one = ring.CoFac.FromInteger(1);
        return new GenPolynomial<C>(ring, one);
    }

    private static bool IsEqualUpToSign(GenPolynomial<C> left, GenPolynomial<C> right)
    {
        return left.Equals(right) || left.Equals(right.Negate());
    }
}
