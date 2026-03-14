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

        squared.Coefficient(ExpVector.Create([0L])).ToString().ShouldBe("4");
        squared.Coefficient(ExpVector.Create([1L])).ToString().ShouldBe("4");
        squared.Coefficient(ExpVector.Create([2L])).ToString().ShouldBe("1");
        scaled.ToString(["x"]).ShouldBe("2 x");
    }

    [Fact]
    public void ShouldRespectZeroOneAndFactorySemantics()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();
        GenSolvablePolynomial<BigRational> x = ring.Univariate(0);

        ring.Zero.IsZero().ShouldBeTrue();
        ring.One.IsOne().ShouldBeTrue();
        x.Factory().ShouldBe(ring);
        x.Copy().ToString(["x"]).ShouldBe(x.ToString(["x"]));
    }

    private static GenSolvablePolynomialRing<BigRational> CreateRing()
    {
        return new GenSolvablePolynomialRing<BigRational>(new BigRational(), 1, new TermOrder(TermOrder.INVLEX), ["x"]);
    }
}
