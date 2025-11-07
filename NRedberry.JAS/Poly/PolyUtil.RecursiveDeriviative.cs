using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<GenPolynomial<C>> RecursiveDeriviative<C>(GenPolynomial<GenPolynomial<C>> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        if (ring.CoFac is not GenPolynomialRing<C> coefficientRing)
        {
            throw new ArgumentException($"{ring.CoFac.GetType().Name} must be a polynomial ring.", nameof(polynomial));
        }

        RingFactory<C> coefficientFactory = coefficientRing.CoFac;
        GenPolynomial<GenPolynomial<C>> derivative = new (ring);
        SortedDictionary<ExpVector, GenPolynomial<C>> derivativeTerms = derivative.Terms;

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            long exponent = term.Key.GetVal(0);
            if (exponent <= 0)
            {
                continue;
            }

            C scalar = coefficientFactory.FromInteger(exponent);
            GenPolynomial<C> coefficient = term.Value.Multiply(scalar);
            if (coefficient.IsZero())
            {
                continue;
            }

            ExpVector newExponent = ExpVector.Create(1, 0, exponent - 1);
            derivativeTerms[newExponent] = coefficient;
        }

        return derivative;
    }
}
