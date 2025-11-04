using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Complex coefficients factorization algorithms.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorComplex
/// </remarks>
public class FactorComplex<C> : FactorAbsolute<Complex<C>> where C : GcdRingElem<C>
{
    private readonly FactorAbstract<AlgebraicNumber<C>> _factorAlgebraic;
    private readonly AlgebraicNumberRing<C> _algebraicRing;

    public FactorComplex(ComplexRing<C> fac)
        : base(fac)
    {
        ArgumentNullException.ThrowIfNull(fac);

        _algebraicRing = fac.AlgebraicRing();
        _factorAlgebraic = FactorFactory.GetImplementation(_algebraicRing);
    }

    public override List<GenPolynomial<Complex<C>>> BaseFactorsSquarefree(GenPolynomial<Complex<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        var factors = new List<GenPolynomial<Complex<C>>>();

        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsOne())
        {
            factors.Add(polynomial);
            return factors;
        }

        var polynomialRing = polynomial.Ring ?? throw new ArgumentException("Polynomial ring must be provided.", nameof(polynomial));
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(polynomial));
        }

        var complexRing = (ComplexRing<C>)polynomialRing.CoFac;
        if (!_algebraicRing.Ring.CoFac.Equals(complexRing.Ring))
        {
            throw new ArgumentException("Coefficient rings do not match.", nameof(polynomial));
        }

        Complex<C> leadingCoefficient = polynomial.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            polynomial = polynomial.Monic();
            var unitFactor = polynomialRing.ValueOf(leadingCoefficient);
            factors.Add(unitFactor);
        }

        string[]? variables = polynomialRing.GetVars();
        GenPolynomial<AlgebraicNumber<C>> algebraicPolynomial = PolyUtil.AlgebraicFromComplex(
            new GenPolynomialRing<AlgebraicNumber<C>>(_algebraicRing, polynomialRing.Nvar, polynomialRing.Tord, variables),
            polynomial);

        foreach (GenPolynomial<AlgebraicNumber<C>> algebraicFactor in _factorAlgebraic.BaseFactorsSquarefree(algebraicPolynomial))
        {
            GenPolynomial<Complex<C>> complexFactor = PolyUtil.ComplexFromAlgebraic(polynomialRing, algebraicFactor);
            factors.Add(complexFactor);
        }

        return factors;
    }
}
