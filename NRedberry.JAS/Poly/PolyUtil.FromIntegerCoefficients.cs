using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with <see cref="BigInteger"/> coefficients into one whose coefficients lie in <typeparamref name="C"/>.
    /// </summary>
    /// <typeparam name="C">Target coefficient type.</typeparam>
    /// <param name="ring">Result polynomial ring.</param>
    /// <param name="polynomial">Polynomial with big integer coefficients.</param>
    /// <returns>Polynomial with coefficients converted to <typeparamref name="C"/>.</returns>
    /// <remarks>Original Java method: PolyUtil#fromIntegerCoefficients (single polynomial overload).</remarks>
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

    /// <summary>
    /// Converts a list of big-integer polynomials into the target coefficient domain.
    /// </summary>
    /// <typeparam name="C">Target coefficient type.</typeparam>
    /// <param name="ring">Result polynomial ring.</param>
    /// <param name="polynomials">List of polynomials with big integer coefficients.</param>
    /// <returns>List whose items are converted to <typeparamref name="C"/> (or <see langword="null"/> when the input is null).</returns>
    /// <remarks>Original Java method: PolyUtil#fromIntegerCoefficients (list overload).</remarks>
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
