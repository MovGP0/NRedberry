using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PolyUfdUtilTests
{
    [Fact]
    public void ShouldClearQuotientDenominatorsAndConvertBackToUnitDenominators()
    {
        GenPolynomialRing<BigRational> coefficientRing = CreateCoefficientRing();
        QuotientRing<BigRational> quotientRing = new(coefficientRing, true);
        GenPolynomialRing<Quotient<BigRational>> quotientPolynomialRing = new(
            quotientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        GenPolynomialRing<GenPolynomial<BigRational>> integralRing = new(
            coefficientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        Quotient<BigRational> twoThirds = new(quotientRing, coefficientRing.FromInteger(2), coefficientRing.FromInteger(3));
        GenPolynomial<Quotient<BigRational>> quotientPolynomial = quotientPolynomialRing.Univariate(0).Multiply(twoThirds)
            .Sum(quotientPolynomialRing.FromInteger(1));

        GenPolynomial<GenPolynomial<BigRational>> integral = PolyUfdUtil.IntegralFromQuotientCoefficients(integralRing, quotientPolynomial);
        GenPolynomial<Quotient<BigRational>> converted = PolyUfdUtil.QuotientFromIntegralCoefficients(quotientPolynomialRing, integral);

        Assert.False(integral.IsZero());
        Assert.Equal(1L, integral.Degree(0));
        Assert.True(converted.LeadingBaseCoefficient().Den.IsOne());
    }

    [Fact]
    public void ShouldValidateFieldPropertyAndSimpleKroneckerRoundTrip()
    {
        ComplexRing<BigRational> complexRing = new(new BigRational());
        AlgebraicNumberRing<BigRational> fieldRing = complexRing.AlgebraicRing();
        GenPolynomialRing<BigRational> polynomialRing = new(
            new BigRational(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        GenPolynomial<BigRational> polynomial = polynomialRing.Univariate(0)
            .Sum(polynomialRing.FromInteger(1));
        GenPolynomial<BigRational> kronecker = PolyUfdUtil.SubstituteKronecker(polynomial, 3);

        PolyUfdUtil.EnsureFieldProperty(fieldRing);

        Assert.False(kronecker.IsZero());
        Assert.NotEqual(-1, fieldRing.GetField());
        Assert.Throws<ArgumentOutOfRangeException>(() => PolyUfdUtil.SubstituteKronecker(polynomial, 0));
    }

    private static GenPolynomialRing<BigRational> CreateCoefficientRing()
    {
        return new GenPolynomialRing<BigRational>(
            new BigRational(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["t"]);
    }
}
