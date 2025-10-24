using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Term order optimization. For example computation of optimal permutations of variables in polynomials.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.TermOrderOptimization
/// </remarks>
public class TermOrderOptimization
{
    public static List<GenPolynomial<BigInteger>> ExpVectorAdd(List<GenPolynomial<BigInteger>> dm, ExpVector e)
    {
        throw new NotImplementedException();
    }

    public static List<int> OptimalPermutation(List<GenPolynomial<BigInteger>> D)
    {
        throw new NotImplementedException();
    }

    public static List<int> InversePermutation(List<int> P)
    {
        throw new NotImplementedException();
    }

    public static string[] StringArrayPermutation(List<int> P, string[] a)
    {
        throw new NotImplementedException();
    }

    public static long[] LongArrayPermutation(List<int> P, long[] a)
    {
        throw new NotImplementedException();
    }

    public static ExpVector Permutation(List<int> P, ExpVector e)
    {
        throw new NotImplementedException();
    }
}
