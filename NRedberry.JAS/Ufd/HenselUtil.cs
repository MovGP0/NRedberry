using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Hensel utilities for ufd.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselUtil
/// </remarks>
public class HenselUtil
{
    public static HenselApprox<MOD> LiftHenselQuadratic<MOD>(
        GenPolynomial<Arith.BigInteger> C,
        Arith.BigInteger M,
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> S,
        GenPolynomial<MOD> T)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A, GenPolynomial<MOD> B, GenPolynomial<MOD> C, long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static bool IsDiophantLift<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> S,
        GenPolynomial<MOD> T,
        GenPolynomial<MOD> C)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> DiophantQuadraticLift<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> C,
        GenPolynomial<MOD> S,
        GenPolynomial<MOD> T,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftExtendedEuclidean<MOD>(
        GenPolynomial<MOD> A, GenPolynomial<MOD> B, long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }
}
