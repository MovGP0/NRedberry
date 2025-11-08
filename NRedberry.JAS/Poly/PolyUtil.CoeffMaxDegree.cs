using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Returns the maximal degree occurring in the coefficient polynomials of a recursive polynomial.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Recursive polynomial.</param>
    /// <returns>Maximum coefficient degree (0 when the polynomial is zero).</returns>
    /// <remarks>Original Java method: PolyUtil#coeffMaxDegree.</remarks>
    public static long CoeffMaxDegree<C>(GenPolynomial<GenPolynomial<C>> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return 0;
        }

        long degree = 0;
        foreach (GenPolynomial<C> coefficient in polynomial.Terms.Values)
        {
            long current = coefficient.Degree();
            if (current > degree)
            {
                degree = current;
            }
        }

        return degree;
    }
}
