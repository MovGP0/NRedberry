using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// List of polynomials with their polynomial ring factory.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolynomialList
/// </remarks>
public class PolynomialList<C> : IComparable<PolynomialList<C>> where C : RingElem<C>
{
    public readonly GenPolynomialRing<C> Ring;
    public readonly List<GenPolynomial<C>> List;

    public PolynomialList(GenPolynomialRing<C> r, List<GenPolynomial<C>> l)
    {
        Ring = r ?? throw new ArgumentNullException(nameof(r));
        List = l ?? throw new ArgumentNullException(nameof(l));
    }

    public PolynomialList<C> Copy()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? p)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(PolynomialList<C>? L)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
