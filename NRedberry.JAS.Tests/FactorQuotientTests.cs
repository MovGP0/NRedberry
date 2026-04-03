using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorQuotientTests
{
    [Fact]
    public void ShouldHandleZeroOneAndLinearPolynomials()
    {
        QuotientRing<BigRational> coefficientRing = new(CreateCoefficientRing(), true);
        FactorQuotient<BigRational> factor = new(coefficientRing);
        GenPolynomialRing<Quotient<BigRational>> ring = CreatePolynomialRing(coefficientRing);
        GenPolynomial<Quotient<BigRational>> zero = ring.Zero;
        GenPolynomial<Quotient<BigRational>> one = ring.One;
        GenPolynomial<Quotient<BigRational>> linear = ring.Univariate(0).Sum(ring.FromInteger(1));

        List<GenPolynomial<Quotient<BigRational>>> zeroFactors = factor.BaseFactorsSquarefree(zero);
        List<GenPolynomial<Quotient<BigRational>>> oneFactors = factor.FactorsSquarefree(one);
        List<GenPolynomial<Quotient<BigRational>>> linearFactors = factor.FactorsSquarefree(linear);

        zeroFactors.ShouldBeEmpty();
        oneFactors.Count.ShouldBe(1);
        oneFactors[0].ShouldBe(one);
        linearFactors.Count.ShouldBe(1);
        linearFactors[0].ShouldBe(linear);
    }

    private static GenPolynomialRing<BigRational> CreateCoefficientRing()
    {
        return new GenPolynomialRing<BigRational>(
            new BigRational(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["t"]);
    }

    private static GenPolynomialRing<Quotient<BigRational>> CreatePolynomialRing(QuotientRing<BigRational> coefficientRing)
    {
        return new GenPolynomialRing<Quotient<BigRational>>(
            coefficientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
