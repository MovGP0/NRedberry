using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class GCDFactoryTests
{
    [Fact]
    public void ShouldReturnExpectedImplementationsForScalarFactories()
    {
        GCDFactory.GetImplementation(new JasBigInteger()).ShouldBeOfType<GreatestCommonDivisorModular<ModLong>>();
        GCDFactory.GetImplementation(new BigRational()).ShouldBeOfType<GreatestCommonDivisorPrimitive<BigRational>>();
        GCDFactory.GetImplementation(new ModIntegerRing(new JasBigInteger(5), true)).ShouldBeOfType<GreatestCommonDivisorModEval<ModInteger>>();
        GCDFactory.GetImplementation(new ModLongRing(9, false)).ShouldBeOfType<GreatestCommonDivisorSubres<ModLong>>();
    }

    [Fact]
    public void ShouldReturnExpectedImplementationsForGenericAndProxyFactories()
    {
        ComplexRing<BigRational> complexRing = new(new BigRational());
        RingFactory<Complex<BigRational>> complexFactory = complexRing;

        GCDFactory.GetImplementation(complexFactory).ShouldBeOfType<GreatestCommonDivisorSimple<Complex<BigRational>>>();
        GCDFactory.GetProxy(complexFactory).ShouldBeOfType<GreatestCommonDivisorSimple<Complex<BigRational>>>();
    }
}
