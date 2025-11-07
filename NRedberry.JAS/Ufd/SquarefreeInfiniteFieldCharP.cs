using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for infinite fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeInfiniteFieldCharP
/// </remarks>
public class SquarefreeInfiniteFieldCharP<C> : SquarefreeFieldCharP<C> where C : GcdRingElem<C>
{
    protected readonly Quotient<C> qone;
    protected readonly Quotient<C> qzero;

    public SquarefreeInfiniteFieldCharP(GreatestCommonDivisorAbstract<C>? engine = null)
        : base(engine)
    {
        qone = null!;
        qzero = null!;
    }

    public override GenPolynomial<C> BaseSquarefreePRoot(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<GenPolynomial<C>> RecursiveUnivariateSquarefreePart(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<GenPolynomial<GenPolynomial<C>>, long> RecursiveUnivariateSquarefreeFactors(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<C> SquarefreePart(GenPolynomial<C> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<C, long> SquarefreeFactors(C coefficient)
    {
        throw new NotImplementedException();
    }
}
