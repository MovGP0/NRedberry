using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<BigInteger> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> resultRing, GenPolynomial<C> polynomial)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomial<BigInteger> result = GenPolynomialRing<BigInteger>.Zero.Clone();
        SortedDictionary<ExpVector, BigInteger> terms = result.Terms;
        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            terms[term.Key] = term.Value.GetSymmetricInteger();
        }

        return result;
    }

    public static List<GenPolynomial<BigInteger>> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> resultRing, List<GenPolynomial<C>> polynomials)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomials);

        return polynomials.ConvertAll(p => IntegerFromModularCoefficients(resultRing, p));
    }
}
