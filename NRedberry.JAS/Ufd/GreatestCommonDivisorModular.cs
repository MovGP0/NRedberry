using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with modular computation and Chinese remainder algorithm.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorModular
/// </remarks>
public class GreatestCommonDivisorModular<MOD> : GreatestCommonDivisorAbstract<Arith.BigInteger>
    where MOD : GcdRingElem<MOD>, Modular
{
    private readonly bool debug;
    protected readonly GreatestCommonDivisorAbstract<MOD> mufd;
    protected readonly GreatestCommonDivisorAbstract<Arith.BigInteger> iufd;

    public GreatestCommonDivisorModular()
        : this(false)
    {
    }

    public GreatestCommonDivisorModular(bool simple)
    {
        if (simple)
        {
            mufd = new GreatestCommonDivisorSimple<MOD>();
        }
        else
        {
            mufd = new GreatestCommonDivisorModEval<MOD>();
        }

        iufd = new GreatestCommonDivisorSubres<Arith.BigInteger>();
    }

    public override GenPolynomial<Arith.BigInteger> BaseGcd(GenPolynomial<Arith.BigInteger> P, GenPolynomial<Arith.BigInteger> S)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<Arith.BigInteger> RecursiveUnivariateGcd(GenPolynomial<Arith.BigInteger> P, GenPolynomial<Arith.BigInteger> S)
    {
        throw new NotImplementedException();
    }
}
