using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<C> BaseDeriviative<C>(GenPolynomial<C> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials supported.", nameof(polynomial));
        }

        RingFactory<C> coefficientFactory = ring.CoFac;
        GenPolynomial<C> derivative = GenPolynomialRing<C>.Zero.Clone();
        SortedDictionary<ExpVector, C> derivativeTerms = derivative.Terms;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector exponent = term.Key;
            long power = exponent.GetVal(0);
            if (power <= 0)
            {
                continue;
            }

            C factor = coefficientFactory.FromInteger(power);
            C value = term.Value.Multiply(factor);
            if (value.IsZero())
            {
                continue;
            }

            ExpVector newExponent = ExpVector.Create(1, 0, power - 1);
            derivativeTerms[newExponent] = value;
        }

        return derivative;
    }
}
