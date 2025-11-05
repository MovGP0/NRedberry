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
public static class FactorFactory
{
    public static FactorAbstract<ModInteger> GetImplementation(ModIntegerRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorModular<ModInteger>(fac);
    }

    public static FactorAbstract<C> GetImplementation<C>(RingFactory<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);

        if (fac is BigInteger)
        {
            return (FactorAbstract<C>)(object)new FactorInteger<ModLong>();
        }

        if (fac is BigRational)
        {
            return (FactorAbstract<C>)(object)new FactorRational();
        }

        if (fac is ModIntegerRing modIntegerRing)
        {
            return (FactorAbstract<C>)(object)new FactorModular<ModInteger>(modIntegerRing);
        }

        if (fac is ModLongRing modLongRing)
        {
            return (FactorAbstract<C>)(object)new FactorModular<ModLong>(modLongRing);
        }

        Type factoryType = fac.GetType();
        if (IsGenericInstanceOf(factoryType, typeof(ComplexRing<>)))
        {
            Type innerType = factoryType.GetGenericArguments()[0];
            Type factorType = typeof(FactorComplex<>).MakeGenericType(innerType);
            object instance = Activator.CreateInstance(factorType, fac)
                ?? throw new InvalidOperationException($"Unable to instantiate {factorType.FullName}.");
            return (FactorAbstract<C>)instance;
        }

        if (IsGenericInstanceOf(factoryType, typeof(AlgebraicNumberRing<>)))
        {
            Type innerType = factoryType.GetGenericArguments()[0];
            Type factorType = typeof(FactorAlgebraic<>).MakeGenericType(innerType);
            object instance = Activator.CreateInstance(factorType, fac)
                ?? throw new InvalidOperationException($"Unable to instantiate {factorType.FullName}.");
            return (FactorAbstract<C>)instance;
        }

        if (IsGenericInstanceOf(factoryType, typeof(QuotientRing<>)))
        {
            Type innerType = factoryType.GetGenericArguments()[0];
            Type factorType = typeof(FactorQuotient<>).MakeGenericType(innerType);
            object instance = Activator.CreateInstance(factorType, fac)
                ?? throw new InvalidOperationException($"Unable to instantiate {factorType.FullName}.");
            return (FactorAbstract<C>)instance;
        }

        if (IsGenericInstanceOf(factoryType, typeof(GenPolynomialRing<>)))
        {
            return GetImplementation((dynamic)fac);
        }

        throw new ArgumentException(
            $"No factorization implementation for {factoryType.FullName}.",
            nameof(fac));
    }

    public static FactorAbstract<ModLong> GetImplementation(ModLongRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorModular<ModLong>(fac);
    }

    public static FactorAbstract<Arith.BigInteger> GetImplementation(Arith.BigInteger fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorInteger<ModLong>();
    }

    public static FactorAbstract<BigRational> GetImplementation(BigRational fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorRational();
    }

    public static FactorAbstract<AlgebraicNumber<C>> GetImplementation<C>(AlgebraicNumberRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorAlgebraic<C>(fac);
    }

    public static FactorAbstract<Complex<C>> GetImplementation<C>(ComplexRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorComplex<C>(fac);
    }

    public static FactorAbstract<Quotient<C>> GetImplementation<C>(QuotientRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new FactorQuotient<C>(fac);
    }

    public static FactorAbstract<C> GetImplementation<C>(GenPolynomialRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return GetImplementation(fac.CoFac);
    }

    private static bool IsGenericInstanceOf(Type candidate, Type genericDefinition)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        ArgumentNullException.ThrowIfNull(genericDefinition);
        return candidate.IsGenericType && candidate.GetGenericTypeDefinition() == genericDefinition;
    }
}
