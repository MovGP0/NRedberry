using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class FactorIntegerTests
{
    [Fact]
    public void ShouldComputeReachableFactorDegrees()
    {
        FactorInteger<ModLong> factor = new();
        List<ExpVector> exponents =
        [
            ExpVector.Create([1L]),
            ExpVector.Create([3L])
        ];

        BitArray degrees = factor.FactorDegrees(exponents, 4);

        degrees[0].ShouldBeTrue();
        degrees[1].ShouldBeTrue();
        degrees[2].ShouldBeFalse();
        degrees[3].ShouldBeTrue();
        degrees[4].ShouldBeTrue();
    }

    [Fact]
    public void ShouldReturnTrivialSquarefreeFactorsAndDegreeSum()
    {
        FactorInteger<ModLong> factor = new();
        GenPolynomialRing<JasBigInteger> ring = CreateRing();
        GenPolynomial<JasBigInteger> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));

        List<GenPolynomial<JasBigInteger>> factors = factor.BaseFactorsSquarefree(polynomial);
        List<GenPolynomial<JasBigInteger>> henselFactors = factor.FactorsSquarefreeHensel(polynomial);

        factors.Count.ShouldBe(1);
        factors[0].ShouldBe(polynomial);
        henselFactors.Count.ShouldBe(1);
        FactorInteger<ModLong>.DegreeSum(factors).ShouldBe(1L);
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
