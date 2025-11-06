using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<AlgebraicNumber<C>> ConvertToAlgebraicCoefficients<C>(GenPolynomialRing<AlgebraicNumber<C>> ring, GenPolynomial<C> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        AlgebraicNumberRing<C> algebraicRing = (AlgebraicNumberRing<C>)ring.CoFac;
        return Map(ring, polynomial, new CoeffToAlg<C>(algebraicRing));
    }
}
