using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorAlgebraicTests
{
    [Fact]
    public void ShouldHandleZeroAndOnePolynomials()
    {
        AlgebraicNumberRing<BigRational> coefficientRing = new ComplexRing<BigRational>(new BigRational()).AlgebraicRing();
        FactorAlgebraic<BigRational> factor = new(coefficientRing);
        GenPolynomialRing<AlgebraicNumber<BigRational>> ring = CreateRing(coefficientRing, 1);

        List<GenPolynomial<AlgebraicNumber<BigRational>>> zeroFactors = factor.BaseFactorsSquarefree(ring.Zero);
        List<GenPolynomial<AlgebraicNumber<BigRational>>> oneFactors = factor.BaseFactorsSquarefree(ring.One);

        Assert.Empty(zeroFactors);
        Assert.Single(oneFactors);
        Assert.Equal(ring.One, oneFactors[0]);
    }

    [Fact]
    public void ShouldRejectMultivariatePolynomials()
    {
        AlgebraicNumberRing<BigRational> coefficientRing = new ComplexRing<BigRational>(new BigRational()).AlgebraicRing();
        FactorAlgebraic<BigRational> factor = new(coefficientRing);
        GenPolynomialRing<AlgebraicNumber<BigRational>> ring = CreateRing(coefficientRing, 2);
        GenPolynomial<AlgebraicNumber<BigRational>> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));

        Assert.Throws<ArgumentException>(() => factor.BaseFactorsSquarefree(polynomial));
    }

    private static GenPolynomialRing<AlgebraicNumber<BigRational>> CreateRing(AlgebraicNumberRing<BigRational> coefficientRing, int nvar)
    {
        return new GenPolynomialRing<AlgebraicNumber<BigRational>>(
            coefficientRing,
            nvar,
            new TermOrder(TermOrder.INVLEX),
            nvar == 1 ? ["x"] : ["x", "y"]);
    }
}
