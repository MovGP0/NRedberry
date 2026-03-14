using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeFactoryTests
{
    [Fact]
    public void ShouldReturnExpectedImplementationsForScalarFactories()
    {
        SquarefreeFactory.GetImplementation(new JasBigInteger()).ShouldBeOfType<SquarefreeRingChar0<JasBigInteger>>();
        SquarefreeFactory.GetImplementation(new BigRational()).ShouldBeOfType<SquarefreeFieldChar0<BigRational>>();
        SquarefreeFactory.GetImplementation(new ModLongRing(5, true)).ShouldBeOfType<SquarefreeFiniteFieldCharP<ModLong>>();
    }

    [Fact]
    public void ShouldReturnExpectedImplementationsForCompositeFactories()
    {
        ComplexRing<BigRational> complexRing = new(new BigRational());
        QuotientRing<BigRational> quotientRing = new(
            new GenPolynomialRing<BigRational>(
                new BigRational(),
                1,
                new TermOrder(TermOrder.INVLEX),
                ["t"]),
            true);
        RingFactory<Complex<BigRational>> complexFactory = complexRing;
        RingFactory<Quotient<BigRational>> quotientFactory = quotientRing;

        SquarefreeFactory.GetImplementation(complexFactory).ShouldBeOfType<SquarefreeFieldChar0<Complex<BigRational>>>();
        SquarefreeFactory.GetImplementation(quotientFactory).ShouldBeOfType<SquarefreeFieldChar0<Quotient<BigRational>>>();
    }
}
