using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorModularTests
{
    [Fact]
    public void ShouldReturnTrivialEqualDegreeFactorsForMatchingDegree()
    {
        ModLongRing coefficientRing = new(5, true);
        FactorModular<ModLong> factor = new(coefficientRing);
        GenPolynomialRing<ModLong> ring = CreateRing(coefficientRing, 1);
        GenPolynomial<ModLong> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));

        List<GenPolynomial<ModLong>> factors = factor.BaseEqualDegreeFactors(polynomial, polynomial.Degree(0));

        factors.Count.ShouldBe(1);
        factors[0].ShouldBe(polynomial);
    }

    [Fact]
    public void ShouldValidateSquarefreeFactorizationInputs()
    {
        ModLongRing coefficientRing = new(5, true);
        FactorModular<ModLong> factor = new(coefficientRing);
        GenPolynomialRing<ModLong> ring = CreateRing(coefficientRing, 1);
        GenPolynomial<ModLong> multivariate = CreateRing(coefficientRing, 2).Univariate(0).Sum(CreateRing(coefficientRing, 2).FromInteger(1));
        GenPolynomial<ModLong> nonMonic = ring.Univariate(0).Multiply(coefficientRing.FromInteger(2)).Sum(ring.FromInteger(1));

        factor.BaseDistinctDegreeFactors(ring.Zero).ShouldBeEmpty();
        Should.Throw<ArgumentException>(() => factor.BaseDistinctDegreeFactors(multivariate));
        Should.Throw<ArgumentException>(() => factor.BaseFactorsSquarefree(nonMonic));
    }

    private static GenPolynomialRing<ModLong> CreateRing(ModLongRing coefficientRing, int nvar)
    {
        return new GenPolynomialRing<ModLong>(
            coefficientRing,
            nvar,
            new TermOrder(TermOrder.INVLEX),
            nvar == 1 ? ["x"] : ["x", "y"]);
    }
}
