using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for (commutative) rings of characteristic 0.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeRingChar0
/// </remarks>
public class SquarefreeRingChar0<C> : SquarefreeAbstract<C> where C : GcdRingElem<C>
{
    public SquarefreeRingChar0(GreatestCommonDivisorAbstract<C>? engine = null)
        : base(engine)
    {
    }

    public override GenPolynomial<C> BaseSquarefreePart(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<GenPolynomial<C>, long> BaseSquarefreeFactors(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }
}
