using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Algebraic number coefficients factorization algorithms.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorAlgebraic
/// </remarks>
public class FactorAlgebraic<C> : FactorAbsolute<AlgebraicNumber<C>> where C : GcdRingElem<C>
{
    protected readonly FactorAbstract<C> factorCoeff;

    public FactorAlgebraic(AlgebraicNumberRing<C> fac) : base(fac)
    {
        throw new NotImplementedException();
    }

    public FactorAlgebraic(AlgebraicNumberRing<C> fac, FactorAbstract<C> factorCoeff) : base(fac)
    {
        this.factorCoeff = factorCoeff;
    }

    public override List<GenPolynomial<AlgebraicNumber<C>>> BaseFactorsSquarefree(GenPolynomial<AlgebraicNumber<C>> P)
    {
        throw new NotImplementedException();
    }
}
