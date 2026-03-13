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
        Assert.IsType<FactorInteger<ModLong>>(FactorFactory.GetImplementation(new JasBigInteger()));
        Assert.IsType<FactorRational>(FactorFactory.GetImplementation(new BigRational()));
        Assert.IsType<FactorModular<ModInteger>>(FactorFactory.GetImplementation(new ModIntegerRing(new JasBigInteger(5), true)));
        Assert.IsType<FactorModular<ModLong>>(FactorFactory.GetImplementation(new ModLongRing(5, true)));
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

        Assert.IsType<FactorRational>(FactorFactory.GetImplementation(rationalFactory));
        Assert.IsType<FactorRational>(FactorFactory.GetImplementation(polynomialRing));
        Assert.IsType<FactorComplex<BigRational>>(FactorFactory.GetImplementation(complexFactory));
        Assert.IsType<FactorAlgebraic<BigRational>>(FactorFactory.GetImplementation(algebraicFactory));
        Assert.IsType<FactorQuotient<BigRational>>(FactorFactory.GetImplementation(quotientFactory));
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
