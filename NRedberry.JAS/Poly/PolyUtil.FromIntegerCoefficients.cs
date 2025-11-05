using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<C> FromIntegerCoefficients<C>(GenPolynomialRing<C> ring, GenPolynomial<BigInteger> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);

        GenPolynomial<C> result = new (ring);
        if (polynomial.IsZero())
        {
            return result;
        }

        SortedDictionary<ExpVector, C> destination = result.Terms;
        foreach (KeyValuePair<ExpVector, BigInteger> term in polynomial.Terms)
        {
            destination[term.Key] = ring.CoFac.FromInteger(term.Value.Val);
        }

        return result;
    }

    public static List<GenPolynomial<C>>? FromIntegerCoefficients<C>(GenPolynomialRing<C> ring, List<GenPolynomial<BigInteger>>? polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        if (polynomials is null)
        {
            return null;
        }

        List<GenPolynomial<C>> result = new (polynomials.Count);
        foreach (GenPolynomial<BigInteger>? polynomial in polynomials)
        {
            result.Add(FromIntegerCoefficients(ring, polynomial));
        }

        return result;
    }
}
