using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Hensel multivariate lifting utilities.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselMultUtil
/// </remarks>
public class HenselMultUtil
{
    private static readonly bool debug = false;

    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A, GenPolynomial<MOD> B, GenPolynomial<MOD> C, 
        List<MOD> V, long d, long k) 
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static HenselApprox<MOD> LiftHenselQuadratic<MOD>(
        GenPolynomial<Arith.BigInteger> C, Arith.BigInteger M,
        GenPolynomial<MOD> A, GenPolynomial<MOD> B,
        List<MOD> V, long d, long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftHenselQuadraticFac<MOD>(
        GenPolynomial<Arith.BigInteger> C, Arith.BigInteger M,
        List<GenPolynomial<MOD>> Af, List<MOD> V, long d, long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }
}
