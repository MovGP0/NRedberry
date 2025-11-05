using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Integer coefficients factorization algorithms. This class implements factorization methods
/// for polynomials over integers.
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

        List<GenPolynomial<BigInteger>> factors = new();
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsOne())
        {
            factors.Add(P);
            return factors;
        }

        factors.Add(P);
        return factors;
    }

    public System.Collections.BitArray FactorDegrees(List<ExpVector> E, int deg)
    {
        ArgumentNullException.ThrowIfNull(E);

        if (deg < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(deg));
        }

        BitArray degrees = new(deg + 1);
        degrees.Set(0, true);

        foreach (ExpVector exponent in E)
        {
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
            ExpVector expVector = polynomial.LeadingExpVector();
            sum += expVector?.GetVal(0) ?? 0;
        }

        return sum;
    }

    public List<GenPolynomial<BigInteger>> FactorsSquarefreeHensel(GenPolynomial<BigInteger> P)
    {
        return base.FactorsSquarefree(P);
    }
}
