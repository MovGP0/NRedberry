using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorsTests
{
    [Fact]
    public void ShouldUsePolynomialHashWhenNoExtensionDataExists()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));
        Factors<BigRational> factors = new(polynomial, null, null, null, null);

        Assert.Equal(polynomial.GetHashCode(), factors.GetHashCode());
    }

    [Fact]
    public void ShouldPreserveExtensionMetadataAndAffectHashCode()
    {
        GenPolynomialRing<BigRational> ring = CreateRing();
        ComplexRing<BigRational> complexRing = new(new BigRational());
        AlgebraicNumberRing<BigRational> algebraicRing = complexRing.AlgebraicRing();
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));
        GenPolynomialRing<AlgebraicNumber<BigRational>> algebraicPolynomialRing = new(
            algebraicRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        GenPolynomial<AlgebraicNumber<BigRational>> algebraicPolynomial = algebraicPolynomialRing.Univariate(0).Sum(algebraicPolynomialRing.FromInteger(1));
        Factors<AlgebraicNumber<BigRational>> nested = new(algebraicPolynomial, null, null, null, null);
        List<GenPolynomial<AlgebraicNumber<BigRational>>> algebraicFactors = [algebraicPolynomial];
        List<Factors<AlgebraicNumber<BigRational>>> nestedFactors = [nested];
        Factors<BigRational> factors = new(polynomial, algebraicRing, algebraicPolynomial, algebraicFactors, nestedFactors);

        Assert.Same(polynomial, factors.Poly);
        Assert.Same(algebraicRing, factors.Afac);
        Assert.Same(algebraicPolynomial, factors.Apoly);
        Assert.Same(algebraicFactors, factors.Afactors);
        Assert.Same(nestedFactors, factors.Arfactors);
        Assert.NotEqual(polynomial.GetHashCode(), factors.GetHashCode());
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
