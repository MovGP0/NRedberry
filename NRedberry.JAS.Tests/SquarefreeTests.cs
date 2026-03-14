using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeTests
{
    [Fact]
    public void ShouldDispatchSquarefreeContractThroughInterface()
    {
        Squarefree<BigRational> squarefree = new InterfaceSquarefree();
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(ring.FromInteger(1));

        squarefree.IsSquarefree(polynomial).ShouldBeTrue();
        squarefree.SquarefreePart(polynomial).ShouldBe(polynomial.Monic());
        squarefree.SquarefreeFactors(polynomial).ShouldHaveSingleItem();
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

file sealed class InterfaceSquarefree : Squarefree<BigRational>
{
    public GenPolynomial<BigRational> SquarefreePart(GenPolynomial<BigRational> P)
    {
        return P.Monic();
    }

    public bool IsSquarefree(GenPolynomial<BigRational> P)
    {
        return SquarefreePart(P).Equals(P.Monic());
    }

    public SortedDictionary<GenPolynomial<BigRational>, long> SquarefreeFactors(GenPolynomial<BigRational> P)
    {
        return new SortedDictionary<GenPolynomial<BigRational>, long>
        {
            [SquarefreePart(P)] = 1L
        };
    }
}
