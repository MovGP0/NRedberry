using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Evaluates a recursive polynomial at a value plugged into its first (lowest) variable.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="coefficientRing">Coefficient polynomial ring in the first variable.</param>
    /// <param name="destinationRing">Ring spanning the remaining variables.</param>
    /// <param name="polynomial">Recursive polynomial to evaluate.</param>
    /// <param name="value">Coefficient value substituted for the first variable.</param>
    /// <returns>The partially evaluated polynomial.</returns>
    /// <remarks>Original Java method: PolyUtil#evaluateFirstRec.</remarks>
    public static GenPolynomial<C> EvaluateFirstRec<C>(
        GenPolynomialRing<C> coefficientRing,
        GenPolynomialRing<C> destinationRing,
        GenPolynomial<GenPolynomial<C>> polynomial,
        C value)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(coefficientRing);
        ArgumentNullException.ThrowIfNull(destinationRing);
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(value);

        if (polynomial.IsZero())
        {
            return new GenPolynomial<C>(destinationRing);
        }

        GenPolynomial<C> result = new (destinationRing);
        SortedDictionary<ExpVector, C> resultTerms = result.Terms;
        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            C evaluated = EvaluateMain(coefficientRing.CoFac, term.Value, value);
            if (!evaluated.IsZero())
            {
                resultTerms[term.Key] = evaluated;
            }
        }

        return result;
    }
}
