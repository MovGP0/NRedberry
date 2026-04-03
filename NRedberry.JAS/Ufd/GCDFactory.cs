using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms factory. Select appropriate GCD engine
/// based on the coefficient types.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GCDFactory
/// </remarks>
public static class GCDFactory
{
    public static GreatestCommonDivisorAbstract<ModLong> GetImplementation(ModLongRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return fac.IsField()
            ? new GreatestCommonDivisorModEval<ModLong>()
            : new GreatestCommonDivisorSubres<ModLong>();
    }

    public static GreatestCommonDivisorAbstract<ModInteger> GetImplementation(ModIntegerRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return fac.IsField()
            ? new GreatestCommonDivisorModEval<ModInteger>()
            : new GreatestCommonDivisorSubres<ModInteger>();
    }

    public static GreatestCommonDivisorAbstract<BigInteger> GetImplementation(BigInteger fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new GreatestCommonDivisorModular<ModLong>();
    }

    public static GreatestCommonDivisorAbstract<BigRational> GetImplementation(BigRational fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new GreatestCommonDivisorPrimitive<BigRational>();
    }

    public static GreatestCommonDivisorAbstract<C> GetImplementation<C>(RingFactory<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);

        return fac switch
        {
            BigInteger bigIntegerFactory => (GreatestCommonDivisorAbstract<C>)(object)GetImplementation(bigIntegerFactory),
            ModIntegerRing modIntegerRing => (GreatestCommonDivisorAbstract<C>)(object)GetImplementation(modIntegerRing),
            ModLongRing modLongRing => (GreatestCommonDivisorAbstract<C>)(object)GetImplementation(modLongRing),
            BigRational bigRationalFactory => (GreatestCommonDivisorAbstract<C>)(object)GetImplementation(bigRationalFactory),
            _ => fac.IsField()
                ? new GreatestCommonDivisorSimple<C>()
                : new GreatestCommonDivisorSubres<C>()
        };
    }

    public static GreatestCommonDivisorAbstract<C> GetProxy<C>(RingFactory<C> fac)
        where C : GcdRingElem<C>
    {
        return GetImplementation(fac);
    }
}
