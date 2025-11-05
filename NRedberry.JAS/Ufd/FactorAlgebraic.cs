using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Algebraic number coefficients factorization algorithms.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorAlgebraic
/// </remarks>
public class FactorAlgebraic<C> : FactorAbsolute<AlgebraicNumber<C>> where C : GcdRingElem<C>
{
    protected readonly FactorAbstract<C> factorCoeff;

    public FactorAlgebraic(AlgebraicNumberRing<C> fac)
        : base(fac)
    {
        if (fac is null)
        {
            throw new ArgumentNullException(nameof(fac));
        }

        factorCoeff = FactorFactory.GetImplementation(fac.Ring.CoFac);
        _ = factorCoeff ?? throw new InvalidOperationException("Unable to resolve coefficient factorization engine.");
        PolyUfdUtil.EnsureFieldProperty(fac);
    }

    public FactorAlgebraic(AlgebraicNumberRing<C> fac, FactorAbstract<C> factorCoeff)
        : base(fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        this.factorCoeff = factorCoeff ?? throw new ArgumentNullException(nameof(factorCoeff));
        PolyUfdUtil.EnsureFieldProperty(fac);
    }

    public override List<GenPolynomial<AlgebraicNumber<C>>> BaseFactorsSquarefree(
        GenPolynomial<AlgebraicNumber<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        List<GenPolynomial<AlgebraicNumber<C>>> factors = new();
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsOne())
        {
            factors.Add(polynomial);
            return factors;
        }

        GenPolynomialRing<AlgebraicNumber<C>> polynomialRing = polynomial.Ring;
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(polynomial));
        }

        AlgebraicNumber<C> leadingCoefficient = polynomial.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsOne())
        {
            polynomial = polynomial.Monic();
            factors.Add(new GenPolynomial<AlgebraicNumber<C>>(polynomialRing, leadingCoefficient));
        }

        long selectedShift = 0;
        GenPolynomial<C>? norm = null;
        bool isSquarefree = false;
        int[] shifts = [0, -1, -2, 1, 2];
        int shiftIndex = 0;

        while (!isSquarefree && shiftIndex < shifts.Length)
        {
            selectedShift = shifts[shiftIndex++];

            norm = PolyUfdUtil.Norm(polynomial, selectedShift);
            if (norm.IsZero() || norm.IsConstant())
            {
                continue;
            }

            isSquarefree = factorCoeff.IsSquarefree(norm);
        }

        norm ??= PolyUfdUtil.Norm(polynomial, selectedShift);
        if (norm == null)
        {
            throw new InvalidOperationException("Failed to compute norm polynomial for algebraic factorization.");
        }

        List<GenPolynomial<C>> normFactors = factorCoeff.BaseFactorsRadical(norm);
        if (normFactors.Count == 1)
        {
            factors.Add(polynomial);
            return factors;
        }

        GenPolynomial<AlgebraicNumber<C>> remaining = polynomial;
        foreach (GenPolynomial<C> normFactor in normFactors)
        {
            GenPolynomial<AlgebraicNumber<C>> lifted = PolyUfdUtil.SubstituteConvertToAlgebraicCoefficients(polynomialRing, normFactor, selectedShift);
            GenPolynomial<AlgebraicNumber<C>> gcd = engine.Gcd(lifted, remaining);
            if (!gcd.LeadingBaseCoefficient().IsOne())
            {
                gcd = gcd.Monic();
            }

            if (gcd.IsOne())
            {
                continue;
            }

            factors.Add(gcd);
            remaining = remaining.Divide(gcd);
        }

        if (!remaining.IsZero() && !remaining.IsOne())
        {
            factors.Add(remaining);
        }

        return factors;
    }
}
