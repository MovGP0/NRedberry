using System.Linq;
using System.Reflection;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree factorization algorithms factory. Select appropriate squarefree
/// factorization engine based on the coefficient types.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFactory
/// </remarks>
public static class SquarefreeFactory
{
    private static readonly MethodInfo AlgebraicNumberRingImplementationMethod = GetGenericFactoryMethod(typeof(AlgebraicNumberRing<>));
    private static readonly MethodInfo QuotientRingImplementationMethod = GetGenericFactoryMethod(typeof(QuotientRing<>));
    private static readonly MethodInfo ComplexRingImplementationMethod = GetGenericFactoryMethod(typeof(ComplexRing<>));

    public static SquarefreeAbstract<C> GetImplementation<C>(RingFactory<C> fac) where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);

        if (fac is ModIntegerRing modIntegerRing)
        {
            return (SquarefreeAbstract<C>)(object)GetImplementation(modIntegerRing);
        }

        if (fac is ModLongRing modLongRing)
        {
            return (SquarefreeAbstract<C>)(object)GetImplementation(modLongRing);
        }

        if (fac is Arith.BigInteger bigInteger)
        {
            return (SquarefreeAbstract<C>)(object)GetImplementation(bigInteger);
        }

        if (fac is BigRational bigRational)
        {
            return (SquarefreeAbstract<C>)(object)GetImplementation(bigRational);
        }

        Type factoryType = fac.GetType();
        if (TryInvokeGenericFactory(fac, factoryType, typeof(AlgebraicNumberRing<>), typeof(AlgebraicNumber<>), AlgebraicNumberRingImplementationMethod, out SquarefreeAbstract<C> algebraicResult))
        {
            return algebraicResult;
        }

        if (TryInvokeGenericFactory(fac, factoryType, typeof(QuotientRing<>), typeof(Quotient<>), QuotientRingImplementationMethod, out SquarefreeAbstract<C> quotientResult))
        {
            return quotientResult;
        }

        if (fac is GenPolynomialRing<C> polynomialRing)
        {
            return GetImplementation(polynomialRing);
        }

        if (TryInvokeGenericFactory(fac, factoryType, typeof(ComplexRing<>), typeof(Complex<>), ComplexRingImplementationMethod, out SquarefreeAbstract<C> complexResult))
        {
            return complexResult;
        }

        if (fac.IsField())
        {
            if (fac.Characteristic().Sign == 0)
            {
                return new SquarefreeFieldChar0<C>(fac);
            }

            if (fac.IsFinite())
            {
                return new SquarefreeFiniteFieldCharP<C>(fac);
            }

            throw new NotSupportedException($"Squarefree factorization for infinite positive characteristic fields is not implemented for {fac.GetType().Name}.");
        }

        if (fac.Characteristic().Sign == 0)
        {
            return new SquarefreeRingChar0<C>(fac);
        }

        throw new ArgumentException($"No squarefree factorization implementation for {fac.GetType().Name}.", nameof(fac));
    }

    public static SquarefreeAbstract<ModInteger> GetImplementation(ModIntegerRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new SquarefreeFiniteFieldCharP<ModInteger>(fac);
    }

    public static SquarefreeAbstract<ModLong> GetImplementation(ModLongRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new SquarefreeFiniteFieldCharP<ModLong>(fac);
    }

    public static SquarefreeAbstract<Arith.BigInteger> GetImplementation(Arith.BigInteger fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new SquarefreeRingChar0<Arith.BigInteger>(fac);
    }

    public static SquarefreeAbstract<BigRational> GetImplementation(BigRational fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new SquarefreeFieldChar0<BigRational>(fac);
    }

    public static SquarefreeAbstract<AlgebraicNumber<C>> GetImplementation<C>(AlgebraicNumberRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        PolyUfdUtil.EnsureFieldProperty(fac);
        if (!fac.IsField())
        {
            throw new ArithmeticException($"eventually no integral domain {fac.GetType().Name}");
        }

        if (fac.Characteristic().Sign == 0)
        {
            return new SquarefreeFieldChar0<AlgebraicNumber<C>>(fac);
        }

        if (fac.IsFinite())
        {
            return new SquarefreeFiniteFieldCharP<AlgebraicNumber<C>>(fac);
        }

        return new SquarefreeInfiniteAlgebraicFieldCharP<C>(fac);
    }

    public static SquarefreeAbstract<Quotient<C>> GetImplementation<C>(QuotientRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return fac.Characteristic().Sign == 0
            ? new SquarefreeFieldChar0<Quotient<C>>(fac)
            : new SquarefreeInfiniteFieldCharP<C>(fac);
    }

    public static SquarefreeAbstract<C> GetImplementation<C>(GenPolynomialRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return GetImplementation(fac.CoFac);
    }

    public static SquarefreeAbstract<Complex<C>> GetImplementation<C>(ComplexRing<C> fac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        return new SquarefreeFieldChar0<Complex<C>>(fac);
    }

    private static bool TryInvokeGenericFactory<C>(
        RingFactory<C> fac,
        Type factoryType,
        Type expectedFactoryDefinition,
        Type expectedElementDefinition,
        MethodInfo implementationMethod,
        out SquarefreeAbstract<C>? implementation)
        where C : GcdRingElem<C>
    {
        if (!factoryType.IsGenericType || factoryType.GetGenericTypeDefinition() != expectedFactoryDefinition)
        {
            implementation = null;
            return false;
        }

        if (!typeof(C).IsGenericType || typeof(C).GetGenericTypeDefinition() != expectedElementDefinition)
        {
            implementation = null;
            return false;
        }

        Type innerFactoryType = factoryType.GetGenericArguments()[0];
        Type innerElementType = typeof(C).GetGenericArguments()[0];
        if (innerFactoryType != innerElementType)
        {
            implementation = null;
            return false;
        }

        MethodInfo closedMethod = implementationMethod.MakeGenericMethod(innerFactoryType);
        object? result = closedMethod.Invoke(null, [fac]);
        implementation = (SquarefreeAbstract<C>)result!;
        return true;
    }

    private static MethodInfo GetGenericFactoryMethod(Type parameterDefinition)
    {
        return typeof(SquarefreeFactory)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.IsGenericMethodDefinition
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType.IsGenericType
                && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == parameterDefinition);
    }
}
