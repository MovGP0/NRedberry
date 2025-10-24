using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree factorization algorithms factory.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFactory
/// </remarks>
public class SquarefreeFactory
{
    public static SquarefreeAbstract<C> GetImplementation<C>(RingFactory<C> fac) where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static SquarefreeAbstract<ModInteger> GetImplementation(ModIntegerRing fac)
    {
        throw new NotImplementedException();
    }

    public static SquarefreeAbstract<ModLong> GetImplementation(ModLongRing fac)
    {
        throw new NotImplementedException();
    }

    public static SquarefreeAbstract<Arith.BigInteger> GetImplementation(Arith.BigInteger fac)
    {
        throw new NotImplementedException();
    }

    public static SquarefreeAbstract<BigRational> GetImplementation(BigRational fac)
    {
        throw new NotImplementedException();
    }

    public static SquarefreeAbstract<AlgebraicNumber<C>> GetImplementation<C>(AlgebraicNumberRing<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static SquarefreeAbstract<Complex<C>> GetImplementation<C>(ComplexRing<C> fac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }
}
