using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for fields of characteristic p.
/// </summary>
/// <typeparam name="C">Coefficient type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFieldCharP
/// </remarks>
public abstract class SquarefreeFieldCharP<C> : SquarefreeAbstract<C> where C : GcdRingElem<C>
{
    protected readonly RingFactory<C> coFac;

    protected SquarefreeFieldCharP(RingFactory<C> fac)
        : base(GCDFactory.GetProxy(fac))
    {
        ArgumentNullException.ThrowIfNull(fac);
        if (fac.Characteristic().Sign == 0)
        {
            throw new ArgumentException("Characteristic must be positive for SquarefreeFieldCharP.", nameof(fac));
        }

        coFac = fac;
    }

    public abstract GenPolynomial<C>? BaseSquarefreePRoot(GenPolynomial<C> polynomial);

    public abstract GenPolynomial<GenPolynomial<C>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<C>> polynomial);

    public override GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} supports univariate polynomials only.", nameof(polynomial));
        }

        GenPolynomial<C> result = CreateConstantPolynomial(ring, ring.CoFac.FromInteger(1));
        SortedDictionary<GenPolynomial<C>, long> factors = BaseSquarefreeFactors(polynomial);
        foreach (GenPolynomial<C> factor in factors.Keys)
        {
            result = result.Multiply(factor);
        }

        return result.Monic();
    }

    public override SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> polynomial)
    {
        SortedDictionary<GenPolynomial<C>, long> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (polynomial.IsConstant())
        {
            C coefficient = polynomial.LeadingBaseCoefficient();
            SortedDictionary<C, long> coefficientFactors = SquarefreeFactors(coefficient);
            if (coefficientFactors.Count == 0)
            {
                factors[polynomial] = 1L;
                return factors;
            }

            foreach (KeyValuePair<C, long> entry in coefficientFactors)
            {
                if (entry.Key.IsOne())
                {
                    continue;
                }

                GenPolynomial<C> constant = CreateConstantPolynomial(ring, entry.Key);
                factors[constant] = entry.Value;
            }

            return factors;
        }

        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} supports univariate polynomials only.", nameof(polynomial));
        }

        C leadingCoefficient = polynomial.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            polynomial = polynomial.Divide(leadingCoefficient);
            SortedDictionary<C, long> coefficientFactors = SquarefreeFactors(leadingCoefficient);
            if (coefficientFactors.Count == 0)
            {
                GenPolynomial<C> constant = CreateConstantPolynomial(ring, leadingCoefficient);
                factors[constant] = 1L;
            }
            else
            {
                foreach (KeyValuePair<C, long> entry in coefficientFactors)
                {
                    if (entry.Key.IsOne())
                    {
                        continue;
                    }

                    GenPolynomial<C> constant = CreateConstantPolynomial(ring, entry.Key);
                    factors[constant] = entry.Value;
                }
            }

            leadingCoefficient = ring.CoFac.FromInteger(1);
        }

        GenPolynomial<C> current = polynomial;
        long exponentScale = 1L;
        GenPolynomial<C>? gcd = null;
        GenPolynomial<C>? quotient = null;
        long multiplicity = 0L;
        long characteristicPower = 0L;
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
                characteristicPower = 0L;
                initialize = false;
            }

            if (quotient!.IsConstant())
            {
                characteristicPower = (long)ring.Characteristic();
                GenPolynomial<C>? root = BaseSquarefreePRoot(gcd!);
                current = root ?? new GenPolynomial<C>(ring);
                exponentScale *= characteristicPower;
                initialize = true;
                continue;
            }

            multiplicity++;
            if (characteristicPower != 0L && multiplicity % characteristicPower == 0L)
            {
                gcd = PolyUtil.BasePseudoDivide(gcd!, quotient);
                multiplicity++;
            }

            GenPolynomial<C> nextGcd = engine.BaseGcd(gcd!, quotient).Monic();
            GenPolynomial<C> factor = PolyUtil.BasePseudoDivide(quotient, nextGcd);
            quotient = nextGcd;
            gcd = PolyUtil.BasePseudoDivide(gcd!, quotient);

            if (factor.Degree(0) > 0)
            {
                if (!leadingCoefficient.IsOne() && exponentScale == 1L)
                {
                    factor = factor.Monic();
                }

                factors[factor] = exponentScale * multiplicity;
            }
        }

        return factors;
    }

    public override GenPolynomial<GenPolynomial<C>> RecursiveUnivariateSquarefreePart(
        GenPolynomial<GenPolynomial<C>> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} supports recursive univariate polynomials only.", nameof(polynomial));
        }

        GenPolynomial<GenPolynomial<C>> result = CreateConstantPolynomial(ring, ring.CoFac.FromInteger(1));
        SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> factors = RecursiveUnivariateSquarefreeFactors(polynomial);
        foreach (GenPolynomial<GenPolynomial<C>> factor in factors.Keys)
        {
            result = result.Multiply(factor);
        }

        return MakeMonicRecursive(result);
    }

    public override SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> RecursiveUnivariateSquarefreeFactors(
        GenPolynomial<GenPolynomial<C>> polynomial)
    {
        SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} supports recursive univariate polynomials only.", nameof(polynomial));
        }

        GenPolynomialRing<C> coefficientRing = (GenPolynomialRing<C>)ring.CoFac;
        GenPolynomial<C> leadingCoefficient = polynomial.LeadingBaseCoefficient();
        C leadingBase = leadingCoefficient.LeadingBaseCoefficient();
        if (!leadingBase.IsOne())
        {
            GenPolynomial<C> unit = CreateConstantPolynomial(coefficientRing, leadingBase);
            GenPolynomial<GenPolynomial<C>> constant = CreateConstantPolynomial(ring, unit);
            factors[constant] = 1L;

            C inverse = leadingBase.Inverse();
            GenPolynomial<C> inversePolynomial = CreateConstantPolynomial(coefficientRing, inverse);
            GenPolynomial<GenPolynomial<C>> liftedInverse = CreateConstantPolynomial(ring, inversePolynomial);
            polynomial = polynomial.Multiply(liftedInverse);
        }

        GenPolynomial<C> content = engine.RecursiveContent(polynomial).Monic();
        if (!content.IsOne())
        {
            polynomial = PolyUtil.CoefficientPseudoDivide(polynomial, content);
        }

        foreach (KeyValuePair<GenPolynomial<C>, long> entry in BaseSquarefreeFactors(content))
        {
            if (entry.Key.IsOne())
            {
                continue;
            }

            GenPolynomial<GenPolynomial<C>> constantFactor = CreateConstantPolynomial(ring, entry.Key);
            factors[constantFactor] = entry.Value;
        }

        GenPolynomial<GenPolynomial<C>> current = polynomial;
        long exponentScale = 1L;
        GenPolynomial<GenPolynomial<C>>? gcd = null;
        GenPolynomial<GenPolynomial<C>>? quotient = null;
        long multiplicity = 0L;
        long characteristicPower = 0L;
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
                characteristicPower = 0L;
                initialize = false;
            }

            if (quotient!.IsConstant())
            {
                characteristicPower = (long)ring.Characteristic();
                GenPolynomial<GenPolynomial<C>>? root = RecursiveUnivariateRootCharacteristic(gcd!);
                current = root ?? new GenPolynomial<GenPolynomial<C>>(ring);
                exponentScale *= characteristicPower;
                initialize = true;
                continue;
            }

            multiplicity++;
            if (characteristicPower != 0L && multiplicity % characteristicPower == 0L)
            {
                gcd = PolyUtil.RecursivePseudoDivide(gcd!, quotient);
                multiplicity++;
            }

            GenPolynomial<GenPolynomial<C>> nextGcd = engine.RecursiveUnivariateGcd(gcd!, quotient);
            nextGcd = MakeMonicRecursive(nextGcd);
            GenPolynomial<GenPolynomial<C>> factor = PolyUtil.RecursivePseudoDivide(quotient, nextGcd);
            quotient = nextGcd;
            gcd = PolyUtil.RecursivePseudoDivide(gcd!, quotient);

            if (!factor.IsOne() && !factor.IsZero())
            {
                factor = MakeMonicRecursive(factor);
                factors[factor] = exponentScale * multiplicity;
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

        GenPolynomial<C> result = CreateConstantPolynomial(ring, ring.CoFac.FromInteger(1));
        SortedDictionary<GenPolynomial<C>, long> factors = SquarefreeFactors(polynomial);
        foreach (GenPolynomial<C> factor in factors.Keys)
        {
            if (factor.IsConstant())
            {
                continue;
            }

            result = result.Multiply(factor);
        }

        return result.Monic();
    }

    public override SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseSquarefreeFactors(polynomial);
        }

        SortedDictionary<GenPolynomial<C>, long> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new(coefficientRing, 1);
        GenPolynomial<GenPolynomial<C>> recursivePolynomial = PolyUtil.Recursive(recursiveRing, polynomial);

        foreach (KeyValuePair<GenPolynomial<GenPolynomial<C>>, long> entry in RecursiveUnivariateSquarefreeFactors(recursivePolynomial))
        {
            GenPolynomial<C> distributed = PolyUtil.Distribute(ring, entry.Key);
            factors[distributed] = entry.Value;
        }

        return factors;
    }

    public override SortedDictionary<C, long> SquarefreeFactors(C coefficient)
    {
        SortedDictionary<C, long> factors = new();
        if (coefficient is null)
        {
            return factors;
        }

        if (!coefficient.IsOne())
        {
            factors[coefficient] = 1L;
        }

        return factors;
    }

    private static GenPolynomial<C> CreateConstantPolynomial(GenPolynomialRing<C> ring, C coefficient)
    {
        return new GenPolynomial<C>(ring, coefficient);
    }

    private static GenPolynomial<GenPolynomial<C>> CreateConstantPolynomial(
        GenPolynomialRing<GenPolynomial<C>> ring,
        GenPolynomial<C> coefficient)
    {
        return new GenPolynomial<GenPolynomial<C>>(ring, coefficient);
    }

    private static GenPolynomial<GenPolynomial<C>> MakeMonicRecursive(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        return PolyUtil.Monic(polynomial) ?? polynomial;
    }

    protected bool TryGetAlgebraicExtensionFactory(out object factory)
    {
        Type type = coFac.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AlgebraicNumberRing<>))
        {
            factory = coFac;
            return true;
        }

        factory = null!;
        return false;
    }

    protected bool TryGetQuotientExtensionFactory(out object factory)
    {
        Type type = coFac.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(QuotientRing<>))
        {
            factory = coFac;
            return true;
        }

        factory = null!;
        return false;
    }
}
