using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorAbstractTests
{
    [Fact]
    public void ShouldIdentifyLinearPolynomialAsIrreducibleAndQuadraticAsReducible()
    {
        StubFactorAbstract factor = new(new BigRational());
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> linear = ring.Univariate(0).Sum(ring.FromInteger(1));
        GenPolynomial<BigRational> reducible = ring.Univariate(0, 2L).Subtract(ring.FromInteger(1));

        factor.ShouldSatisfyAllConditions(
            () => factor.IsIrreducible(linear).ShouldBeTrue(),
            () => factor.IsIrreducible(reducible).ShouldBeFalse());
    }

    [Fact]
    public void ShouldNormalizeSignsAndReturnRadicalFactors()
    {
        StubFactorAbstract factor = new(new BigRational());
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> reducible = ring.Univariate(0, 2L).Subtract(ring.FromInteger(1));

        List<GenPolynomial<BigRational>> normalized = factor.NormalizeFactorization(
        [
            ring.FromInteger(-1),
            x.Negate().Subtract(ring.FromInteger(1)),
            x.Subtract(ring.FromInteger(1))
        ]);
        List<GenPolynomial<BigRational>> radicalFactors = factor.BaseFactorsRadical(reducible);

        normalized.ShouldSatisfyAllConditions(
            () => normalized.Count.ShouldBe(2),
            () => normalized[0].ShouldBe(x.Sum(ring.FromInteger(1))),
            () => normalized[1].ShouldBe(x.Subtract(ring.FromInteger(1))),
            () => radicalFactors.Count.ShouldBe(2));
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

file sealed class StubFactorAbstract(RingFactory<BigRational> coefficientFactory)
    : FactorAbstract<BigRational>(coefficientFactory)
{
    public override List<GenPolynomial<BigRational>> BaseFactorsSquarefree(GenPolynomial<BigRational> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        List<GenPolynomial<BigRational>> factors = [];
        if (polynomial.IsZero())
        {
            return factors;
        }

        if (polynomial.IsOne() || polynomial.Degree() <= 1)
        {
            factors.Add(polynomial);
            return factors;
        }

        GenPolynomialRing<BigRational> ring = (GenPolynomialRing<BigRational>)polynomial.Factory();
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> expected = x.Multiply(x).Subtract(ring.FromInteger(1));
        if (polynomial.Equals(expected))
        {
            factors.Add(x.Sum(ring.FromInteger(1)));
            factors.Add(x.Subtract(ring.FromInteger(1)));
            return factors;
        }

        factors.Add(polynomial);
        return factors;
    }
}
