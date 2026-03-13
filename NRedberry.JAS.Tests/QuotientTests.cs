using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class QuotientTests
{
    [Fact]
    public void ShouldReduceFractionsAndSupportArithmetic()
    {
        QuotientRing<BigRational> ring = CreateRing();
        GenPolynomialRing<BigRational> polynomialRing = ring.Ring;
        GenPolynomial<BigRational> t = polynomialRing.Univariate(0);
        Quotient<BigRational> reduced = new(
            ring,
            t.Multiply(t).Subtract(polynomialRing.FromInteger(1)),
            t.Subtract(polynomialRing.FromInteger(1)));
        Quotient<BigRational> one = ring.One;

        Assert.True(reduced.Den.IsOne());
        Assert.Equal(t.Sum(polynomialRing.FromInteger(1)), reduced.Num);
        Assert.True(reduced.Multiply(reduced.Inverse()).IsOne());
        Assert.True(reduced.Divide(reduced).IsOne());
    }

    [Fact]
    public void ShouldParseAndCompareEquivalentValues()
    {
        QuotientRing<BigRational> ring = CreateRing();
        Quotient<BigRational> parsed = ring.Parse("{ 2 | 3 }");
        Quotient<BigRational> same = new(ring, ring.Ring.FromInteger(2), ring.Ring.FromInteger(3));

        Assert.Equal(same, parsed);
        Assert.Equal(0, parsed.CompareTo(same));
        Assert.Equal("{ 2/3 }", parsed.ToString());
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
