using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with coefficients in <typeparamref name="C"/> into one with algebraic number coefficients.
    /// </summary>
    /// <typeparam name="C">Coefficient type (e.g., <c>ModInteger</c> or <c>BigRational</c>).</typeparam>
    /// <param name="ring">Target polynomial ring over algebraic numbers.</param>
    /// <param name="polynomial">Polynomial with coefficients of type <typeparamref name="C"/>.</param>
    /// <returns>Polynomial with coefficients lifted to <see cref="AlgebraicNumber{T}"/>.</returns>
    /// <remarks>Original Java method: PolyUtil#convertToAlgebraicCoefficients.</remarks>
    public static GenPolynomial<AlgebraicNumber<C>> ConvertToAlgebraicCoefficients<C>(GenPolynomialRing<AlgebraicNumber<C>> ring, GenPolynomial<C> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        AlgebraicNumberRing<C> algebraicRing = (AlgebraicNumberRing<C>)ring.CoFac;
        return Map(ring, polynomial, new CoeffToAlg<C>(algebraicRing));
    }
}
