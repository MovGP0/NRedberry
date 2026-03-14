using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeAbstractTests
{
    [Fact]
    public void ShouldNormalizeFactorsAndVerifyFactorizations()
    {
        StubSquarefreeAbstract squarefree = new();
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> first = x.Sum(ring.FromInteger(1));
        GenPolynomial<BigRational> second = x.Subtract(ring.FromInteger(1));
        GenPolynomial<BigRational> product = first.Multiply(second);
        SortedDictionary<GenPolynomial<BigRational>, long> expectedFactors = new()
        {
            [first] = 1L,
            [second] = 1L
        };

        squarefree.IsSquarefree(first).ShouldBeTrue();
        squarefree.IsFactorization(product, [first, second]).ShouldBeTrue();
        squarefree.IsFactorization(product, expectedFactors).ShouldBeTrue();
        squarefree.SquarefreeFactors(new BigRational(2))[new BigRational(2)].ShouldBe(1L);
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

file sealed class StubSquarefreeAbstract()
    : SquarefreeAbstract<BigRational>(new GreatestCommonDivisorSimple<BigRational>())
{
    public override GenPolynomial<BigRational> BaseSquarefreePart(GenPolynomial<BigRational> polynomial)
    {
        return polynomial.Monic();
    }

    public override SortedDictionary<GenPolynomial<BigRational>, long> BaseSquarefreeFactors(GenPolynomial<BigRational> polynomial)
    {
        return new SortedDictionary<GenPolynomial<BigRational>, long>
        {
            [BaseSquarefreePart(polynomial)] = 1L
        };
    }

    public override GenPolynomial<GenPolynomial<BigRational>> RecursiveUnivariateSquarefreePart(
        GenPolynomial<GenPolynomial<BigRational>> polynomial)
    {
        return polynomial;
    }

    public override SortedDictionary<GenPolynomial<GenPolynomial<BigRational>>, long> RecursiveUnivariateSquarefreeFactors(
        GenPolynomial<GenPolynomial<BigRational>> polynomial)
    {
        return new SortedDictionary<GenPolynomial<GenPolynomial<BigRational>>, long>
        {
            [polynomial] = 1L
        };
    }

    public override GenPolynomial<BigRational> SquarefreePart(GenPolynomial<BigRational> polynomial)
    {
        return BaseSquarefreePart(polynomial);
    }

    public override SortedDictionary<GenPolynomial<BigRational>, long> SquarefreeFactors(GenPolynomial<BigRational> polynomial)
    {
        return BaseSquarefreeFactors(polynomial);
    }

    public override SortedDictionary<BigRational, long> SquarefreeFactors(BigRational coefficient)
    {
        SortedDictionary<BigRational, long> result = [];
        if (!coefficient.IsOne())
        {
            result[coefficient] = 1L;
        }

        return result;
    }
}
