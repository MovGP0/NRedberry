using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GreatestCommonDivisorSubresTests
{
    [Fact]
    public void ShouldComputeBaseGcdAndResultantForUnivariatePolynomials()
    {
        GreatestCommonDivisorSubres<BigRational> gcd = new();
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> first = ring.Univariate(0, 2L).Subtract(ring.FromInteger(1));
        GenPolynomial<BigRational> second = ring.Univariate(0).Subtract(ring.FromInteger(1));

        Assert.Equal(second, gcd.BaseGcd(first, second));
        Assert.True(gcd.BaseResultant(first, second).IsZero());
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(
            new BigRational(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
