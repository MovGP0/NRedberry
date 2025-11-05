using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Modular coefficients factorization algorithms.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorModular
/// </remarks>
public class FactorModular<MOD> : FactorAbsolute<MOD> where MOD : GcdRingElem<MOD>, Modular
{
    public FactorModular(RingFactory<MOD> cfac)
        : base(cfac)
    {
    }

    public SortedDictionary<long, GenPolynomial<MOD>> BaseDistinctDegreeFactors(GenPolynomial<MOD> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        SortedDictionary<long, GenPolynomial<MOD>> factors = new ();
        if (P.IsZero())
        {
            return factors;
        }

        GenPolynomialRing<MOD> polynomialRing = P.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(P));
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(P));
        }

        if (polynomialRing.CoFac is not ModularRingFactory<MOD> modularRing)
        {
            throw new ArgumentException("Coefficient ring must be modular.", nameof(P));
        }

        System.Numerics.BigInteger modulus = modularRing.GetIntegerModul().Val;
        GenPolynomial<MOD> variable = polynomialRing.Univariate(0);
        GenPolynomial<MOD> h = variable;
        GenPolynomial<MOD> f = P;
        Power<GenPolynomial<MOD>> power = new (polynomialRing);
        long degree = 0;
        while (degree + 1 <= f.Degree(0) / 2)
        {
            degree++;
            h = power.ModPower(h, modulus, f);
            GenPolynomial<MOD> gcd = engine.Gcd(h.Subtract(variable), f);
            if (!gcd.IsOne())
            {
                factors[degree] = gcd;
                f = f.Divide(gcd);
            }
        }

        if (!f.IsOne())
        {
            factors[f.Degree(0)] = f;
        }

        return factors;
    }

    public List<GenPolynomial<MOD>> BaseEqualDegreeFactors(GenPolynomial<MOD> P, long deg)
    {
        ArgumentNullException.ThrowIfNull(P);

        List<GenPolynomial<MOD>> factors = new ();
        if (P.IsZero())
        {
            return factors;
        }

        GenPolynomialRing<MOD> polynomialRing = P.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(P));
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(P));
        }

        if (P.Degree(0) == deg)
        {
            factors.Add(P);
            return factors;
        }

        if (polynomialRing.CoFac is not ModularRingFactory<MOD> modularRing)
        {
            throw new ArgumentException("Coefficient ring must be modular.", nameof(P));
        }

        System.Numerics.BigInteger modulus = modularRing.GetIntegerModul().Val;
        bool modulusIsTwo = modulus == 2;
        GenPolynomial<MOD> one = new (polynomialRing, polynomialRing.CoFac.FromInteger(1));
        GenPolynomial<MOD> t = polynomialRing.Univariate(0, 1L);
        GenPolynomial<MOD> f = P;
        Power<GenPolynomial<MOD>> power = new (polynomialRing);

        int intDegree = (int)deg;
        Arith.BigInteger modulusBigInteger = new (modulus);
        System.Numerics.BigInteger modulusPower = Power<Arith.BigInteger>.PositivePower(modulusBigInteger, deg).Val;
        modulusPower >>= 1;

        GenPolynomial<MOD> gcd;
        do
        {
            GenPolynomial<MOD> h;
            if (modulusIsTwo)
            {
                h = t;
                for (int i = 1; i < intDegree; i++)
                {
                    h = t.Sum(h.Multiply(h));
                    h = h.Remainder(f);
                }

                t = t.Multiply(polynomialRing.Univariate(0, 2L));
            }
            else
            {
                GenPolynomial<MOD> random = polynomialRing.Random(17, intDegree, 2 * intDegree, 1.0f);
                if (random.Degree(0) >= f.Degree(0))
                {
                    random = random.Remainder(f);
                }

                random = random.Monic();
                h = power.ModPower(random, modulusPower, f).Subtract(one);
                intDegree++;
            }

            gcd = engine.Gcd(h, f);
        }
        while (gcd.Degree(0) == 0 || gcd.Degree(0) == f.Degree(0));

        f = f.Divide(gcd);
        factors.AddRange(BaseEqualDegreeFactors(f, deg));
        factors.AddRange(BaseEqualDegreeFactors(gcd, deg));
        return factors;
    }

    public override List<GenPolynomial<MOD>> BaseFactorsSquarefree(GenPolynomial<MOD> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        List<GenPolynomial<MOD>> factors = new ();
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsOne())
        {
            factors.Add(P);
            return factors;
        }

        GenPolynomialRing<MOD> polynomialRing = P.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(P));
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(P));
        }

        if (!P.LeadingBaseCoefficient().IsOne())
        {
            throw new ArgumentException("Leading coefficient must be one for modular factorization.", nameof(P));
        }

        foreach (KeyValuePair<long, GenPolynomial<MOD>> entry in BaseDistinctDegreeFactors(P))
        {
            List<GenPolynomial<MOD>> equalDegreeFactors = BaseEqualDegreeFactors(entry.Value, entry.Key);
            factors.AddRange(equalDegreeFactors);
        }

        factors = PolyUtil.Monic(factors);
        SortedSet<GenPolynomial<MOD>> uniqueSorted = new (factors);
        factors.Clear();
        factors.AddRange(uniqueSorted);
        return factors;
    }
}
