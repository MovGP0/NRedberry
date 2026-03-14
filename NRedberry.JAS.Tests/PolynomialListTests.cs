using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PolynomialListTests
{
    [Fact]
    public void ShouldCopyListsWithoutSharingCollectionState()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> xSquared = x.Multiply(x);
        PolynomialList<BigRational> left = new(ring, [x, xSquared]);
        PolynomialList<BigRational> right = left.Copy();

        right.Ring.ShouldBe(left.Ring);
        right.Polynomials.Count.ShouldBe(left.Polynomials.Count);
        right.ToString().ShouldBe(left.ToString());
        right.Polynomials.ShouldNotBeSameAs(left.Polynomials);
    }

    [Fact]
    public void ShouldIncludeRingAndPolynomialsInStringRepresentation()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        PolynomialList<BigRational> list = new(ring, [ring.Univariate(0)]);

        list.ToString().ShouldContain("x");
    }
}
