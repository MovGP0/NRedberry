using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
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
