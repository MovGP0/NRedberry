using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFieldCharP
/// </remarks>
public abstract class SquarefreeFieldCharP<C> : SquarefreeAbstract<C> where C : GcdRingElem<C>
{
    protected SquarefreeFieldCharP(GreatestCommonDivisorAbstract<C>? engine = null)
        : base(engine)
    {
    }

    public abstract GenPolynomial<C> BaseSquarefreePRoot(GenPolynomial<C> P);

    public override GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }
}
