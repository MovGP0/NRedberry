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

        reduced.Den.IsOne().ShouldBeTrue();
        reduced.Num.ShouldBe(t.Sum(polynomialRing.FromInteger(1)));
        reduced.Multiply(reduced.Inverse()).IsOne().ShouldBeTrue();
        reduced.Divide(reduced).IsOne().ShouldBeTrue();
    }

    [Fact]
    public void ShouldParseAndCompareEquivalentValues()
    {
        QuotientRing<BigRational> ring = CreateRing();
        Quotient<BigRational> parsed = ring.Parse("{ 2 | 3 }");
        Quotient<BigRational> same = new(ring, ring.Ring.FromInteger(2), ring.Ring.FromInteger(3));

        parsed.ShouldBe(same);
        parsed.CompareTo(same).ShouldBe(0);
        parsed.ToString().ShouldBe("{ 2/3 }");
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
