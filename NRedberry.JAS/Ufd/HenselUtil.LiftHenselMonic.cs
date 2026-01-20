using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

public static partial class HenselUtil
{
    /// <summary>
    /// Lifts a set of monic modular factors of a polynomial to higher p-adic precision.
    /// </summary>
    /// <typeparam name="MOD">Modular coefficient type.</typeparam>
    /// <param name="C">Integral polynomial to be factored.</param>
    /// <param name="F">List of monic modular factors.</param>
    /// <param name="k">Desired lifting exponent.</param>
    /// <returns>List of lifted factors mod <c>p^k</c>.</returns>
    /// <remarks>Original Java method: HenselUtil#liftHenselMonic.</remarks>
    public static List<GenPolynomial<MOD>> LiftHenselMonic<MOD>(
        GenPolynomial<BigInteger> C,
        List<GenPolynomial<MOD>> F,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(C);
        ArgumentNullException.ThrowIfNull(F);
        if (C.IsZero() || F.Count == 0)
        {
            throw new ArgumentException("C must be nonzero and F must be nonempty.");
        }

        GenPolynomialRing<BigInteger> integerRing = C.Ring;
        if (integerRing.Nvar != 1)
        {
            throw new ArgumentException("polynomial ring must be univariate");
        }

        List<GenPolynomial<MOD>> result = new(F.Count);
        GenPolynomialRing<MOD> polynomialRing = F[0].Ring;
        ModularRingFactory<MOD> coefficientFactory = (ModularRingFactory<MOD>)polynomialRing.CoFac;
        BigInteger modulus = coefficientFactory.GetIntegerModul();
        int factorCount = F.Count;

        if (factorCount == 1)
        {
            GenPolynomial<MOD> factor = F[0];
            ModularRingFactory<MOD> liftedFactory = ModLongRing.MaxLong.CompareTo(modulus.Val) > 0
                ? (ModularRingFactory<MOD>)(object)new ModLongRing(modulus.Val)
                : (ModularRingFactory<MOD>)(object)new ModIntegerRing(modulus.Val);
            GenPolynomialRing<MOD> liftedRing = CreatePolynomialRingFromTemplate(liftedFactory, integerRing);
            GenPolynomial<MOD> liftedFactor = PolyUtil.FromIntegerCoefficients(
                liftedRing,
                PolyUtil.IntegerFromModularCoefficients(integerRing, factor))
                ?? throw new InvalidOperationException("Failed to lift polynomial factor.");
            result.Add(liftedFactor);
            return result;
        }

        GenPolynomialRing<BigInteger> liftRing = CreateIntegerPolynomialRing(integerRing);
        List<GenPolynomial<BigInteger>> Fi = PolyUtil.IntegerFromModularCoefficients(liftRing, F);
        List<GenPolynomial<MOD>> extended = LiftExtendedEuclidean(F, k + 1);
        List<GenPolynomial<BigInteger>> Si = PolyUtil.IntegerFromModularCoefficients(liftRing, extended);

        ModularRingFactory<MOD> currentFactory = coefficientFactory;
        BigInteger baseModulus = currentFactory.GetIntegerModul();
        BigInteger accumulatedModulus = baseModulus;
        GenPolynomialRing<MOD> modularRing = CreatePolynomialRingFromTemplate(currentFactory, integerRing);
        List<GenPolynomial<MOD>> Sp = PolyUtil.FromIntegerCoefficients(modularRing, Si)
            ?? throw new InvalidOperationException("Failed to map lifting coefficients.");

        for (int i = 1; i < k; i++)
        {
            GenPolynomial<BigInteger> error = integerRing.FromInteger(1);
            foreach (GenPolynomial<BigInteger> fi in Fi)
            {
                error = error.Multiply(fi);
            }

            error = C.Subtract(error);
            if (error.IsZero())
            {
                break;
            }

            try
            {
                error = error.Divide(accumulatedModulus);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Failed to divide error by modulus.", exception);
            }

            GenPolynomial<MOD> correction = PolyUtil.FromIntegerCoefficients(modularRing, error)
                ?? throw new InvalidOperationException("Failed to reduce correction polynomial.");

            List<GenPolynomial<MOD>> s = new(Sp.Count);
            for (int j = 0; j < Sp.Count; j++)
            {
                GenPolynomial<MOD> term = Sp[j].Multiply(correction);
                term = term.Remainder(F[j]);
                s.Add(term);
            }

            List<GenPolynomial<BigInteger>> si = PolyUtil.IntegerFromModularCoefficients(liftRing, s);
            List<GenPolynomial<BigInteger>> updated = new(Fi.Count);
            for (int j = 0; j < Fi.Count; j++)
            {
                GenPolynomial<BigInteger> lifted = Fi[j].Sum(si[j].Multiply(accumulatedModulus));
                updated.Add(lifted);
            }

            Fi = updated;
            accumulatedModulus = accumulatedModulus.Multiply(baseModulus);
            if (i >= k - 1)
            {
                break;
            }
        }

        BigInteger finalModulus = Power<BigInteger>.PositivePower(baseModulus, k);
        currentFactory = ModLongRing.MaxLong.CompareTo(finalModulus.Val) > 0
            ? (ModularRingFactory<MOD>)(object)new ModLongRing(finalModulus.Val)
            : (ModularRingFactory<MOD>)(object)new ModIntegerRing(finalModulus.Val);
        GenPolynomialRing<MOD> finalRing = CreatePolynomialRingFromTemplate(currentFactory, integerRing);
        result = PolyUtil.FromIntegerCoefficients(finalRing, Fi)
            ?? throw new InvalidOperationException("Failed to map lifted factors.");
        return result;
    }
}
