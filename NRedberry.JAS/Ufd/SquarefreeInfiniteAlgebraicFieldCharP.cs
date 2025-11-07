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
    public SquarefreeInfiniteAlgebraicFieldCharP(GreatestCommonDivisorAbstract<AlgebraicNumber<C>>? engine = null)
        : base(engine)
    {
    }

    public override GenPolynomial<AlgebraicNumber<C>> BaseSquarefreePRoot(GenPolynomial<AlgebraicNumber<C>> P)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> RecursiveUnivariateSquarefreePart(GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<GenPolynomial<GenPolynomial<AlgebraicNumber<C>>>, long> RecursiveUnivariateSquarefreeFactors(GenPolynomial<GenPolynomial<AlgebraicNumber<C>>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<AlgebraicNumber<C>> SquarefreePart(GenPolynomial<AlgebraicNumber<C>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<GenPolynomial<AlgebraicNumber<C>>, long> SquarefreeFactors(GenPolynomial<AlgebraicNumber<C>> polynomial)
    {
        throw new NotImplementedException();
    }

    public override SortedDictionary<AlgebraicNumber<C>, long> SquarefreeFactors(AlgebraicNumber<C> coefficient)
    {
        throw new NotImplementedException();
    }
}
