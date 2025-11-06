using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
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
