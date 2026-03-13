using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorRationalTests
{
    [Fact]
    public void ShouldHandleZeroOneAndLinearPolynomials()
    {
        FactorRational factor = new();
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> zero = ring.Zero;
        GenPolynomial<BigRational> one = ring.One;
        GenPolynomial<BigRational> linear = ring.Univariate(0).Sum(ring.FromInteger(1));

        List<GenPolynomial<BigRational>> zeroFactors = factor.BaseFactorsSquarefree(zero);
        List<GenPolynomial<BigRational>> oneFactors = factor.BaseFactorsSquarefree(one);
        List<GenPolynomial<BigRational>> linearFactors = factor.FactorsSquarefree(linear);

        Assert.Empty(zeroFactors);
        Assert.Single(oneFactors);
        Assert.Equal(one, oneFactors[0]);
        Assert.Single(linearFactors);
        Assert.Equal(linear, linearFactors[0]);
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
