using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<D> Map<C, D>(GenPolynomialRing<D> ring, GenPolynomial<C> polynomial, UnaryFunctor<C, D> functor)
        where C : RingElem<C>
        where D : RingElem<D>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(functor);

        GenPolynomial<D> result = new (ring);
        SortedDictionary<ExpVector, D> destination = result.Terms;
        foreach (Monomial<C> monomial in polynomial)
        {
            D mapped = functor.Eval(monomial.C);
            if (mapped is not null && !mapped.IsZero())
            {
                destination[monomial.E] = mapped;
            }
        }

        return result;
    }
}
