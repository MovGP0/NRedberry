using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
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
