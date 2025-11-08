using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Retrieves the trailing (lowest-degree) coefficient of a univariate polynomial.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Polynomial whose trailing coefficient is required.</param>
    /// <returns>The trailing coefficient or zero when the polynomial has no terms.</returns>
    private static C GetTrailingCoefficient<C>(GenPolynomial<C> polynomial)
        where C : RingElem<C>
    {
        if (polynomial.Terms.Count == 0)
        {
            return polynomial.Ring.CoFac.FromInteger(0);
        }

        return polynomial.Terms.Last().Value;
    }
}
