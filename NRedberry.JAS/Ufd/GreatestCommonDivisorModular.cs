using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with modular computation and chinese
/// remainder algorithm.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorModular
/// </remarks>
public class GreatestCommonDivisorModular<MOD>(bool simple) : GreatestCommonDivisorAbstract<BigInteger>
    where MOD : GcdRingElem<MOD>, Modular
{
    private const int PrimeBudget = 30;
    private const int GcdPrimeLimit = 10;

    protected readonly GreatestCommonDivisorAbstract<MOD> modularEngine = simple ? new GreatestCommonDivisorSimple<MOD>() : new GreatestCommonDivisorModEval<MOD>();
    protected readonly GreatestCommonDivisorAbstract<BigInteger> integerFallback = new GreatestCommonDivisorSubres<BigInteger>();

    public GreatestCommonDivisorModular()
        : this(false)
    {
    }

    public override GenPolynomial<BigInteger> BaseGcd(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        return integerFallback.BaseGcd(first, second);
    }

    public override GenPolynomial<GenPolynomial<BigInteger>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<BigInteger>> first,
        GenPolynomial<GenPolynomial<BigInteger>> second)
    {
        return integerFallback.RecursiveUnivariateGcd(first, second);
    }

    public override GenPolynomial<BigInteger> Gcd(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        GenPolynomialRing<BigInteger> ring = first.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseGcd(first, second);
        }

        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);

        GenPolynomial<BigInteger> q;
        GenPolynomial<BigInteger> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            (degreeSecond, degreeFirst) = (degreeFirst, degreeSecond);
        }
        else
        {
            q = first;
            r = second;
        }

        q = q.Abs();
        r = r.Abs();

        BigInteger contentR = BaseContent(r);
        BigInteger contentQ = BaseContent(q);
        BigInteger sharedContent = contentR.Gcd(contentQ);
        r = Divide(r, contentR);
        q = Divide(q, contentQ);

        if (r.IsOne())
        {
            return r.Multiply(sharedContent);
        }

        if (q.IsOne())
        {
            return q.Multiply(sharedContent);
        }

        BigInteger leadingR = r.LeadingBaseCoefficient();
        BigInteger leadingQ = q.LeadingBaseCoefficient();
        BigInteger normalization = leadingR.Gcd(leadingQ);

        BigInteger normR = r.MaxNorm();
        BigInteger normQ = q.MaxNorm();
        BigInteger completionBound = normR.CompareTo(normQ) < 0 ? normQ : normR;
        completionBound = completionBound.Multiply(normalization).Multiply(new BigInteger(2));

        ExpVector rDegreeVector = r.DegreeVector();
        ExpVector qDegreeVector = q.DegreeVector();

        BigInteger factorR = normR.Multiply(PolyUtil.FactorBound(rDegreeVector));
        BigInteger factorQ = normQ.Multiply(PolyUtil.FactorBound(qDegreeVector));

        PrimeList primes = new();
        ExpVector targetDegree = rDegreeVector.Subst(0, rDegreeVector.GetVal(0) + 1);

        int primeCount = 0;
        BigInteger? modulusProduct = null;
        GenPolynomial<BigInteger>? accumulated = null;

        foreach (BigInteger prime in primes)
        {
            if (prime.LongValue() == 2L)
            {
                continue;
            }

            if (++primeCount >= GcdPrimeLimit)
            {
                return integerFallback.Gcd(first, second);
            }

            if (!TryCreatePrimeFactory(prime, out ModularRingFactory<MOD>? coefficientFactory))
            {
                continue;
            }

            MOD normalizationFactor = coefficientFactory.FromInteger(normalization.GetVal());
            if (normalizationFactor.IsZero())
            {
                continue;
            }

            GenPolynomialRing<MOD> modularRing = new (coefficientFactory, ring.Nvar, ring.Tord, ring.GetVars());
            GenPolynomial<MOD> mappedQ = PolyUtil.FromIntegerCoefficients(modularRing, q);
            if (mappedQ.IsZero() || !DegreeVectorsEqual(mappedQ.DegreeVector(), qDegreeVector))
            {
                continue;
            }

            GenPolynomial<MOD> mappedR = PolyUtil.FromIntegerCoefficients(modularRing, r);
            if (mappedR.IsZero() || !DegreeVectorsEqual(mappedR.DegreeVector(), rDegreeVector))
            {
                continue;
            }

            GenPolynomial<MOD> modularGcd = modularEngine.Gcd(mappedR, mappedQ);
            if (modularGcd.IsConstant())
            {
                return CreateConstantPolynomial(ring, sharedContent);
            }

            ExpVector modularDegree = modularGcd.DegreeVector();
            bool resetAccumulation = false;
            if (!DegreeVectorsEqual(targetDegree, modularDegree))
            {
                if (targetDegree.MultipleOf(modularDegree))
                {
                    resetAccumulation = true;
                }
                else if (modularDegree.MultipleOf(targetDegree))
                {
                    continue;
                }
                else
                {
                    modulusProduct = null;
                    accumulated = null;
                    continue;
                }
            }

            if (resetAccumulation)
            {
                modulusProduct = null;
                accumulated = null;
            }

            modularGcd = modularGcd.Multiply(normalizationFactor);
            GenPolynomial<BigInteger> lifted = PolyUtil.IntegerFromModularCoefficients(ring, modularGcd);

            if (modulusProduct is null || accumulated is null)
            {
                modulusProduct = new BigInteger(prime);
                accumulated = lifted;
                targetDegree = targetDegree.Gcd(modularDegree);
            }
            else
            {
                BigInteger modulusInverse = ComputeModularInverse(modulusProduct.Mod(prime), prime);
                BigInteger newModulus = modulusProduct.Multiply(prime);
                accumulated = CombineChineseRemainder(ring, accumulated, modulusProduct, lifted, prime, modulusInverse, newModulus);
                modulusProduct = newModulus;
            }

            if (modulusProduct is not null && completionBound.CompareTo(modulusProduct) <= 0)
            {
                break;
            }

            if (accumulated.IsZero())
            {
                continue;
            }

            if (primeCount % 2 == 1)
            {
                GenPolynomial<BigInteger> candidate = BasePrimitivePart(accumulated);
                if (!PolyUtil.BaseSparsePseudoRemainder(q, candidate).IsZero())
                {
                    continue;
                }

                if (!PolyUtil.BaseSparsePseudoRemainder(r, candidate).IsZero())
                {
                    continue;
                }

                break;
            }
        }

        if (accumulated is null)
        {
            return integerFallback.Gcd(first, second);
        }

        GenPolynomial<BigInteger> primitive = BasePrimitivePart(accumulated);
        return primitive.Abs().Multiply(sharedContent);
    }

    public override GenPolynomial<BigInteger> Resultant(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        GenPolynomialRing<BigInteger> ring = first.Ring;
        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);

        GenPolynomial<BigInteger> q;
        GenPolynomial<BigInteger> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            (degreeSecond, degreeFirst) = (degreeFirst, degreeSecond);
        }
        else
        {
            q = first;
            r = second;
        }

        BigInteger normR = r.MaxNorm();
        BigInteger normQ = q.MaxNorm();
        BigInteger normPowerR = Power<BigInteger>.PowerMethod(ring.CoFac, normR, degreeSecond);
        BigInteger normPowerQ = Power<BigInteger>.PowerMethod(ring.CoFac, normQ, degreeFirst);
        BigInteger factorial = Combinatoric.Factorial(degreeFirst + degreeSecond);
        BigInteger completionBound = factorial.Multiply(normPowerR).Multiply(normPowerQ);

        ExpVector rLeadingExponent = r.LeadingExpVector();
        ExpVector qLeadingExponent = q.LeadingExpVector();

        PrimeList primes = new();
        int primeCount = 0;
        BigInteger? modulusProduct = null;
        GenPolynomial<BigInteger>? accumulated = null;

        foreach (BigInteger prime in primes)
        {
            if (prime.LongValue() == 2L)
            {
                continue;
            }

            if (++primeCount >= PrimeBudget)
            {
                return integerFallback.Resultant(first, second);
            }

            if (!TryCreatePrimeFactory(prime, out ModularRingFactory<MOD>? coefficientFactory))
            {
                continue;
            }

            GenPolynomialRing<MOD> modularRing = new (coefficientFactory, ring.Nvar, ring.Tord, ring.GetVars());
            GenPolynomial<MOD> mappedQ = PolyUtil.FromIntegerCoefficients(modularRing, q);
            if (mappedQ.IsZero() || !DegreeVectorsEqual(mappedQ.LeadingExpVector(), qLeadingExponent))
            {
                continue;
            }

            GenPolynomial<MOD> mappedR = PolyUtil.FromIntegerCoefficients(modularRing, r);
            if (mappedR.IsZero() || !DegreeVectorsEqual(mappedR.LeadingExpVector(), rLeadingExponent))
            {
                continue;
            }

            GenPolynomial<MOD> modularResultant = modularEngine.Resultant(mappedQ, mappedR);
            GenPolynomial<BigInteger> lifted = PolyUtil.IntegerFromModularCoefficients(ring, modularResultant);

            if (modulusProduct is null || accumulated is null)
            {
                modulusProduct = new BigInteger(prime);
                accumulated = lifted;
            }
            else
            {
                BigInteger modulusInverse = ComputeModularInverse(modulusProduct.Mod(prime), prime);
                BigInteger newModulus = modulusProduct.Multiply(prime);
                accumulated = CombineChineseRemainder(ring, accumulated, modulusProduct, lifted, prime, modulusInverse, newModulus);
                modulusProduct = newModulus;
            }

            if (modulusProduct is not null && completionBound.CompareTo(modulusProduct) <= 0)
            {
                break;
            }
        }

        if (accumulated is null)
        {
            return integerFallback.Resultant(first, second);
        }

        return accumulated;
    }

    public override GenPolynomial<BigInteger> BaseResultant(GenPolynomial<BigInteger> first, GenPolynomial<BigInteger> second)
    {
        return Resultant(first, second);
    }

    public override GenPolynomial<GenPolynomial<BigInteger>> RecursiveUnivariateResultant(
        GenPolynomial<GenPolynomial<BigInteger>> first,
        GenPolynomial<GenPolynomial<BigInteger>> second)
    {
        return RecursiveResultant(first, second);
    }

    private static bool DegreeVectorsEqual(ExpVector? left, ExpVector? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    private static bool TryCreatePrimeFactory(BigInteger prime, out ModularRingFactory<MOD>? factory)
    {
        factory = null;
        if (typeof(MOD) == typeof(ModLong))
        {
            if (ModLongRing.MaxLong.CompareTo(prime.GetVal()) <= 0)
            {
                return false;
            }

            factory = (ModularRingFactory<MOD>)(object)new ModLongRing(prime.GetVal(), true);
            return true;
        }

        if (typeof(MOD) == typeof(ModInteger))
        {
            factory = (ModularRingFactory<MOD>)(object)new ModIntegerRing(prime, true);
            return true;
        }

        throw new NotSupportedException($"Unsupported modular coefficient type '{typeof(MOD).FullName}'.");
    }

    private static GenPolynomial<BigInteger> CombineChineseRemainder(
        GenPolynomialRing<BigInteger> ring,
        GenPolynomial<BigInteger> accumulated,
        BigInteger accumulatedModulus,
        GenPolynomial<BigInteger> lifted,
        BigInteger prime,
        BigInteger modulusInverse,
        BigInteger newModulus)
    {
        GenPolynomial<BigInteger> result = new (ring);
        SortedDictionary<ExpVector, BigInteger> resultTerms = result.Terms;
        SortedDictionary<ExpVector, BigInteger> accumulatedTerms = accumulated.Terms;
        SortedDictionary<ExpVector, BigInteger> liftedTerms = lifted.Terms;

        HashSet<ExpVector> processed = new ();
        foreach (KeyValuePair<ExpVector, BigInteger> term in liftedTerms)
        {
            ExpVector exponent = term.Key;
            BigInteger nextCoefficient = PositiveMod(term.Value, prime);
            BigInteger currentCoefficient = accumulatedTerms.TryGetValue(exponent, out BigInteger current)
                ? PositiveMod(current, accumulatedModulus)
                : BigInteger.Zero;

            if (accumulatedTerms.ContainsKey(exponent))
            {
                processed.Add(exponent);
            }

            BigInteger combined = CombineCoefficient(currentCoefficient, nextCoefficient, accumulatedModulus, prime, modulusInverse, newModulus);
            if (!combined.IsZero())
            {
                resultTerms[exponent] = combined;
            }
        }

        foreach (KeyValuePair<ExpVector, BigInteger> term in accumulatedTerms)
        {
            if (processed.Contains(term.Key))
            {
                continue;
            }

            BigInteger coefficient = PositiveMod(term.Value, newModulus);
            if (!coefficient.IsZero())
            {
                resultTerms[term.Key] = coefficient;
            }
        }

        return result;
    }

    private static BigInteger CombineCoefficient(
        BigInteger currentCoefficient,
        BigInteger nextCoefficient,
        BigInteger accumulatedModulus,
        BigInteger prime,
        BigInteger modulusInverse,
        BigInteger newModulus)
    {
        BigInteger currentModPrime = PositiveMod(currentCoefficient.Mod(prime), prime);
        BigInteger difference = nextCoefficient.Subtract(currentModPrime);
        difference = PositiveMod(difference, prime);

        BigInteger adjustment = difference.Multiply(modulusInverse);
        adjustment = PositiveMod(adjustment, prime);

        BigInteger increment = adjustment.Multiply(accumulatedModulus);
        BigInteger combined = currentCoefficient.Sum(increment);
        return PositiveMod(combined, newModulus);
    }

    private static BigInteger ComputeModularInverse(BigInteger value, BigInteger modulus)
    {
        if (value.IsZero())
        {
            throw new ArithmeticException("Value is not invertible modulo the current modulus.");
        }

        BigInteger[] extended = value.Egcd(modulus);
        if (!extended[0].IsOne())
        {
            throw new ArithmeticException("Modulus and value are not coprime.");
        }

        BigInteger inverse = extended[1].Mod(modulus);
        if (inverse.Signum() < 0)
        {
            inverse = inverse.Sum(modulus);
        }

        return inverse;
    }

    private static BigInteger PositiveMod(BigInteger value, BigInteger modulus)
    {
        BigInteger remainder = value.Mod(modulus);
        if (remainder.Signum() < 0)
        {
            remainder = remainder.Sum(modulus);
        }

        return remainder;
    }

    private static GenPolynomial<BigInteger> CreateConstantPolynomial(GenPolynomialRing<BigInteger> ring, BigInteger coefficient)
    {
        GenPolynomial<BigInteger> result = new (ring);
        if (!coefficient.IsZero())
        {
            result.Terms[ring.Evzero] = coefficient;
        }

        return result;
    }
}
