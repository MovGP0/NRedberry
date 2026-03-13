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
        Assert.IsType<GreatestCommonDivisorModular<ModLong>>(GCDFactory.GetImplementation(new JasBigInteger()));
        Assert.IsType<GreatestCommonDivisorPrimitive<BigRational>>(GCDFactory.GetImplementation(new BigRational()));
        Assert.IsType<GreatestCommonDivisorModEval<ModInteger>>(GCDFactory.GetImplementation(new ModIntegerRing(new JasBigInteger(5), true)));
        Assert.IsType<GreatestCommonDivisorSubres<ModLong>>(GCDFactory.GetImplementation(new ModLongRing(9, false)));
    }

    [Fact]
    public void ShouldReturnExpectedImplementationsForGenericAndProxyFactories()
    {
        ComplexRing<BigRational> complexRing = new(new BigRational());
        RingFactory<Complex<BigRational>> complexFactory = complexRing;

        Assert.IsType<GreatestCommonDivisorSimple<Complex<BigRational>>>(GCDFactory.GetImplementation(complexFactory));
        Assert.IsType<GreatestCommonDivisorSimple<Complex<BigRational>>>(GCDFactory.GetProxy(complexFactory));
    }
}
