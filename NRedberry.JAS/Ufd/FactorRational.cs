using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Rational number coefficients factorization algorithms. This class implements
/// factorization methods for polynomials over rational numbers.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorRational
/// </remarks>
public class FactorRational : FactorAbsolute<BigRational>
{
    private readonly FactorAbstract<BigInteger> _integerFactorEngine;

    public FactorRational()
        : base(BigRational.One)
    {
        _integerFactorEngine = FactorFactory.GetImplementation(BigInteger.One);
    }

    public override List<GenPolynomial<BigRational>> BaseFactorsSquarefree(GenPolynomial<BigRational> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        List<GenPolynomial<BigRational>> factors = new ();
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsOne())
        {
            factors.Add(P);
            return factors;
        }

        GenPolynomialRing<BigRational> polynomialRing = P.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(P));
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(P));
        }

        GenPolynomial<BigRational> monicPolynomial = P;
        BigRational leadingCoefficient = P.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            monicPolynomial = monicPolynomial.Monic();
        }

        BigInteger integerFactory = BigInteger.One;
        string[]? variables = polynomialRing.GetVars();
        GenPolynomialRing<BigInteger> integerRing = new (integerFactory, polynomialRing.Nvar, polynomialRing.Tord, variables);
        GenPolynomial<BigInteger> integerPolynomial = PolyUtil.IntegerFromRationalCoefficients(integerRing, monicPolynomial);
        List<GenPolynomial<BigInteger>> integerFactors = _integerFactorEngine.BaseFactorsSquarefree(integerPolynomial);
        if (integerFactors.Count <= 1)
        {
            factors.Add(P);
            return factors;
        }

        List<GenPolynomial<BigRational>> rationalFactors = PolyUtil.FromIntegerCoefficients(polynomialRing, integerFactors) ?? throw new InvalidOperationException("Failed to convert integer factors to rational coefficients.");
        rationalFactors = PolyUtil.Monic(rationalFactors);

        if (!leadingCoefficient.IsOne())
        {
            GenPolynomial<BigRational> firstFactor = rationalFactors[0];
            rationalFactors.RemoveAt(0);
            firstFactor = firstFactor.Multiply(leadingCoefficient);
            rationalFactors.Insert(0, firstFactor);
        }

        factors.AddRange(rationalFactors);
        return factors;
    }

    public override List<GenPolynomial<BigRational>> FactorsSquarefree(GenPolynomial<BigRational> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        List<GenPolynomial<BigRational>> factors = new ();
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsOne())
        {
            factors.Add(P);
            return factors;
        }

        GenPolynomialRing<BigRational> polynomialRing = P.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(P));
        if (polynomialRing.Nvar == 1)
        {
            return BaseFactorsSquarefree(P);
        }

        GenPolynomial<BigRational> monicPolynomial = P;
        BigRational leadingCoefficient = P.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            monicPolynomial = monicPolynomial.Monic();
        }

        BigInteger integerFactory = BigInteger.One;
        string[]? variables = polynomialRing.GetVars();
        GenPolynomialRing<BigInteger> integerRing = new (integerFactory, polynomialRing.Nvar, polynomialRing.Tord, variables);
        GenPolynomial<BigInteger> integerPolynomial = PolyUtil.IntegerFromRationalCoefficients(integerRing, monicPolynomial);
        List<GenPolynomial<BigInteger>> integerFactors = _integerFactorEngine.FactorsSquarefree(integerPolynomial);
        if (integerFactors.Count <= 1)
        {
            factors.Add(P);
            return factors;
        }

        List<GenPolynomial<BigRational>> rationalFactors = PolyUtil.FromIntegerCoefficients(polynomialRing, integerFactors) ?? throw new InvalidOperationException("Failed to convert integer factors to rational coefficients.");
        rationalFactors = PolyUtil.Monic(rationalFactors);

        if (!leadingCoefficient.IsOne())
        {
            GenPolynomial<BigRational> firstFactor = rationalFactors[0];
            rationalFactors.RemoveAt(0);
            firstFactor = firstFactor.Multiply(leadingCoefficient);
            rationalFactors.Insert(0, firstFactor);
        }

        factors.AddRange(rationalFactors);
        return factors;
    }
}
