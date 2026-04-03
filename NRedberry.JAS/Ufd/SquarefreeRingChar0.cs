using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for coefficient rings of characteristic 0.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeRingChar0
/// </remarks>
public class SquarefreeRingChar0<C> : SquarefreeAbstract<C> where C : GcdRingElem<C>
{
    private readonly RingFactory<C> coFac;

    public SquarefreeRingChar0(RingFactory<C> fac)
        : base(GCDFactory.GetProxy(fac))
    {
        ArgumentNullException.ThrowIfNull(fac);
        if (fac.IsField())
        {
            throw new ArgumentException("Coefficient factory must not be a field. Use SquarefreeFieldChar0 instead.", nameof(fac));
        }

        if (fac.Characteristic().Sign != 0)
        {
            throw new ArgumentException("Coefficient factory characteristic must be zero.", nameof(fac));
        }

        coFac = fac;
    }

    public override string ToString()
    {
        return $"{GetType().Name} with {engine} over {coFac}";
    }

    public override GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        if (P.IsZero())
        {
            return P;
        }

        GenPolynomialRing<C> ring = P.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only applies to univariate polynomials.", nameof(P));
        }

        GenPolynomial<C> primitive = engine.BasePrimitivePart(P);
        if (primitive.IsConstant())
        {
            return primitive;
        }

        GenPolynomial<C> derivative = PolyUtil.BaseDeriviative(primitive);
        derivative = engine.BasePrimitivePart(derivative);

        GenPolynomial<C> gcd = engine.BaseGcd(primitive, derivative);
        gcd = engine.BasePrimitivePart(gcd);

        GenPolynomial<C> quotient = PolyUtil.BasePseudoDivide(primitive, gcd);
        quotient = engine.BasePrimitivePart(quotient);

        return quotient;
    }

    public override SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        SortedDictionary<GenPolynomial<C>, long> factors = new();
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsConstant())
        {
            factors[P] = 1L;
            return factors;
        }

        GenPolynomialRing<C> ring = P.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only applies to univariate polynomials.", nameof(P));
        }

        C leadingCoefficient = P.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            C content = engine.BaseContent(P);
            P = P.Divide(content);
            GenPolynomial<C> coefficientFactor = ring.One.Multiply(content);
            factors[coefficientFactor] = 1L;
        }

        GenPolynomial<C> current = P;
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
                gcd = engine.BaseGcd(current, derivative);
                gcd = engine.BasePrimitivePart(gcd);
                quotient = PolyUtil.BasePseudoDivide(current, gcd);
                multiplicity = 0L;
                initialize = false;
            }

            if (quotient!.IsConstant())
            {
                break;
            }

            multiplicity++;
            GenPolynomial<C> nextGcd = engine.BaseGcd(gcd!, quotient);
            nextGcd = engine.BasePrimitivePart(nextGcd);
            GenPolynomial<C> factor = PolyUtil.BasePseudoDivide(quotient, nextGcd);
            quotient = nextGcd;
            gcd = PolyUtil.BasePseudoDivide(gcd!, quotient);

            if (factor.Degree(0) > 0)
            {
                if (leadingCoefficient.IsOne() && !factor.LeadingBaseCoefficient().IsOne())
                {
                    factor = engine.BasePrimitivePart(factor);
                }

                factors[factor] = multiplicity;
            }
        }

        return NormalizeFactorization(factors) ?? factors;
    }

    public override GenPolynomial<GenPolynomial<C>> RecursiveUnivariateSquarefreePart(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only applies to univariate polynomials.", nameof(polynomial));
        }

        GenPolynomial<GenPolynomial<C>> primitive = polynomial;
        GenPolynomial<C> content = engine.RecursiveContent(polynomial);
        content = engine.BasePrimitivePart(content);
        if (!content.IsOne())
        {
            primitive = PolyUtil.CoefficientPseudoDivide(primitive, content);
        }

        ExpVector? leadingExponent = primitive.LeadingExpVector();
        if (leadingExponent?.GetVal(0) < 1)
        {
            return primitive.Multiply(content);
        }

        GenPolynomial<GenPolynomial<C>> derivative = PolyUtil.RecursiveDeriviative(primitive);
        GenPolynomial<GenPolynomial<C>> gcd = engine.RecursiveUnivariateGcd(primitive, derivative);
        gcd = engine.BaseRecursivePrimitivePart(gcd);

        GenPolynomial<GenPolynomial<C>> quotient = PolyUtil.RecursivePseudoDivide(primitive, gcd);
        quotient = engine.BaseRecursivePrimitivePart(quotient);

        return quotient.Multiply(content);
    }

    public override SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> RecursiveUnivariateSquarefreeFactors(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only applies to univariate polynomials.", nameof(polynomial));
        }

        GenPolynomialRing<C> coefficientRing = (GenPolynomialRing<C>)ring.CoFac;
        C baseContent = engine.BaseRecursiveContent(polynomial);
        if (!baseContent.IsOne())
        {
            GenPolynomial<C> baseFactor = coefficientRing.One.Multiply(baseContent);
            GenPolynomial<GenPolynomial<C>> recursiveBaseFactor = ring.One.Multiply(baseFactor);
            factors[recursiveBaseFactor] = 1L;
            polynomial = PolyUtil.BaseRecursiveDivide(polynomial, baseContent);
        }

        GenPolynomial<C> content = engine.RecursiveContent(polynomial);
        content = engine.BasePrimitivePart(content);
        if (!content.IsOne())
        {
            polynomial = PolyUtil.CoefficientPseudoDivide(polynomial, content);
        }

        foreach (KeyValuePair<GenPolynomial<C>, long> entry in SquarefreeFactors(content))
        {
            if (entry.Key.IsOne())
            {
                continue;
            }

            GenPolynomial<GenPolynomial<C>> recursiveFactor = ring.One.Multiply(entry.Key);
            factors[recursiveFactor] = entry.Value;
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
                gcd = engine.BaseRecursivePrimitivePart(gcd);
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
            nextGcd = engine.BaseRecursivePrimitivePart(nextGcd);

            GenPolynomial<GenPolynomial<C>> factor = PolyUtil.RecursivePseudoDivide(quotient, nextGcd);
            quotient = nextGcd;
            gcd = PolyUtil.RecursivePseudoDivide(gcd!, quotient);

            if (!factor.IsOne() && !factor.IsZero())
            {
                factor = engine.BaseRecursivePrimitivePart(factor);
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
        GenPolynomial<C> coefficientContent = engine.RecursiveContent(recursivePolynomial);
        recursivePolynomial = PolyUtil.CoefficientPseudoDivide(recursivePolynomial, coefficientContent);
        GenPolynomial<C> contentSquarefree = SquarefreePart(coefficientContent);
        GenPolynomial<GenPolynomial<C>> recursiveSquarefree = RecursiveUnivariateSquarefreePart(recursivePolynomial);
        GenPolynomial<GenPolynomial<C>> combined = recursiveSquarefree.Multiply(contentSquarefree);

        return PolyUtil.Distribute(ring, combined);
    }

    public override SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return new SortedDictionary<GenPolynomial<C>, long>();
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseSquarefreeFactors(polynomial);
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new (coefficientRing, 1);

        GenPolynomial<GenPolynomial<C>> recursivePolynomial = PolyUtil.Recursive(recursiveRing, polynomial);
        SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> recursiveFactors =
            RecursiveUnivariateSquarefreeFactors(recursivePolynomial);

        SortedDictionary<GenPolynomial<C>, long> result = new();
        foreach (KeyValuePair<GenPolynomial<GenPolynomial<C>>, long> factor in recursiveFactors)
        {
            GenPolynomial<C> distributed = PolyUtil.Distribute(ring, factor.Key);
            result[distributed] = factor.Value;
        }

        return NormalizeFactorization(result) ?? result;
    }

    public override SortedDictionary<C, long> SquarefreeFactors(C coefficient)
    {
        throw new NotSupportedException("Squarefree factorization for coefficients is not implemented for characteristic 0 rings.");
    }
}
