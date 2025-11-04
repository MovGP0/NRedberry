using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with modular evaluation algorithm for recursion.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorModEval
/// </remarks>
public class GreatestCommonDivisorModEval<MOD> : GreatestCommonDivisorAbstract<MOD>
    where MOD : GcdRingElem<MOD>, Modular
{
    private readonly bool debug;
    protected readonly GreatestCommonDivisorAbstract<MOD> mufd;

    public GreatestCommonDivisorModEval()
    {
        mufd = new GreatestCommonDivisorSimple<MOD>();
    }

    public override GenPolynomial<MOD> BaseGcd(GenPolynomial<MOD> P, GenPolynomial<MOD> S)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<MOD> RecursiveUnivariateGcd(GenPolynomial<MOD> P, GenPolynomial<MOD> S)
    {
        throw new NotImplementedException();
    }
}
