using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Quotient coefficients factorization algorithms.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorQuotient
/// </remarks>
public class FactorQuotient<C> : FactorAbstract<Quotient<C>> where C : GcdRingElem<C>
{
    protected readonly FactorAbstract<C> nengine;

    public FactorQuotient(QuotientRing<C> fac) : base(fac)
    {
        throw new NotImplementedException();
    }

    public FactorQuotient(QuotientRing<C> fac, FactorAbstract<C> nengine) : base(fac)
    {
        this.nengine = nengine;
    }

    public override List<GenPolynomial<Quotient<C>>> BaseFactorsSquarefree(GenPolynomial<Quotient<C>> P)
    {
        throw new NotImplementedException();
    }

    public override List<GenPolynomial<Quotient<C>>> FactorsSquarefree(GenPolynomial<Quotient<C>> P)
    {
        throw new NotImplementedException();
    }
}
