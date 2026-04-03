using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Integer coefficients factorization algorithms. This class implements
/// factorization methods for polynomials over integers.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorInteger
/// </remarks>
public class FactorInteger<MOD> : FactorAbstract<BigInteger> where MOD : GcdRingElem<MOD>, Modular
{
    protected readonly FactorAbstract<MOD> mfactor;
    protected readonly GreatestCommonDivisorAbstract<MOD> mengine;

    public FactorInteger()
        : this(BigInteger.One)
    {
    }

    public FactorInteger(RingFactory<BigInteger> cfac)
        : base(cfac)
    {
        ArgumentNullException.ThrowIfNull(cfac);

        ModLongRing modularRing = new(13, true);
        FactorAbstract<ModLong> modularFactor = FactorFactory.GetImplementation(modularRing);
        GreatestCommonDivisorAbstract<ModLong> modularGcd = GCDFactory.GetImplementation(modularRing);

        mfactor = (FactorAbstract<MOD>)(object)modularFactor;
        mengine = (GreatestCommonDivisorAbstract<MOD>)(object)modularGcd;
    }

    public override List<GenPolynomial<BigInteger>> BaseFactorsSquarefree(GenPolynomial<BigInteger> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        List<GenPolynomial<BigInteger>> factors = [];
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsOne())
        {
            factors.Add(P);
            return factors;
        }

        GenPolynomialRing<BigInteger> polynomialRing = P.Ring;
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException($"{GetType().Name} only for univariate polynomials", nameof(P));
        }

        if (!engine.BaseContent(P).IsOne())
        {
            throw new ArgumentException($"{GetType().Name} P not primitive", nameof(P));
        }

        if (P.Degree(0) <= 1L)
        {
            factors.Add(P);
            return factors;
        }

        BigInteger norm = P.MaxNorm();
        BigInteger leadingCoefficient = P.LeadingBaseCoefficient();
        ExpVector degreeVector = P.DegreeVector();
        int degree = (int)P.Degree(0);
        BigInteger bound = norm.Multiply(PolyUtil.FactorBound(degreeVector));
        bound = bound.Multiply(leadingCoefficient.Abs().Multiply(leadingCoefficient.FromInteger(8)));

        PrimeList primes = new(PrimeList.Range.Small);
        const int trialFactorLists = 5;
        const int primeLimit = 30;
        int testedPrimeCount = 0;
        List<GenPolynomial<MOD>>[] modularFactorLists = new List<GenPolynomial<MOD>>[trialFactorLists];
        List<GenPolynomial<MOD>>? modularFactors = null;

        IEnumerator<BigInteger> primeEnumerator = primes.GetEnumerator();
        primeEnumerator.MoveNext();
        primeEnumerator.MoveNext();

        for (int trial = 0; trial < trialFactorLists; trial++)
        {
            if (trial == trialFactorLists - 1)
            {
                primes = new PrimeList(PrimeList.Range.Medium);
                primeEnumerator = primes.GetEnumerator();
            }

            while (primeEnumerator.MoveNext())
            {
                BigInteger prime = primeEnumerator.Current;
                testedPrimeCount++;
                if (testedPrimeCount >= primeLimit)
                {
                    throw new ArithmeticException("prime list exhausted");
                }

                ModularRingFactory<MOD> modularCoefficientFactory =
                    ModLongRing.MaxLong.CompareTo(prime.Val) > 0
                        ? (ModularRingFactory<MOD>)(object)new ModLongRing(prime.Val, true)
                        : (ModularRingFactory<MOD>)(object)new ModIntegerRing(prime.Val, true);

                MOD modularLeading = modularCoefficientFactory.FromInteger(leadingCoefficient.Val);
                if (modularLeading.IsZero())
                {
                    continue;
                }

                GenPolynomialRing<MOD> modularRing = new(
                    modularCoefficientFactory,
                    polynomialRing.Nvar,
                    polynomialRing.Tord,
                    polynomialRing.GetVars());
                GenPolynomial<MOD> modularPolynomial = PolyUtil.FromIntegerCoefficients(modularRing, P)
                    ?? throw new InvalidOperationException("Failed to map polynomial to modular coefficients.");
                if (!modularPolynomial.DegreeVector().Equals(degreeVector))
                {
                    continue;
                }

                GenPolynomial<MOD> derivative = PolyUtil.BaseDeriviative(modularPolynomial);
                if (derivative.IsZero())
                {
                    continue;
                }

                GenPolynomial<MOD> gcd = mengine.BaseGcd(modularPolynomial, derivative);
                if (!gcd.IsOne())
                {
                    continue;
                }

                if (!modularLeading.IsOne())
                {
                    modularPolynomial = modularPolynomial.Divide(modularLeading);
                }

                modularFactors = mfactor.BaseFactorsSquarefree(modularPolynomial);
                if (modularFactors.Count <= 1)
                {
                    factors.Add(P);
                    return factors;
                }

                if (!modularLeading.IsOne())
                {
                    GenPolynomial<MOD> content = modularRing.One.Multiply(modularLeading);
                    modularFactors.Insert(0, content);
                }

                modularFactorLists[trial] = modularFactors;
                break;
            }
        }

        int minimumSize = int.MaxValue;
        BitArray? allowedDegrees = null;
        for (int trial = 0; trial < trialFactorLists; trial++)
        {
            List<ExpVector?> leadingExponents = PolyUtil.LeadingExpVector(modularFactorLists[trial])
                ?? throw new InvalidOperationException("Missing modular factor list.");
            BitArray currentDegrees = FactorDegrees(leadingExponents, degree);
            if (allowedDegrees is null)
            {
                allowedDegrees = currentDegrees;
            }
            else
            {
                allowedDegrees.And(currentDegrees);
            }

            int size = modularFactorLists[trial].Count;
            if (size < minimumSize)
            {
                minimumSize = size;
                modularFactors = modularFactorLists[trial];
            }
        }

        if (modularFactors is null || modularFactors.Count <= 1)
        {
            factors.Add(P);
            return factors;
        }

        if (GetCardinality(allowedDegrees) <= 2)
        {
            factors.Add(P);
            return factors;
        }

        if (allowedDegrees is null)
        {
            factors.Add(P);
            return factors;
        }

        if (P.LeadingBaseCoefficient().IsOne())
        {
            try
            {
                List<GenPolynomial<MOD>> monicFactors = PolyUtil.Monic(modularFactors)
                    ?? throw new InvalidOperationException("Failed to normalize modular factors.");
                factors = SearchFactorsMonic(P, bound, monicFactors, allowedDegrees);
            }
            catch (Exception)
            {
                factors = SearchFactorsNonMonic(P, bound, modularFactors, allowedDegrees);
            }

            return NormalizeFactorization(factors);
        }

        factors = SearchFactorsNonMonic(P, bound, modularFactors, allowedDegrees);
        return NormalizeFactorization(factors);
    }

    public BitArray FactorDegrees(List<ExpVector?> E, int deg)
    {
        ArgumentNullException.ThrowIfNull(E);

        if (deg < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(deg));
        }

        BitArray degrees = new(deg + 1);
        degrees.Set(0, true);

        foreach (ExpVector? exponent in E)
        {
            if (exponent is null)
            {
                continue;
            }

            int value = (int)exponent.GetVal(0);
            if (value < 0 || value > deg)
            {
                continue;
            }

            BitArray shifted = new(deg + 1);
            for (int k = 0; k <= deg - value; k++)
            {
                if (degrees[k])
                {
                    shifted.Set(k + value, true);
                }
            }

            degrees.Or(shifted);
        }

        return degrees;
    }

    public static long DegreeSum<C>(List<GenPolynomial<C>> L) where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(L);

        long sum = 0;
        foreach (GenPolynomial<C> polynomial in L)
        {
            ExpVector? expVector = polynomial.LeadingExpVector();
            sum += expVector?.GetVal(0) ?? 0;
        }

        return sum;
    }

    public List<GenPolynomial<BigInteger>> FactorsSquarefreeHensel(GenPolynomial<BigInteger> P)
    {
        return base.FactorsSquarefree(P);
    }

    private List<GenPolynomial<BigInteger>> SearchFactorsMonic(
        GenPolynomial<BigInteger> polynomial,
        BigInteger bound,
        List<GenPolynomial<MOD>> modularFactors,
        BitArray allowedDegrees)
    {
        List<GenPolynomial<BigInteger>> factors = new(modularFactors.Count);
        List<GenPolynomial<MOD>> workingFactors = new(modularFactors);

        GenPolynomial<MOD> first = workingFactors[0];
        if (first.IsConstant())
        {
            workingFactors.RemoveAt(0);
            if (workingFactors.Count <= 1)
            {
                factors.Add(polynomial);
                return factors;
            }
        }

        ModularRingFactory<MOD> modularCoefficientFactory = (ModularRingFactory<MOD>)workingFactors[0].Ring.CoFac;
        BigInteger modulus = modularCoefficientFactory.GetIntegerModul();
        long liftingExponent = 1;
        BigInteger modulusPower = modulus;
        while (modulusPower.CompareTo(bound) < 0)
        {
            liftingExponent++;
            modulusPower = modulusPower.Multiply(modulus);
        }

        List<GenPolynomial<MOD>> liftedFactors = HenselUtil.LiftHenselMonic(polynomial, workingFactors, liftingExponent);
        GenPolynomialRing<MOD> liftedRing = liftedFactors[0].Ring;

        GenPolynomial<BigInteger> remaining = polynomial;
        int maxSubsetSize = (liftedFactors.Count + 1) / 2;

        for (int subsetSize = 1; subsetSize <= maxSubsetSize; subsetSize++)
        {
            foreach (List<GenPolynomial<MOD>> subset in new KsubSet<GenPolynomial<MOD>>(liftedFactors, subsetSize))
            {
                long subsetDegree = DegreeSum(subset);
                if (subsetDegree < 0 || subsetDegree >= allowedDegrees.Count || !allowedDegrees[(int)subsetDegree])
                {
                    continue;
                }

                GenPolynomial<MOD> modularTrial = Power<GenPolynomial<MOD>>.Multiply(liftedRing, subset);
                GenPolynomial<BigInteger> trial = PolyUtil.IntegerFromModularCoefficients(polynomial.Ring, modularTrial);
                trial = engine.BasePrimitivePart(trial);
                if (!PolyUtil.BaseSparsePseudoRemainder(remaining, trial).IsZero())
                {
                    continue;
                }

                factors.Add(trial);
                remaining = PolyUtil.BasePseudoDivide(remaining, trial);
                liftedFactors = RemoveOnce(liftedFactors, subset);
                maxSubsetSize = (liftedFactors.Count + 1) / 2;
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

    private List<GenPolynomial<BigInteger>> SearchFactorsNonMonic(
        GenPolynomial<BigInteger> polynomial,
        BigInteger bound,
        List<GenPolynomial<MOD>> modularFactors,
        BitArray allowedDegrees)
    {
        List<GenPolynomial<BigInteger>> factors = new(modularFactors.Count);
        List<GenPolynomial<MOD>> workingFactors = new(modularFactors);

        MOD normalizationFactor;
        GenPolynomial<MOD> first = workingFactors[0];
        if (first.IsConstant())
        {
            normalizationFactor = first.LeadingBaseCoefficient();
            workingFactors.RemoveAt(0);
            if (workingFactors.Count <= 1)
            {
                factors.Add(polynomial);
                return factors;
            }
        }
        else
        {
            normalizationFactor = first.Ring.CoFac.FromInteger(1);
        }

        GenPolynomialRing<MOD> modularRing = workingFactors[0].Ring;
        GenPolynomial<MOD> modularPolynomial = PolyUtil.FromIntegerCoefficients(modularRing, polynomial)
            ?? throw new InvalidOperationException("Failed to map polynomial to modular coefficients.");

        GenPolynomial<BigInteger> original = polynomial;
        GenPolynomial<BigInteger> remaining = polynomial;
        GenPolynomial<MOD> remainingModular = modularPolynomial;
        int maxSubsetSize = (workingFactors.Count + 1) / 2;

        for (int subsetSize = 1; subsetSize <= maxSubsetSize; subsetSize++)
        {
            foreach (List<GenPolynomial<MOD>> subset in new KsubSet<GenPolynomial<MOD>>(workingFactors, subsetSize))
            {
                long subsetDegree = DegreeSum(subset);
                if (subsetDegree < 0 || subsetDegree >= allowedDegrees.Count || !allowedDegrees[(int)subsetDegree])
                {
                    continue;
                }

                GenPolynomial<MOD> trialModular = modularRing.One.Multiply(normalizationFactor);
                foreach (GenPolynomial<MOD> factor in subset)
                {
                    trialModular = trialModular.Multiply(factor);
                }

                GenPolynomial<MOD> cofactorModular = remainingModular.Divide(trialModular);
                HenselApprox<MOD> lifted;
                try
                {
                    lifted = HenselUtil.LiftHenselQuadratic(remaining, bound, trialModular, cofactorModular);
                }
                catch (NoLiftingException)
                {
                    continue;
                }

                GenPolynomial<BigInteger> trial = engine.BasePrimitivePart(lifted.A);
                if (!PolyUtil.BaseSparsePseudoRemainder(remaining, trial).IsZero())
                {
                    continue;
                }

                factors.Add(trial);
                remaining = lifted.B;
                workingFactors = RemoveOnce(workingFactors, subset);
                remainingModular = cofactorModular;
                maxSubsetSize = (workingFactors.Count + 1) / 2;
                subsetSize = 0;
                break;
            }
        }

        if (!remaining.IsOne() && !remaining.Equals(original))
        {
            factors.Add(remaining);
        }

        if (factors.Count == 0)
        {
            factors.Add(original);
        }

        return NormalizeFactorization(factors);
    }

    private static int GetCardinality(BitArray? array)
    {
        if (array is null)
        {
            return 0;
        }

        int count = 0;
        foreach (bool bit in array)
        {
            if (bit)
            {
                count++;
            }
        }

        return count;
    }

    private static List<T> RemoveOnce<T>(List<T> source, List<T> items)
    {
        List<T> result = new(source);
        foreach (T item in items)
        {
            result.Remove(item);
        }

        return result;
    }
}
