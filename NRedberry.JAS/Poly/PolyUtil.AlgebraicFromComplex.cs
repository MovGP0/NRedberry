using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with complex coefficients into one with algebraic number coefficients over ℚ(i).
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="ring">Target polynomial ring over algebraic numbers.</param>
    /// <param name="polynomial">Polynomial with complex coefficients to convert.</param>
    /// <returns>Polynomial with algebraic number coefficients.</returns>
    /// <remarks>Original Java method: PolyUtil#algebraicFromComplex.</remarks>
    public static GenPolynomial<AlgebraicNumber<C>> AlgebraicFromComplex<C>(GenPolynomialRing<AlgebraicNumber<C>> ring, GenPolynomial<Complex<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        AlgebraicNumberRing<C> algebraicRing = (AlgebraicNumberRing<C>)ring.CoFac;
        return Map(ring, polynomial, new ComplToAlgeb<C>(algebraicRing));
    }
}
