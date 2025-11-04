using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for finite fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFiniteFieldCharP
/// </remarks>
public class SquarefreeFiniteFieldCharP<C> : SquarefreeFieldCharP<C> where C : GcdRingElem<C>
{
    public SquarefreeFiniteFieldCharP(GreatestCommonDivisorAbstract<C>? engine = null)
        : base(engine)
    {
    }

    public override GenPolynomial<C> BaseSquarefreePRoot(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }
}
