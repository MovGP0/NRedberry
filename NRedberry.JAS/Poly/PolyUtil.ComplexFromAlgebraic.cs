using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with algebraic number coefficients (ℚ(i)) to one with complex coefficients.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="ring">Target polynomial ring over complex numbers.</param>
    /// <param name="polynomial">Polynomial with algebraic number coefficients.</param>
    /// <returns>Polynomial with complex coefficients.</returns>
    /// <remarks>Original Java method: PolyUtil#complexFromAlgebraic.</remarks>
    public static GenPolynomial<Complex<C>> ComplexFromAlgebraic<C>(GenPolynomialRing<Complex<C>> ring, GenPolynomial<AlgebraicNumber<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        ComplexRing<C> complexRing = (ComplexRing<C>)ring.CoFac;
        return Map(ring, polynomial, new AlgebToCompl<C>(complexRing));
    }
}
