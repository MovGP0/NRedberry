using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GenPolynomialTests
{
    [Fact]
    public void ShouldSupportAdditionMultiplicationAndMonicNormalization()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> polynomial = x.Sum(new BigRational(2));
        GenPolynomial<BigRational> squared = polynomial.Multiply(polynomial);
        GenPolynomial<BigRational> scaled = x.Multiply(new BigRational(2));

        Assert.Equal("4", squared.Coefficient(ExpVector.Create([0L])).ToString());
        Assert.Equal("4", squared.Coefficient(ExpVector.Create([1L])).ToString());
        Assert.Equal("1", squared.Coefficient(ExpVector.Create([2L])).ToString());
        Assert.Equal("x", scaled.Monic().ToString(["x"]));
    }

    [Fact]
    public void ShouldEnumerateMonomialsInLeadingOrder()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(new BigRational(2));

        Monomial<BigRational>[] monomials = polynomial.ToArray();

        Assert.Equal(2, monomials.Length);
        Assert.Equal([1L], monomials[0].Exponent().GetVal());
        Assert.Equal("1", monomials[0].Coefficient().ToString());
        Assert.Equal([0L], monomials[1].Exponent().GetVal());
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(new BigRational(), 1, ["x"]);
    }
}
