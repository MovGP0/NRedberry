using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GenSolvablePolynomialTests
{
    [Fact]
    public void ShouldSupportAdditionMultiplicationAndCoefficientScaling()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();
        GenSolvablePolynomial<BigRational> x = ring.Univariate(0);
        GenSolvablePolynomial<BigRational> polynomial = x.Sum(ring.FromInteger(2));
        GenSolvablePolynomial<BigRational> squared = polynomial.Multiply(polynomial);
        GenSolvablePolynomial<BigRational> scaled = x.Multiply(new BigRational(2));

        Assert.Equal("4", squared.Coefficient(ExpVector.Create([0L])).ToString());
        Assert.Equal("4", squared.Coefficient(ExpVector.Create([1L])).ToString());
        Assert.Equal("1", squared.Coefficient(ExpVector.Create([2L])).ToString());
        Assert.Equal("2 x", scaled.ToString(["x"]));
    }

    [Fact]
    public void ShouldRespectZeroOneAndFactorySemantics()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();
        GenSolvablePolynomial<BigRational> x = ring.Univariate(0);

        Assert.True(GenSolvablePolynomialRing<BigRational>.Zero.IsZero());
        Assert.True(GenSolvablePolynomialRing<BigRational>.One.IsOne());
        Assert.Equal(ring, x.Factory());
        Assert.Equal(x.ToString(["x"]), x.Copy().ToString(["x"]));
    }

    private static GenSolvablePolynomialRing<BigRational> CreateRing()
    {
        return new GenSolvablePolynomialRing<BigRational>(new BigRational(), 1, new TermOrder(TermOrder.INVLEX), ["x"]);
    }
}
