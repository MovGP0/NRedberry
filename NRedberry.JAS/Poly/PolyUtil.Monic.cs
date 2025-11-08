using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Normalizes a recursive polynomial so that its leading coefficient becomes 1 whenever that coefficient is invertible.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Recursive polynomial.</param>
    /// <returns>The monic polynomial, or the original polynomial when the leading coefficient is not invertible.</returns>
    /// <remarks>Original Java method: PolyUtil#monic (recursive overload).</remarks>
    public static GenPolynomial<GenPolynomial<C>>? Monic<C>(GenPolynomial<GenPolynomial<C>> polynomial)
        where C : RingElem<C>
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomial<C> leadingCoefficient = polynomial.LeadingBaseCoefficient();
        C head = leadingCoefficient.LeadingBaseCoefficient();
        if (!head.IsUnit())
        {
            return polynomial;
        }

        C inverse = head.Inverse();
        GenPolynomial<C> unit = polynomial.Ring.CoFac.FromInteger(1);
        GenPolynomial<C> multiplier = unit.Multiply(inverse);
        return polynomial.Multiply(multiplier);
    }

    /// <summary>
    /// Normalizes every polynomial in a list so their leading coefficients equal 1.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomials">List of polynomials with field coefficients.</param>
    /// <returns>A list of monic polynomials (or <see langword="null"/> if the input list is null).</returns>
    /// <remarks>Original Java method: PolyUtil#monic (list overload).</remarks>
    public static List<GenPolynomial<C>>? Monic<C>(List<GenPolynomial<C>>? polynomials)
        where C : RingElem<C>
    {
        if (polynomials is null)
        {
            return null;
        }

        List<GenPolynomial<C>> result = new (polynomials.Count);
        foreach (GenPolynomial<C>? polynomial in polynomials)
        {
            result.Add(polynomial is null ? null! : polynomial.Monic());
        }

        return result;
    }
}
