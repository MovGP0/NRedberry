using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GreatestCommonDivisorModEvalTests
{
    [Fact]
    public void ShouldComputeBaseGcdForModularPolynomials()
    {
        ModLongRing coefficientRing = new(5, true);
        GreatestCommonDivisorModEval<ModLong> gcd = new();
        GenPolynomialRing<ModLong> ring = CreateRing(coefficientRing);
        GenPolynomial<ModLong> first = ring.Univariate(0, 2L).Subtract(ring.FromInteger(1));
        GenPolynomial<ModLong> second = ring.Univariate(0).Subtract(ring.FromInteger(1));

        Assert.Equal(second, gcd.BaseGcd(first, second));
    }

    private static GenPolynomialRing<ModLong> CreateRing(ModLongRing coefficientRing)
    {
        return new GenPolynomialRing<ModLong>(
            coefficientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
