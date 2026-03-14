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

        ring.IsCommutative().ShouldBeTrue();
        ring.IsAssociative().ShouldBeTrue();
        ring.FromInteger(3).ToString(["x"]).ShouldBe("3");
        ring.Univariate(0).ToString(["x"]).ShouldBe("x");
        ring.UnivariateList().ShouldHaveSingleItem();
        ring.GetVars()!.ShouldBe(["x"]);
        ring.ToString().ShouldContain("RelationTable");
    }

    [Fact]
    public void ShouldSupportStructuralTransformsWithEmptyRelationTable()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();

        ring.Extend(1).Nvar.ShouldBe(2);
        ring.Extend(1).Contract(1).Nvar.ShouldBe(1);
        ring.Reverse().GetVars()!.Single().ShouldBe("x");
        ring.Copy(ring.Univariate(0)).ToString(["x"]).ShouldBe("x");
    }

    private static GenSolvablePolynomialRing<BigRational> CreateRing()
    {
        return new GenSolvablePolynomialRing<BigRational>(new BigRational(), 1, new TermOrder(TermOrder.INVLEX), ["x"]);
    }
}
