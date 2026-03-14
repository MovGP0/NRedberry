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

        PolynomialTaylorFunction<BigRational> derivative = function.Deriviative().ShouldBeOfType<PolynomialTaylorFunction<BigRational>>();

        function.IsZERO().ShouldBeFalse();
        function.ToString().ShouldBe(polynomial.ToString(["x"]));
        function.Evaluate(new BigRational(2)).ToString().ShouldBe("9");
        derivative.Evaluate(new BigRational(3)).ToString().ShouldBe("8");
    }

    [Fact]
    public void ShouldReportZeroPolynomialFunctions()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        PolynomialTaylorFunction<BigRational> function = new(new GenPolynomial<BigRational>(ring));

        function.IsZERO().ShouldBeTrue();
    }
}
