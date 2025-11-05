using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<GenPolynomial<C>> Recursive<C>(GenPolynomialRing<GenPolynomial<C>> recursiveRing, GenPolynomial<C> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(recursiveRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomial<GenPolynomial<C>> result = GenPolynomialRing<GenPolynomial<C>>.Zero.Clone();
        if (polynomial.IsZero())
        {
            return result;
        }

        int split = recursiveRing.Nvar;
        GenPolynomial<C> zeroCoefficient = GenPolynomialRing<C>.Zero;

        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;
        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector exponent = term.Key;
            C coefficient = term.Value;

            ExpVector head = exponent.Contract(0, split);
            ExpVector tail = exponent.Contract(split, exponent.Length() - split);

            if (!resultTerms.TryGetValue(head, out GenPolynomial<C>? existing))
            {
                existing = zeroCoefficient;
            }

            resultTerms[head] = existing.Sum(coefficient, tail);
        }

        return result;
    }

    public static List<GenPolynomial<GenPolynomial<C>>> Recursive<C>(GenPolynomialRing<GenPolynomial<C>> recursiveRing, List<GenPolynomial<C>> polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(recursiveRing);
        ArgumentNullException.ThrowIfNull(polynomials);

        return polynomials.ConvertAll(p => Recursive(recursiveRing, p));
    }
}
