using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Rational function coefficients factorization algorithms. This class
/// implements factorization methods for polynomials over rational functions,
/// that is, with coefficients from class <code>application.Quotient</code>.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorQuotient
/// </remarks>
public class FactorQuotient<C> : FactorAbstract<Quotient<C>> where C : GcdRingElem<C>
{
    private readonly FactorAbstract<C> _baseCoefficientEngine;

    public FactorQuotient(QuotientRing<C> fac)
        : base(fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        _baseCoefficientEngine = FactorFactory.GetImplementation(fac.Ring.CoFac);
    }

    public FactorQuotient(QuotientRing<C> fac, FactorAbstract<C> nengine)
        : base(fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        _baseCoefficientEngine = nengine ?? throw new ArgumentNullException(nameof(nengine));
    }

    public override List<GenPolynomial<Quotient<C>>> BaseFactorsSquarefree(GenPolynomial<Quotient<C>> P)
    {
        return FactorsSquarefree(P);
    }

    public override List<GenPolynomial<Quotient<C>>> FactorsSquarefree(GenPolynomial<Quotient<C>> P)
    {
        ArgumentNullException.ThrowIfNull(P);

        List<GenPolynomial<Quotient<C>>> factors = new ();
        if (P.IsZero())
        {
            return factors;
        }

        if (P.IsOne())
        {
            factors.Add(P);
            return factors;
        }

        GenPolynomialRing<Quotient<C>> polynomialRing = P.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(P));
        GenPolynomial<Quotient<C>> monicPolynomial = P;
        Quotient<C> leadingCoefficient = P.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            monicPolynomial = monicPolynomial.Monic();
        }

        if (polynomialRing.CoFac is not QuotientRing<C> quotientRing)
        {
            throw new ArgumentException("Coefficient ring must be a quotient ring.", nameof(P));
        }

        GenPolynomialRing<C> coefficientRing = quotientRing.Ring;
        GenPolynomialRing<GenPolynomial<C>> integralRing = new (coefficientRing, polynomialRing.Nvar, polynomialRing.Tord, polynomialRing.GetVars());
        GenPolynomial<GenPolynomial<C>> integralPolynomial = PolyUfdUtil.IntegralFromQuotientCoefficients(integralRing, monicPolynomial);

        List<GenPolynomial<GenPolynomial<C>>> integralFactors = _baseCoefficientEngine.RecursiveFactorsSquarefree(integralPolynomial);
        if (integralFactors.Count <= 1)
        {
            factors.Add(P);
            return factors;
        }

        List<GenPolynomial<Quotient<C>>> quotientFactors = PolyUfdUtil.QuotientFromIntegralCoefficients(polynomialRing, integralFactors);
        if (!leadingCoefficient.IsOne())
        {
            GenPolynomial<Quotient<C>> first = quotientFactors[0];
            quotientFactors.RemoveAt(0);
            first = first.Multiply(leadingCoefficient);
            quotientFactors.Insert(0, first);
        }

        factors.AddRange(quotientFactors);
        return factors;
    }
}
