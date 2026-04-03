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

        squared.Coefficient(ExpVector.Create([0L])).ToString().ShouldBe("4");
        squared.Coefficient(ExpVector.Create([1L])).ToString().ShouldBe("4");
        squared.Coefficient(ExpVector.Create([2L])).ToString().ShouldBe("1");
        scaled.Monic().ToString(["x"]).ShouldBe("x");
    }

    [Fact]
    public void ShouldEnumerateMonomialsInLeadingOrder()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(new BigRational(2));

        Monomial<BigRational>[] monomials = polynomial.ToArray();

        monomials.Length.ShouldBe(2);
        monomials[0].Exponent().GetVal().ShouldBe([1L]);
        monomials[0].Coefficient().ToString().ShouldBe("1");
        monomials[1].Exponent().GetVal().ShouldBe([0L]);
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(new BigRational(), 1, ["x"]);
    }
}
