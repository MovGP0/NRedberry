using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Extracts the leading exponent vectors from a list of polynomials.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomials">Polynomials whose leading exponents are requested.</param>
    /// <returns>List of leading exponent vectors (or <see langword="null"/> when the input list is null).</returns>
    /// <remarks>Original Java method: PolyUtil#leadingExpVector(List).</remarks>
    public static List<ExpVector?>? LeadingExpVector<C>(List<GenPolynomial<C>>? polynomials)
        where C : RingElem<C>
    {
        if (polynomials is null)
        {
            return null;
        }

        List<ExpVector?> result = new (polynomials.Count);
        foreach (GenPolynomial<C>? polynomial in polynomials)
        {
            result.Add(polynomial?.LeadingExpVector());
        }

        return result;
    }
}
