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

        Assert.True(ring.IsField());
        Assert.False(ring.IsFinite());
        Assert.True(ring.IsCommutative());
        Assert.True(ring.IsAssociative());
        Assert.True(ring.Zero.IsZero());
        Assert.True(ring.One.IsOne());
        Assert.Equal(parsed, factory.FromInteger(new System.Numerics.BigInteger(5)).Divide(ring.FromInteger(2)));
        Assert.NotEmpty(ring.Generators());
        Assert.Contains("RatFunc", ring.ToString(), System.StringComparison.Ordinal);
        Assert.Equal(ring.ToString(), ring.ToScript());
    }

    [Fact]
    public void ShouldCloneEqualityAndHashByUnderlyingRing()
    {
        QuotientRing<BigRational> ring = CreateRing();
        Quotient<BigRational> value = ring.FromInteger(3);

        Assert.Equal(value, QuotientRing<BigRational>.Clone(value));
        Assert.Equal(ring, CreateRing());
        Assert.Equal(ring.GetHashCode(), CreateRing().GetHashCode());
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
