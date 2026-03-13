using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorComplexTests
{
    [Fact]
    public void ShouldHandleZeroAndOnePolynomials()
    {
        ComplexRing<BigRational> coefficientRing = new(new BigRational());
        FactorComplex<BigRational> factor = new(coefficientRing);
        GenPolynomialRing<Complex<BigRational>> ring = CreateRing(coefficientRing, 1);

        List<GenPolynomial<Complex<BigRational>>> zeroFactors = factor.BaseFactorsSquarefree(ring.Zero);
        List<GenPolynomial<Complex<BigRational>>> oneFactors = factor.BaseFactorsSquarefree(ring.One);

        Assert.Empty(zeroFactors);
        Assert.Single(oneFactors);
        Assert.Equal(ring.One, oneFactors[0]);
    }

    [Fact]
    public void ShouldRejectMultivariatePolynomials()
    {
        ComplexRing<BigRational> coefficientRing = new(new BigRational());
        FactorComplex<BigRational> factor = new(coefficientRing);
        GenPolynomialRing<Complex<BigRational>> ring = CreateRing(coefficientRing, 2);
        GenPolynomial<Complex<BigRational>> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));

        Assert.Throws<ArgumentException>(() => factor.BaseFactorsSquarefree(polynomial));
    }

    private static GenPolynomialRing<Complex<BigRational>> CreateRing(ComplexRing<BigRational> coefficientRing, int nvar)
    {
        return new GenPolynomialRing<Complex<BigRational>>(
            coefficientRing,
            nvar,
            new TermOrder(TermOrder.INVLEX),
            nvar == 1 ? ["x"] : ["x", "y"]);
    }
}
