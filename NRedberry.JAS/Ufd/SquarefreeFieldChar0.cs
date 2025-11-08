using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for coefficient fields of characteristic 0.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFieldChar0
/// </remarks>
public class SquarefreeFieldChar0<C> : SquarefreeAbstract<C> where C : GcdRingElem<C>
{
    private readonly RingFactory<C> coFac;

    public SquarefreeFieldChar0(RingFactory<C> fac)
        : base(GCDFactory.GetProxy(fac))
    {
        ArgumentNullException.ThrowIfNull(fac);
        if (!fac.IsField())
        {
            throw new ArgumentException("Coefficient factory must be a field.", nameof(fac));
        }

        if (fac.Characteristic().Sign != 0)
        {
            throw new ArgumentException("Characteristic must be zero.", nameof(fac));
        }

        coFac = fac;
    }

    public override string ToString()
    {
        return $"{GetType().Name} with {engine} over {coFac}";
    }

    public override GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        GenPolynomial<C> monic = polynomial.Monic();
        if (monic.IsConstant())
        {
            return monic;
        }

        GenPolynomial<C> derivative = PolyUtil.BaseDeriviative(monic).Monic();
        GenPolynomial<C> gcd = engine.BaseGcd(monic, derivative).Monic();
        return PolyUtil.BasePseudoDivide(monic, gcd).Monic();
    }

    public override SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> polynomial)
    {
        SortedDictionary<GenPolynomial<C>, long> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsConstant())
        {
            factors[polynomial] = 1L;
            return factors;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        const bool ensureMonicFactors = true;
        C leadingCoefficient = polynomial.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            polynomial = polynomial.Divide(leadingCoefficient);
            GenPolynomial<C> coefficientFactor = new (ring, leadingCoefficient);
            factors[coefficientFactor] = 1L;
        }

        GenPolynomial<C> current = polynomial;
        GenPolynomial<C>? gcd = null;
        GenPolynomial<C>? quotient = null;
        long multiplicity = 0L;
        bool initialize = true;

        while (true)
        {
            if (initialize)
            {
                if (current.IsConstant() || current.IsZero())
                {
                    break;
                }

                GenPolynomial<C> derivative = PolyUtil.BaseDeriviative(current);
                gcd = engine.BaseGcd(current, derivative).Monic();
                quotient = PolyUtil.BasePseudoDivide(current, gcd);
                multiplicity = 0L;
                initialize = false;
            }

            if (quotient!.IsConstant())
            {
                break;
            }

            multiplicity++;
            GenPolynomial<C> nextGcd = engine.BaseGcd(gcd!, quotient).Monic();
            GenPolynomial<C> factor = PolyUtil.BasePseudoDivide(quotient, nextGcd);
            quotient = nextGcd;
            gcd = PolyUtil.BasePseudoDivide(gcd!, quotient);

            if (factor.Degree(0) > 0)
            {
                if (ensureMonicFactors && !factor.LeadingBaseCoefficient().IsOne())
                {
                    factor = factor.Monic();
                }

                factors[factor] = multiplicity;
            }
        }

        return NormalizeFactorization(factors) ?? factors;
    }

    public override GenPolynomial<GenPolynomial<C>> RecursiveUnivariateSquarefreePart(
        GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        GenPolynomial<GenPolynomial<C>> primitive = polynomial;
        GenPolynomial<C> content = engine.RecursiveContent(primitive);
        if (!content.IsOne())
        {
            primitive = PolyUtil.CoefficientPseudoDivide(primitive, content);
        }

        GenPolynomial<GenPolynomial<C>> derivative = PolyUtil.RecursiveDeriviative(primitive);
        derivative = MakeMonicRecursive(derivative);

        GenPolynomial<GenPolynomial<C>> gcd = engine.RecursiveUnivariateGcd(primitive, derivative);
        gcd = MakeMonicRecursive(gcd);

        GenPolynomial<GenPolynomial<C>> quotient = PolyUtil.RecursivePseudoDivide(primitive, gcd);
        quotient = MakeMonicRecursive(quotient);
        return quotient;
    }

    public override SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> RecursiveUnivariateSquarefreeFactors(
        GenPolynomial<GenPolynomial<C>> polynomial)
    {
        SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsConstant())
        {
            factors[polynomial] = 1L;
            return factors;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        GenPolynomial<C> leadingCoefficient = polynomial.LeadingBaseCoefficient();
        const bool monicFactors = true;
        if (!leadingCoefficient.IsOne())
        {
            polynomial = PolyUtil.CoefficientPseudoDivide(polynomial, leadingCoefficient);
            factors[CreateConstantPolynomial(ring, leadingCoefficient)] = 1L;
        }

        GenPolynomial<GenPolynomial<C>> current = polynomial;
        GenPolynomial<GenPolynomial<C>>? gcd = null;
        GenPolynomial<GenPolynomial<C>>? quotient = null;
        long multiplicity = 0L;
        bool initialize = true;

        while (true)
        {
            if (initialize)
            {
                if (current.IsConstant() || current.IsZero())
                {
                    break;
                }

                GenPolynomial<GenPolynomial<C>> derivative = PolyUtil.RecursiveDeriviative(current);
                gcd = engine.RecursiveUnivariateGcd(current, derivative);
                gcd = MakeMonicRecursive(gcd);
                quotient = PolyUtil.RecursivePseudoDivide(current, gcd);
                multiplicity = 0L;
                initialize = false;
            }

            if (quotient!.IsConstant())
            {
                break;
            }

            multiplicity++;
            GenPolynomial<GenPolynomial<C>> nextGcd = engine.RecursiveUnivariateGcd(gcd!, quotient);
            nextGcd = MakeMonicRecursive(nextGcd);
            GenPolynomial<GenPolynomial<C>> factor = PolyUtil.RecursivePseudoDivide(quotient, nextGcd);
            quotient = nextGcd;
            gcd = PolyUtil.RecursivePseudoDivide(gcd!, quotient);

            if (!factor.IsOne() && !factor.IsZero())
            {
                if (monicFactors && !factor.LeadingBaseCoefficient().IsOne())
                {
                    factor = MakeMonicRecursive(factor);
                }

                factors[factor] = multiplicity;
            }
        }

        return factors;
    }

    public override GenPolynomial<C> SquarefreePart(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseSquarefreePart(polynomial);
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new (coefficientRing, 1);

        GenPolynomial<GenPolynomial<C>> recursivePolynomial = PolyUtil.Recursive(recursiveRing, polynomial);
        GenPolynomial<C> content = engine.RecursiveContent(recursivePolynomial);
        recursivePolynomial = PolyUtil.CoefficientPseudoDivide(recursivePolynomial, content);

        GenPolynomial<C> contentSquarefree = SquarefreePart(content);
        GenPolynomial<GenPolynomial<C>> recursiveSquarefree = RecursiveUnivariateSquarefreePart(recursivePolynomial);
        GenPolynomial<GenPolynomial<C>> liftedContent = CreateConstantPolynomial(recursiveRing, contentSquarefree);

        GenPolynomial<GenPolynomial<C>> combined = recursiveSquarefree.Multiply(liftedContent);
        return PolyUtil.Distribute(ring, combined);
    }

    public override SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar <= 1)
        {
            return NormalizeFactorization(BaseSquarefreeFactors(polynomial)) ?? new SortedDictionary<GenPolynomial<C>, long>();
        }

        SortedDictionary<GenPolynomial<C>, long> factors = new();
        if (polynomial.IsZero())
        {
            return NormalizeFactorization(factors) ?? factors;
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new (coefficientRing, 1);
        GenPolynomial<GenPolynomial<C>> recursivePolynomial = PolyUtil.Recursive(recursiveRing, polynomial);

        foreach (KeyValuePair<GenPolynomial<GenPolynomial<C>>, long> entry in RecursiveUnivariateSquarefreeFactors(recursivePolynomial))
        {
            GenPolynomial<C> distributed = PolyUtil.Distribute(ring, entry.Key);
            factors[distributed] = entry.Value;
        }

        return NormalizeFactorization(factors) ?? factors;
    }

    public override SortedDictionary<C, long> SquarefreeFactors(C coefficient)
    {
        throw new NotSupportedException("Coefficient squarefree factorization is not supported for characteristic 0 fields.");
    }

    private static GenPolynomial<GenPolynomial<C>> MakeMonicRecursive(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        return PolyUtil.Monic(polynomial) ?? polynomial;
    }

    private static GenPolynomial<GenPolynomial<C>> CreateConstantPolynomial(
        GenPolynomialRing<GenPolynomial<C>> ring,
        GenPolynomial<C> coefficient)
    {
        return new GenPolynomial<GenPolynomial<C>>(ring, coefficient);
    }
}
