using System.Collections;
using System.Collections.Generic;
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

        Assert.True(degrees[0]);
        Assert.True(degrees[1]);
        Assert.False(degrees[2]);
        Assert.True(degrees[3]);
        Assert.True(degrees[4]);
    }

    [Fact]
    public void ShouldReturnTrivialSquarefreeFactorsAndDegreeSum()
    {
        FactorInteger<ModLong> factor = new();
        GenPolynomialRing<JasBigInteger> ring = CreateRing();
        GenPolynomial<JasBigInteger> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));

        List<GenPolynomial<JasBigInteger>> factors = factor.BaseFactorsSquarefree(polynomial);
        List<GenPolynomial<JasBigInteger>> henselFactors = factor.FactorsSquarefreeHensel(polynomial);

        Assert.Single(factors);
        Assert.Equal(polynomial, factors[0]);
        Assert.Single(henselFactors);
        Assert.Equal(1L, FactorInteger<ModLong>.DegreeSum(factors));
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
