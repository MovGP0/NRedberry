using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Abstract squarefree decomposition class.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeAbstract
/// </remarks>
public abstract class SquarefreeAbstract<C> : Squarefree<C> where C : GcdRingElem<C>
{
    protected readonly GreatestCommonDivisorAbstract<C> engine;

    protected SquarefreeAbstract(GreatestCommonDivisorAbstract<C>? engine = null)
    {
        this.engine = engine ?? GCDFactory.GetProxy<C>(null!);
    }

    public abstract GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> P);

    public abstract SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> P);

    public virtual GenPolynomial<C> SquarefreePart(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsSquarefree(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public virtual SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public virtual GenPolynomial<C> RecursiveUnivariateSquarefreePart(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public virtual SortedDictionary<GenPolynomial<C>, long> RecursiveUnivariateSquarefreeFactors(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }
}
