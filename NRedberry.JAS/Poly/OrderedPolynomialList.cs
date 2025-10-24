using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// List of ordered polynomials. Mainly for storage and printing.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.OrderedPolynomialList
/// </remarks>
public class OrderedPolynomialList<C> : PolynomialList<C>, IComparer<GenPolynomial<C>> where C : RingElem<C>
{
    public OrderedPolynomialList(GenPolynomialRing<C> r, List<GenPolynomial<C>> l)
        : base(r, l)
    {
    }

    public override bool Equals(object? p)
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<C>> Sort(GenPolynomialRing<C> r, List<GenPolynomial<C>> l)
    {
        throw new NotImplementedException();
    }

    public int Compare(GenPolynomial<C>? p1, GenPolynomial<C>? p2)
    {
        throw new NotImplementedException();
    }
}
