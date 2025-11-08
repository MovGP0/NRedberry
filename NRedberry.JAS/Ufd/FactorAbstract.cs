using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Abstract factorization algorithms class. This class contains implementations
/// of all methods of the <code>Factorization</code> interface, except the method
/// for factorization of a squarefree polynomial. The methods to obtain
/// squarefree polynomials delegate the computation to the
/// <code>GreatestCommonDivisor</code> classes and are included for convenience.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorAbstract
/// </remarks>
public abstract class FactorAbstract<C> : Factorization<C> where C : GcdRingElem<C>
{
    protected readonly GreatestCommonDivisorAbstract<C> engine;
    protected readonly SquarefreeAbstract<C> sengine;

    protected FactorAbstract()
    {
        throw new ArgumentException("don't use this constructor");
    }

    protected FactorAbstract(RingFactory<C> cfac)
    {
        engine = GCDFactory.GetProxy(cfac);
        sengine = SquarefreeFactory.GetImplementation(cfac);
    }

    public override string ToString()
    {
        return GetType().Name;
    }

    public bool IsIrreducible(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (!IsSquarefree(polynomial))
        {
            return false;
        }

        List<GenPolynomial<C>> factors = FactorsSquarefree(polynomial);
        if (factors.Count == 1)
        {
            return true;
        }

        if (factors.Count > 2)
        {
            return false;
        }

        foreach (GenPolynomial<C> factor in factors)
        {
            if (factor.IsConstant())
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSquarefree(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return sengine.IsSquarefree(polynomial);
    }

    public virtual List<GenPolynomial<C>> FactorsSquarefree(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomialRing<C> polynomialRing = polynomial.Ring;
        if (polynomialRing.Nvar == 1)
        {
            return BaseFactorsSquarefree(polynomial);
        }

        List<GenPolynomial<C>> factors = new ();
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.DegreeVector().TotalDeg() <= 1)
        {
            factors.Add(polynomial);
            return factors;
        }

        long substitutionDegree = polynomial.Degree() + 1;
        GenPolynomial<C> substituted = PolyUfdUtil.SubstituteKronecker(polynomial, substitutionDegree);
        GenPolynomialRing<C> univariateRing = substituted.Ring;
        univariateRing.SetVars(univariateRing.NewVars("zz"));

        List<GenPolynomial<C>> univariateFactors = new ();
        foreach (KeyValuePair<GenPolynomial<C>, long> entry in BaseFactors(substituted))
        {
            for (int i = 0; i < entry.Value; i++)
            {
                univariateFactors.Add(entry.Key);
            }
        }

        if (univariateFactors.Count == 1 && univariateFactors[0].Degree() == polynomial.Degree())
        {
            factors.Add(polynomial);
            return NormalizeFactorization(factors);
        }

        int maxSubsetSize = univariateFactors.Count - 1;
        int trialCounter = 0;
        GenPolynomial<C> remaining = polynomial;
        long maximumDegree = (remaining.Degree() + 1) / 2;

        ExpVector? leadingExponent = remaining.LeadingExpVector();
        ExpVector? trailingExponent = remaining.TrailingExpVector();

        for (int subsetSize = 1; subsetSize <= maxSubsetSize; subsetSize++)
        {
            foreach (List<GenPolynomial<C>> subset in new KsubSet<GenPolynomial<C>>(univariateFactors, subsetSize))
            {
                GenPolynomial<C> candidate = new GenPolynomial<C>(univariateRing, univariateRing.CoFac.FromInteger(1));
                foreach (GenPolynomial<C> factor in subset)
                {
                    candidate = candidate.Multiply(factor);
                }

                GenPolynomial<C> trial = PolyUfdUtil.BackSubstituteKronecker(polynomialRing, candidate, substitutionDegree);
                trialCounter++;

                if (trial.IsConstant() || trial.Degree() > maximumDegree)
                {
                    continue;
                }

                ExpVector? trialLeadingExponent = trial.LeadingExpVector();
                ExpVector? trialTrailingExponent = trial.TrailingExpVector();
                if (leadingExponent is not null && trialLeadingExponent is not null && !leadingExponent.MultipleOf(trialLeadingExponent))
                {
                    continue;
                }

                if (trailingExponent is not null && trialTrailingExponent is not null && !trailingExponent.MultipleOf(trialTrailingExponent))
                {
                    continue;
                }

                GenPolynomial<C> remainder = PolyUtil.BaseSparsePseudoRemainder(remaining, trial);
                if (!remainder.IsZero())
                {
                    continue;
                }

                factors.Add(trial);
                remaining = PolyUtil.BasePseudoDivide(remaining, trial);
                leadingExponent = remaining.LeadingExpVector();
                trailingExponent = remaining.TrailingExpVector();

                if (remaining.IsConstant())
                {
                    subsetSize = maxSubsetSize + 1;
                    break;
                }

                univariateFactors = RemoveOnce(univariateFactors, subset);
                maxSubsetSize = (univariateFactors.Count + 1) / 2;
                subsetSize = 0;
                break;
            }
        }

        if (!remaining.IsOne() && !remaining.Equals(polynomial))
        {
            factors.Add(remaining);
        }

        if (factors.Count == 0)
        {
            factors.Add(polynomial);
        }

        return NormalizeFactorization(factors);
    }

    public List<GenPolynomial<C>> BaseFactorsRadical(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        SortedDictionary<GenPolynomial<C>, long> factors = BaseFactors(polynomial);

        List<GenPolynomial<C>> result = new (factors.Count);
        foreach (GenPolynomial<C> factor in factors.Keys)
        {
            result.Add(factor);
        }

        return result;
    }

    public bool IsFactorization(GenPolynomial<C> polynomial, SortedDictionary<GenPolynomial<C>, long> factors)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(factors);

        if (polynomial.IsZero())
        {
            return factors.Count == 0;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        GenPolynomial<C> accumulated = new GenPolynomial<C>(ring, ring.CoFac.FromInteger(1));
        foreach (KeyValuePair<GenPolynomial<C>, long> entry in factors)
        {
            GenPolynomial<C> powered = Power<GenPolynomial<C>>.PositivePower(entry.Key, entry.Value);
            accumulated = accumulated.Multiply(powered);
        }

        if (polynomial.Equals(accumulated) || polynomial.Equals(accumulated.Negate()))
        {
            return true;
        }

        GenPolynomial<C> monicPolynomial = polynomial.Monic();
        GenPolynomial<C> monicAccumulated = accumulated.Monic();
        return monicPolynomial.Equals(monicAccumulated) || monicPolynomial.Equals(monicAccumulated.Negate());
    }

    public SortedDictionary<GenPolynomial<C>, long> BaseFactors(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomialRing<C> polynomialRing = polynomial.Ring;
        SortedDictionary<GenPolynomial<C>, long> factors = new (polynomialRing.GetComparator());

        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(polynomial));
        }

        if (polynomial.IsConstant())
        {
            factors[polynomial] = 1;
            return factors;
        }

        C content;
        if (polynomialRing.CoFac.IsField())
        {
            content = polynomial.LeadingBaseCoefficient();
        }
        else
        {
            content = engine.BaseContent(polynomial);
            if (polynomial.Signum() < 0 && content.Signum() > 0)
            {
                content = content.Negate();
            }
        }

        if (!content.IsOne())
        {
            GenPolynomial<C> constant = new (polynomialRing, content);
            factors[constant] = 1;
            polynomial = polynomial.Divide(content);
        }

        SortedDictionary<GenPolynomial<C>, long> squarefreeFactors = sengine.BaseSquarefreeFactors(polynomial);
        if (squarefreeFactors == null || squarefreeFactors.Count == 0)
        {
            squarefreeFactors = new SortedDictionary<GenPolynomial<C>, long>(polynomialRing.GetComparator())
            {
                { polynomial, 1 }
            };
        }

        foreach (KeyValuePair<GenPolynomial<C>, long> entry in squarefreeFactors)
        {
            GenPolynomial<C> factor = entry.Key;
            long multiplicity = entry.Value;

            if (polynomialRing.CoFac.IsField() && !factor.LeadingBaseCoefficient().IsOne())
            {
                factor = factor.Monic();
            }

            if (factor.Degree(0) <= 1)
            {
                if (!factor.IsOne())
                {
                    factors[factor] = multiplicity;
                }

                continue;
            }

            foreach (GenPolynomial<C> simpleFactor in BaseFactorsSquarefree(factor))
            {
                if (simpleFactor.IsOne())
                {
                    continue;
                }

                long exponent = multiplicity;
                if (factors.TryGetValue(simpleFactor, out long existing))
                {
                    exponent += existing;
                }

                factors[simpleFactor] = exponent;
            }
        }

        return factors;
    }

    public abstract List<GenPolynomial<C>> BaseFactorsSquarefree(GenPolynomial<C> polynomial);

    public SortedDictionary<GenPolynomial<C>, long> Factors(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomialRing<C> polynomialRing = polynomial.Ring;
        if (polynomialRing.Nvar == 1)
        {
            return BaseFactors(polynomial);
        }

        SortedDictionary<GenPolynomial<C>, long> factors = new (polynomialRing.GetComparator());
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsConstant())
        {
            factors[polynomial] = 1;
            return factors;
        }

        C content;
        if (polynomialRing.CoFac.IsField())
        {
            content = polynomial.LeadingBaseCoefficient();
        }
        else
        {
            content = engine.BaseContent(polynomial);
            if (polynomial.Signum() < 0 && content.Signum() > 0)
            {
                content = content.Negate();
            }
        }

        if (!content.IsOne())
        {
            GenPolynomial<C> constant = new (polynomialRing, content);
            factors[constant] = 1;
            polynomial = polynomial.Divide(content);
        }

        SortedDictionary<GenPolynomial<C>, long> squarefreeFactors = sengine.SquarefreeFactors(polynomial);
        if (squarefreeFactors == null || squarefreeFactors.Count == 0)
        {
            SortedDictionary<GenPolynomial<C>, long> fallback = new (polynomialRing.GetComparator())
            {
                { polynomial, 1 }
            };

            throw new InvalidOperationException("Unexpected empty squarefree factorization result: " + polynomial);
        }

        foreach (KeyValuePair<GenPolynomial<C>, long> entry in squarefreeFactors)
        {
            GenPolynomial<C> factor = entry.Key;
            if (factor.IsOne())
            {
                continue;
            }

            long multiplicity = entry.Value;
            foreach (GenPolynomial<C> simpleFactor in FactorsSquarefree(factor))
            {
                long exponent = multiplicity;
                if (factors.TryGetValue(simpleFactor, out long existing))
                {
                    exponent += existing;
                }

                factors[simpleFactor] = exponent;
            }
        }

        return factors;
    }

    public List<GenPolynomial<GenPolynomial<C>>> RecursiveFactorsSquarefree(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        List<GenPolynomial<GenPolynomial<C>>> factors = new ();
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsOne())
        {
            factors.Add(polynomial);
            return factors;
        }

        GenPolynomialRing<GenPolynomial<C>> polynomialRing = polynomial.Ring;
        var coefficientRing = (GenPolynomialRing<C>)polynomialRing.CoFac;
        GenPolynomialRing<C> extendedRing = coefficientRing.Extend(polynomialRing.GetVars());
        GenPolynomial<C> distributed = PolyUtil.Distribute(extendedRing, polynomial);

        C leadingCoefficient = distributed.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne() && leadingCoefficient.IsUnit())
        {
            distributed = distributed.Monic();
        }

        List<GenPolynomial<C>> distributedFactors = FactorsSquarefree(distributed);
        if (distributedFactors.Count <= 1)
        {
            factors.Add(polynomial);
            return factors;
        }

        if (!leadingCoefficient.IsOne() && leadingCoefficient.IsUnit())
        {
            GenPolynomial<C> firstFactor = distributedFactors[0];
            distributedFactors.RemoveAt(0);
            firstFactor = firstFactor.Multiply(leadingCoefficient);
            distributedFactors.Insert(0, firstFactor);
        }

        List<GenPolynomial<GenPolynomial<C>>> recursiveFactors = PolyUtil.Recursive(polynomialRing, distributedFactors);
        factors.AddRange(recursiveFactors);
        return factors;
    }

    public List<GenPolynomial<C>> NormalizeFactorization(List<GenPolynomial<C>> factors)
    {
        if (factors == null || factors.Count <= 1)
        {
            return factors;
        }

        List<GenPolynomial<C>> normalized = new (factors.Count);
        GenPolynomial<C> first = factors[0];
        for (int i = 1; i < factors.Count; i++)
        {
            GenPolynomial<C> factor = factors[i];
            if (factor.Signum() < 0)
            {
                factor = factor.Negate();
                first = first.Negate();
            }

            normalized.Add(factor);
        }

        if (!first.IsOne())
        {
            normalized.Insert(0, first);
        }

        return normalized;
    }

    private static List<T> RemoveOnce<T>(List<T> source, List<T> items)
    {
        List<T> result = new (source);
        foreach (T item in items)
        {
            result.Remove(item);
        }

        return result;
    }
}
