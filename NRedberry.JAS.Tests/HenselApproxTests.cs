using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class HenselApproxTests
{
    [Fact]
    public void ShouldSupportEqualityHashCodeAndStringRepresentation()
    {
        GenPolynomialRing<JasBigInteger> integerRing = new(
            new JasBigInteger(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        ModLongRing modularRing = new(5, true);
        GenPolynomialRing<ModLong> modularPolynomialRing = new(
            modularRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);

        HenselApprox<ModLong> first = new(
            integerRing.Univariate(0).Sum(integerRing.FromInteger(1)),
            integerRing.Univariate(0).Subtract(integerRing.FromInteger(1)),
            modularPolynomialRing.Univariate(0).Sum(modularPolynomialRing.FromInteger(1)),
            modularPolynomialRing.Univariate(0).Subtract(modularPolynomialRing.FromInteger(1)));
        HenselApprox<ModLong> second = new(
            integerRing.Univariate(0).Sum(integerRing.FromInteger(1)),
            integerRing.Univariate(0).Subtract(integerRing.FromInteger(1)),
            modularPolynomialRing.Univariate(0).Sum(modularPolynomialRing.FromInteger(1)),
            modularPolynomialRing.Univariate(0).Subtract(modularPolynomialRing.FromInteger(1)));

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        Assert.Contains(",", first.ToString(), System.StringComparison.Ordinal);
    }
}
