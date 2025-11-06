using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<Complex<C>> ComplexFromAlgebraic<C>(GenPolynomialRing<Complex<C>> ring, GenPolynomial<AlgebraicNumber<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        ComplexRing<C> complexRing = (ComplexRing<C>)ring.CoFac;
        return Map(ring, polynomial, new AlgebToCompl<C>(complexRing));
    }
}
