using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class FactorFactoryTests
{
    [Fact]
    public void ShouldReturnExpectedImplementationsForScalarFactories()
    {
        FactorFactory.GetImplementation(new JasBigInteger()).ShouldBeOfType<FactorInteger<ModLong>>();
        FactorFactory.GetImplementation(new BigRational()).ShouldBeOfType<FactorRational>();
        FactorFactory.GetImplementation(new ModIntegerRing(new JasBigInteger(5), true)).ShouldBeOfType<FactorModular<ModInteger>>();
        FactorFactory.GetImplementation(new ModLongRing(5, true)).ShouldBeOfType<FactorModular<ModLong>>();
    }

    [Fact]
    public void ShouldReturnExpectedImplementationsForCompositeFactories()
    {
        GenPolynomialRing<BigRational> polynomialRing = CreateRationalPolynomialRing();
        ComplexRing<BigRational> complexRing = new(new BigRational());
        AlgebraicNumberRing<BigRational> algebraicRing = complexRing.AlgebraicRing();
        QuotientRing<BigRational> quotientRing = new(polynomialRing, true);
        RingFactory<BigRational> rationalFactory = new BigRational();
        RingFactory<Complex<BigRational>> complexFactory = complexRing;
        RingFactory<AlgebraicNumber<BigRational>> algebraicFactory = algebraicRing;
        RingFactory<Quotient<BigRational>> quotientFactory = quotientRing;

        FactorFactory.GetImplementation(rationalFactory).ShouldBeOfType<FactorRational>();
        FactorFactory.GetImplementation(polynomialRing).ShouldBeOfType<FactorRational>();
        FactorFactory.GetImplementation(complexFactory).ShouldBeOfType<FactorComplex<BigRational>>();
        FactorFactory.GetImplementation(algebraicFactory).ShouldBeOfType<FactorAlgebraic<BigRational>>();
        FactorFactory.GetImplementation(quotientFactory).ShouldBeOfType<FactorQuotient<BigRational>>();
    }

    private static GenPolynomialRing<BigRational> CreateRationalPolynomialRing()
    {
        return new GenPolynomialRing<BigRational>(
            new BigRational(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
