using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Distributes a recursive polynomial into the combined polynomial ring respecting arbitrary term orders.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="distributedRing">Combined polynomial ring factory.</param>
    /// <param name="recursivePolynomial">Recursive polynomial to convert.</param>
    /// <returns>The distributed polynomial.</returns>
    /// <remarks>Original Java method: PolyUtil#distribute.</remarks>
    public static GenPolynomial<C> Distribute<C>(GenPolynomialRing<C> distributedRing, GenPolynomial<GenPolynomial<C>> recursivePolynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(distributedRing);
        ArgumentNullException.ThrowIfNull(recursivePolynomial);

        GenPolynomial<C> result = GenPolynomialRing<C>.Zero.Clone();
        if (recursivePolynomial.IsZero())
        {
            return result;
        }

        SortedDictionary<ExpVector, C> resultTerms = result.Terms;
        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> outer in recursivePolynomial.Terms)
        {
            ExpVector head = outer.Key;
            GenPolynomial<C> inner = outer.Value;

            foreach (KeyValuePair<ExpVector, C> innerTerm in inner.Terms)
            {
                ExpVector exponent = head.Combine(innerTerm.Key);
                if (resultTerms.ContainsKey(exponent))
                {
                    throw new InvalidOperationException("Duplicate exponent encountered during distribution.");
                }

                resultTerms[exponent] = innerTerm.Value;
            }
        }

        return result;
    }
}
