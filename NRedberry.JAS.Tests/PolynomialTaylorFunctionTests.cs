using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PolynomialTaylorFunctionTests
{
    [Fact]
    public void ShouldEvaluateDifferentiateAndFormatPolynomialFunctions()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> polynomial = ring.Univariate(0, 2).Sum(new BigRational(2), ExpVector.Create([1L])).Sum(new BigRational(1));
        PolynomialTaylorFunction<BigRational> function = new(polynomial);

        PolynomialTaylorFunction<BigRational> derivative = Assert.IsType<PolynomialTaylorFunction<BigRational>>(function.Deriviative());

        Assert.False(function.IsZERO());
        Assert.Equal(polynomial.ToString(["x"]), function.ToString());
        Assert.Equal("9", function.Evaluate(new BigRational(2)).ToString());
        Assert.Equal("8", derivative.Evaluate(new BigRational(3)).ToString());
    }

    [Fact]
    public void ShouldReportZeroPolynomialFunctions()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        PolynomialTaylorFunction<BigRational> function = new(new GenPolynomial<BigRational>(ring));

        Assert.True(function.IsZERO());
    }
}
