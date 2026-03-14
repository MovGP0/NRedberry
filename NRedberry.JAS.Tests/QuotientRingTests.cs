using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class QuotientRingTests
{
    [Fact]
    public void ShouldExposeFactoryPropertiesParsingAndGenerators()
    {
        QuotientRing<BigRational> ring = CreateRing();
        Quotient<BigRational> parsed = ring.Parse("{ 5 | 2 }");
        ElemFactory<Quotient<BigRational>> factory = ring;

        ring.IsField().ShouldBeTrue();
        ring.IsFinite().ShouldBeFalse();
        ring.IsCommutative().ShouldBeTrue();
        ring.IsAssociative().ShouldBeTrue();
        ring.Zero.IsZero().ShouldBeTrue();
        ring.One.IsOne().ShouldBeTrue();
        factory.FromInteger(new System.Numerics.BigInteger(5)).Divide(ring.FromInteger(2)).ShouldBe(parsed);
        ring.Generators().ShouldNotBeEmpty();
        ring.ToString().ShouldContain("RatFunc");
        ring.ToScript().ShouldBe(ring.ToString());
    }

    [Fact]
    public void ShouldCloneEqualityAndHashByUnderlyingRing()
    {
        QuotientRing<BigRational> ring = CreateRing();
        Quotient<BigRational> value = ring.FromInteger(3);

        QuotientRing<BigRational>.Clone(value).ShouldBe(value);
        CreateRing().ShouldBe(ring);
        CreateRing().GetHashCode().ShouldBe(ring.GetHashCode());
    }

    private static QuotientRing<BigRational> CreateRing()
    {
        return new QuotientRing<BigRational>(
            new GenPolynomialRing<BigRational>(
                new BigRational(),
                1,
                new TermOrder(TermOrder.INVLEX),
                ["t"]),
            true);
    }
}
