using System.Collections.Generic;
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

        Assert.True(factor.IsIrreducible(linear));
        Assert.False(factor.IsIrreducible(reducible));
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

        Assert.Collection(
            normalized,
            factorPolynomial => Assert.Equal(x.Sum(ring.FromInteger(1)), factorPolynomial),
            factorPolynomial => Assert.Equal(x.Subtract(ring.FromInteger(1)), factorPolynomial));
        Assert.Equal(2, radicalFactors.Count);
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

file sealed class StubFactorAbstract : FactorAbstract<BigRational>
{
    public StubFactorAbstract(RingFactory<BigRational> coefficientFactory)
        : base(coefficientFactory)
    {
    }

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
