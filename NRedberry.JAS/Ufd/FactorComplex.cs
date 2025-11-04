using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Complex coefficients factorization algorithms.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorComplex
/// </remarks>
public class FactorComplex<C> : FactorAbsolute<Complex<C>> where C : GcdRingElem<C>
{
    public FactorComplex(ComplexRing<C> fac)
        : base(fac)
    {
    }

    public override List<GenPolynomial<Complex<C>>> BaseFactorsSquarefree(GenPolynomial<Complex<C>> P)
    {
        throw new NotImplementedException();
    }
}
