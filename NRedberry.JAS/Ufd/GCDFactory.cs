using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms factory.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GCDFactory
/// </remarks>
public class GCDFactory
{
    public static GreatestCommonDivisorAbstract<C> GetProxy<C>(RingFactory<C> fac) where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GreatestCommonDivisorAbstract<C> GetImplementation<C>(RingFactory<C> fac) where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GreatestCommonDivisorAbstract<ModInteger> GetImplementation(ModIntegerRing fac)
    {
        throw new NotImplementedException();
    }

    public static GreatestCommonDivisorAbstract<ModLong> GetImplementation(ModLongRing fac)
    {
        throw new NotImplementedException();
    }

    public static GreatestCommonDivisorAbstract<Arith.BigInteger> GetImplementation(Arith.BigInteger fac)
    {
        throw new NotImplementedException();
    }

    public static GreatestCommonDivisorAbstract<BigRational> GetImplementation(BigRational fac)
    {
        throw new NotImplementedException();
    }
}
