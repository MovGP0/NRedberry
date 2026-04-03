using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GenPolynomialRingTests
{
    [Fact]
    public void ShouldCreateConstantsAndUnivariatePolynomials()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> constant = ring.FromInteger(3);
        GenPolynomial<BigRational> variable = ring.Univariate(0);

        constant.IsConstant().ShouldBeTrue();
        constant.LeadingBaseCoefficient().ToString().ShouldBe("3");
        variable.LeadingExpVector()!.GetVal().ShouldBe([1L]);
        ring.GetOneCoefficient().ToString().ShouldBe("1");
    }

    [Fact]
    public void ShouldTrackVariableNames()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        string[]? variables = ring.GetVars();

        variables.ShouldNotBeNull();
        variables.ShouldBe(["x"]);
        ring.VarsToString().ShouldContain("x");
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(new BigRational(), 1, ["x"]);
    }
}
