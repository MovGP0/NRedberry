using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Integer coefficients factorization algorithms. This class implements factorization methods
/// for polynomials over integers.
/// </summary>
/// <typeparam name="MOD">modular coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorInteger
/// </remarks>
public class FactorInteger<MOD> : FactorAbstract<BigInteger> where MOD : GcdRingElem<MOD>, Modular
{
    private readonly bool debug = false;
    protected readonly FactorAbstract<MOD> mfactor;
    protected readonly GreatestCommonDivisorAbstract<MOD> mengine;

    public FactorInteger() : this(BigInteger.One)
    {
    }

    public FactorInteger(RingFactory<BigInteger> cfac) : base(cfac)
    {
        throw new NotImplementedException();
    }

    public override List<GenPolynomial<BigInteger>> BaseFactorsSquarefree(GenPolynomial<BigInteger> P)
    {
        throw new NotImplementedException();
    }

    public System.Collections.BitArray FactorDegrees(List<ExpVector> E, int deg)
    {
        throw new NotImplementedException();
    }

    public static long DegreeSum<C>(List<GenPolynomial<C>> L) where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public override List<GenPolynomial<BigInteger>> FactorsSquarefree(GenPolynomial<BigInteger> P)
    {
        throw new NotImplementedException();
    }

    public List<GenPolynomial<BigInteger>> FactorsSquarefreeHensel(GenPolynomial<BigInteger> P)
    {
        throw new NotImplementedException();
    }
}
