using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeInfiniteAlgebraicFieldCharPTests
{
    [Fact]
    public void ShouldRejectFiniteAlgebraicField()
    {
        ModLongRing coefficientRing = new(2, true);
        GenPolynomialRing<ModLong> polynomialRing = new(
            coefficientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["z"]);
        AlgebraicNumberRing<ModLong> algebraicRing = new(
            polynomialRing.Univariate(0, 2L).Sum(polynomialRing.Univariate(0)).Sum(polynomialRing.FromInteger(1)),
            true);

        Assert.Throws<ArgumentException>(() => new SquarefreeInfiniteAlgebraicFieldCharP<ModLong>(algebraicRing));
    }

    [Fact]
    public void ShouldFactorNonTrivialAlgebraicCoefficient()
    {
        AlgebraicNumberRing<Quotient<ModLong>> algebraicRing = CreateInfiniteAlgebraicRing();
        SquarefreeInfiniteAlgebraicFieldCharP<Quotient<ModLong>> squarefree = new(algebraicRing);
        AlgebraicNumber<Quotient<ModLong>> generator = algebraicRing.GetGenerator();

        SortedDictionary<AlgebraicNumber<Quotient<ModLong>>, long> factors = squarefree.SquarefreeFactors(generator);

        Assert.Single(factors);
        Assert.Equal(1L, factors.Single().Value);
        Assert.Equal(generator, factors.Single().Key);
    }

    private static AlgebraicNumberRing<Quotient<ModLong>> CreateInfiniteAlgebraicRing()
    {
        QuotientRing<ModLong> quotientRing = new(
            new GenPolynomialRing<ModLong>(
                new ModLongRing(2, true),
                1,
                new TermOrder(TermOrder.INVLEX),
                ["t"]),
            true);
        GenPolynomialRing<Quotient<ModLong>> algebraicPolynomialRing = new(
            quotientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["a"]);
        GenPolynomial<Quotient<ModLong>> modulus = algebraicPolynomialRing.Univariate(0, 2L)
            .Sum(algebraicPolynomialRing.Univariate(0))
            .Sum(algebraicPolynomialRing.FromInteger(1));

        return new AlgebraicNumberRing<Quotient<ModLong>>(modulus, true);
    }
}
