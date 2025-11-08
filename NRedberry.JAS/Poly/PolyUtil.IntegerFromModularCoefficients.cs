using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with modular coefficients into one with symmetric <see cref="BigInteger"/> coefficients.
    /// </summary>
    /// <typeparam name="C">Modular coefficient type.</typeparam>
    /// <param name="resultRing">Target polynomial ring with big integer coefficients.</param>
    /// <param name="polynomial">Polynomial over a modular ring.</param>
    /// <returns>Polynomial with coefficients lifted to symmetric integers.</returns>
    /// <remarks>Original Java method: PolyUtil#integerFromModularCoefficients (single polynomial overload).</remarks>
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

    /// <summary>
    /// Converts a list of modular-coefficient polynomials into big-integer polynomials.
    /// </summary>
    /// <typeparam name="C">Modular coefficient type.</typeparam>
    /// <param name="resultRing">Target polynomial ring.</param>
    /// <param name="polynomials">Polynomials over a modular ring.</param>
    /// <returns>List of polynomials with symmetric integer coefficients.</returns>
    /// <remarks>Original Java method: PolyUtil#integerFromModularCoefficients (list overload).</remarks>
    public static List<GenPolynomial<BigInteger>> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> resultRing, List<GenPolynomial<C>> polynomials)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomials);

        return polynomials.ConvertAll(p => IntegerFromModularCoefficients(resultRing, p));
    }
}
