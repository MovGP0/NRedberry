using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for infinite algebraic fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeInfiniteAlgebraicFieldCharP
/// </remarks>
public class SquarefreeInfiniteAlgebraicFieldCharP<C> : SquarefreeFieldCharP<AlgebraicNumber<C>>
    where C : GcdRingElem<C>
{
    public SquarefreeInfiniteAlgebraicFieldCharP(RingFactory<AlgebraicNumber<C>> fac)
        : base(fac)
    {
    }

    public override GenPolynomial<AlgebraicNumber<C>>? BaseSquarefreePRoot(GenPolynomial<AlgebraicNumber<C>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<GenPolynomial<AlgebraicNumber<C>>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<AlgebraicNumber<C>, long> SquarefreeFactors(AlgebraicNumber<C> coefficient)
    {
        throw new NotImplementedException();
    }
}
