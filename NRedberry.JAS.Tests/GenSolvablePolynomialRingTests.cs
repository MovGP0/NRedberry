using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GenSolvablePolynomialRingTests
{
    [Fact]
    public void ShouldExposeRingPropertiesAndUnivariates()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();

        Assert.True(ring.IsCommutative());
        Assert.True(ring.IsAssociative());
        Assert.Equal("3", ring.FromInteger(3).ToString(["x"]));
        Assert.Equal("x", ring.Univariate(0).ToString(["x"]));
        Assert.Single(ring.UnivariateList());
        Assert.Equal(["x"], ring.GetVars()!);
        Assert.Contains("RelationTable", ring.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void ShouldSupportStructuralTransformsWithEmptyRelationTable()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();

        Assert.Equal(2, ring.Extend(1).Nvar);
        Assert.Equal(1, ring.Extend(1).Contract(1).Nvar);
        Assert.Equal("x", ring.Reverse().GetVars()!.Single());
        Assert.Equal("x", ring.Copy(ring.Univariate(0)).ToString(["x"]));
    }

    private static GenSolvablePolynomialRing<BigRational> CreateRing()
    {
        return new GenSolvablePolynomialRing<BigRational>(new BigRational(), 1, new TermOrder(TermOrder.INVLEX), ["x"]);
    }
}
