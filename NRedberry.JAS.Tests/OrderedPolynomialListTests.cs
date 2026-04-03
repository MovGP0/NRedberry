using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class OrderedPolynomialListTests
{
    [Fact]
    public void ShouldSortPolynomialsByLeadingExponent()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> xSquared = x.Multiply(x);
        OrderedPolynomialList<BigRational> ordered = new(ring, [xSquared, x]);

        ordered.Polynomials[0].LeadingExpVector()!.GetVal().ShouldBe([1L]);
        ordered.Polynomials[1].LeadingExpVector()!.GetVal().ShouldBe([2L]);
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(new BigRational(), 1, ["x"]);
    }
}
