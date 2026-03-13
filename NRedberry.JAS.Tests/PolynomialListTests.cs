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

        Assert.Equal(left.Ring, right.Ring);
        Assert.Equal(left.Polynomials.Count, right.Polynomials.Count);
        Assert.Equal(left.ToString(), right.ToString());
        Assert.NotSame(left.Polynomials, right.Polynomials);
    }

    [Fact]
    public void ShouldIncludeRingAndPolynomialsInStringRepresentation()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        PolynomialList<BigRational> list = new(ring, [ring.Univariate(0)]);

        Assert.Contains("x", list.ToString(), System.StringComparison.Ordinal);
    }
}
