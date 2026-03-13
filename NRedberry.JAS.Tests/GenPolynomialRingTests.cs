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

        Assert.True(constant.IsConstant());
        Assert.Equal("3", constant.LeadingBaseCoefficient().ToString());
        Assert.Equal([1L], variable.LeadingExpVector()!.GetVal());
        Assert.Equal("1", ring.GetOneCoefficient().ToString());
    }

    [Fact]
    public void ShouldTrackVariableNames()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        string[]? variables = ring.GetVars();

        Assert.NotNull(variables);
        Assert.Equal(["x"], variables);
        Assert.Contains("x", ring.VarsToString(), System.StringComparison.Ordinal);
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(new BigRational(), 1, ["x"]);
    }
}
