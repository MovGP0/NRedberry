using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class GreatestCommonDivisorModularTests
{
    [Fact]
    public void ShouldComputeBaseGcdWithBothModularStrategies()
    {
        GreatestCommonDivisorModular<ModLong> modular = new();
        GreatestCommonDivisorModular<ModLong> simple = new(true);
        GenPolynomialRing<JasBigInteger> ring = CreateRing();
        GenPolynomial<JasBigInteger> first = ring.Univariate(0, 2L).Subtract(ring.FromInteger(1));
        GenPolynomial<JasBigInteger> second = ring.Univariate(0).Subtract(ring.FromInteger(1));

        Assert.Equal(second, modular.BaseGcd(first, second));
        Assert.Equal(second, simple.BaseGcd(first, second));
    }

    private static GenPolynomialRing<JasBigInteger> CreateRing()
    {
        return new GenPolynomialRing<JasBigInteger>(
            new JasBigInteger(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
