using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with algebraic number coefficients into one whose coefficients are polynomials over <typeparamref name="C"/>.
    /// </summary>
    /// <typeparam name="C">Coefficient type of the embedded algebraic numbers.</typeparam>
    /// <param name="ring">Target polynomial ring with polynomial coefficients.</param>
    /// <param name="polynomial">Polynomial with algebraic number coefficients.</param>
    /// <returns>Polynomial whose coefficients are of type <c>GenPolynomial&lt;C&gt;</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#fromAlgebraicCoefficients.</remarks>
    public static GenPolynomial<GenPolynomial<C>> FromAlgebraicCoefficients<C>(GenPolynomialRing<GenPolynomial<C>> ring, GenPolynomial<AlgebraicNumber<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        return Map(ring, polynomial, new AlgToPoly<C>());
    }
}
