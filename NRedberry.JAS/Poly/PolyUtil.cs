using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Polynomial utilities, conversion between different representations and properties of polynomials.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolyUtil
/// </remarks>
public class PolyUtil
{
    public static GenPolynomial<GenPolynomial<C>> Recursive<C>(GenPolynomialRing<GenPolynomial<C>> rfac, GenPolynomial<C> p)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<C> Distribute<C>(GenPolynomialRing<C> dfac, GenPolynomial<GenPolynomial<C>> p)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<GenPolynomial<C>>> Recursive<C>(GenPolynomialRing<GenPolynomial<C>> rfac, List<GenPolynomial<C>> L)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<BigInteger> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> fac, GenPolynomial<C> A)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<BigInteger>> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> fac, List<GenPolynomial<C>> L)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<BigInteger> IntegerFromRationalCoefficients(GenPolynomialRing<BigInteger> fac, GenPolynomial<BigRational> A)
    {
        throw new NotImplementedException();
    }

    public static object[] IntegerFromRationalCoefficientsFactor(GenPolynomialRing<BigInteger> fac, GenPolynomial<BigRational> A)
    {
        throw new NotImplementedException();
    }

    public static C EvaluateMain<C>(RingFactory<C> cfac, GenPolynomial<C> A, C a)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<C> BaseDeriviative<C>(GenPolynomial<C> p)
        where C : RingElem<C>
    {
        throw new NotImplementedException();
    }
}
