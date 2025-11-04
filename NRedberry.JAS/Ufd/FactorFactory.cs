using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Factorization algorithms factory. Select appropriate factorization engine based on the coefficient types.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorFactory
/// </remarks>
public class FactorFactory
{
    public static FactorAbstract<ModInteger> GetImplementation(ModIntegerRing fac)
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<C> GetImplementation<C>(RingFactory<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<ModLong> GetImplementation(ModLongRing fac)
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<Arith.BigInteger> GetImplementation(Arith.BigInteger fac)
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<BigRational> GetImplementation(BigRational fac)
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<AlgebraicNumber<C>> GetImplementation<C>(AlgebraicNumberRing<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<Complex<C>> GetImplementation<C>(ComplexRing<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<Quotient<C>> GetImplementation<C>(QuotientRing<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static FactorAbstract<C> GetImplementation<C>(GenPolynomialRing<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }
}
