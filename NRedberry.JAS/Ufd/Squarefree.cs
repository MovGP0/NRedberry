using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition interface.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.Squarefree
/// </remarks>
public interface Squarefree<C> where C : GcdRingElem<C>
{
    GenPolynomial<C> SquarefreePart(GenPolynomial<C> P);

    bool IsSquarefree(GenPolynomial<C> P);

    SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> P);
}
