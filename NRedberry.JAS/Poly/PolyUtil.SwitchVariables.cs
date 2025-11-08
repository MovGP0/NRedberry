using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<GenPolynomial<C>> SwitchVariables<C>(GenPolynomial<GenPolynomial<C>> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        GenPolynomialRing<GenPolynomial<C>> firstRing = polynomial.Ring;
        GenPolynomialRing<C> coefficientRing = (GenPolynomialRing<C>)firstRing.CoFac;
        GenPolynomialRing<C> swappedCoefficientRing = new GenPolynomialRing<C>(
            coefficientRing.CoFac,
            firstRing.Nvar,
            firstRing.Tord,
            firstRing.GetVars());
        GenPolynomial<C> zeroCoefficient = new GenPolynomial<C>(swappedCoefficientRing);
        GenPolynomialRing<GenPolynomial<C>> swappedRing = new GenPolynomialRing<GenPolynomial<C>>(
            swappedCoefficientRing,
            coefficientRing.Nvar,
            coefficientRing.Tord,
            coefficientRing.GetVars());
        GenPolynomial<GenPolynomial<C>> result = new GenPolynomial<GenPolynomial<C>>(swappedRing);

        if (polynomial.IsZero())
        {
            return result;
        }

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> outerTerm in polynomial.Terms)
        {
            GenPolynomial<C> coefficientPolynomial = outerTerm.Value;
            foreach (KeyValuePair<ExpVector, C> innerTerm in coefficientPolynomial.Terms)
            {
                GenPolynomial<C> coefficient = zeroCoefficient.Sum(innerTerm.Value, outerTerm.Key);
                result = result.Sum(coefficient, innerTerm.Key);
            }
        }

        return result;
    }
}
