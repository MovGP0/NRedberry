using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<GenPolynomial<C>> FromAlgebraicCoefficients<C>(GenPolynomialRing<GenPolynomial<C>> ring, GenPolynomial<AlgebraicNumber<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        return Map(ring, polynomial, new AlgToPoly<C>());
    }
}
