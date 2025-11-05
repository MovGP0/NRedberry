using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Hensel utilities for unique factorization domains.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselUtil
/// </remarks>
public static class HenselUtil
{
    private const bool Debug = false;

    public static HenselApprox<MOD> LiftHenselQuadratic<MOD>(
        GenPolynomial<BigInteger> C,
        BigInteger M,
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> S,
        GenPolynomial<MOD> T)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static HenselApprox<MOD> LiftHenselQuadratic<MOD>(
        GenPolynomial<BigInteger> C,
        BigInteger M,
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<MOD>[] LiftExtendedEuclidean<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftExtendedEuclidean<MOD>(
        List<GenPolynomial<MOD>> A,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> C,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        List<GenPolynomial<MOD>> A,
        GenPolynomial<MOD> C,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        long exponent,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        List<GenPolynomial<MOD>> A,
        long exponent,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static bool IsDiophantLift<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> S1,
        GenPolynomial<MOD> S2,
        GenPolynomial<MOD> C)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }

    public static bool IsDiophantLift<MOD>(
        List<GenPolynomial<MOD>> A,
        List<GenPolynomial<MOD>> S,
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

    public static List<GenPolynomial<MOD>> LiftHenselMonic<MOD>(
        GenPolynomial<BigInteger> C,
        List<GenPolynomial<MOD>> F,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        throw new NotImplementedException();
    }
}
