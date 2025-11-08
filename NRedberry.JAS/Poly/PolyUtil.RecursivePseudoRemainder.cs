using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Computes the pseudo remainder of two recursive polynomials.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Dividend polynomial.</param>
    /// <param name="divisor">Divisor polynomial.</param>
    /// <returns>The pseudo remainder.</returns>
    /// <remarks>Original Java method: PolyUtil#recursivePseudoRemainder.</remarks>
    public static GenPolynomial<GenPolynomial<C>> RecursivePseudoRemainder<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
        where C : RingElem<C>
    {
        return RecursiveSparsePseudoRemainder(polynomial, divisor);
    }
}
