using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Modular coefficients factorization algorithms.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorModular
/// </remarks>
public class FactorModular<MOD> : FactorAbsolute<MOD> where MOD : GcdRingElem<MOD>, Modular
{
    public FactorModular(RingFactory<MOD> cfac) : base(cfac)
    {
    }

    public SortedDictionary<long, GenPolynomial<MOD>> BaseDistinctDegreeFactors(GenPolynomial<MOD> P)
    {
        throw new NotImplementedException();
    }

    public List<GenPolynomial<MOD>> BaseEqualDegreeFactors(GenPolynomial<MOD> P, long deg)
    {
        throw new NotImplementedException();
    }

    public override List<GenPolynomial<MOD>> BaseFactorsSquarefree(GenPolynomial<MOD> P)
    {
        throw new NotImplementedException();
    }
}
