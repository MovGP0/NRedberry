using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using System.Linq;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeFieldChar0Tests
{
    [Fact]
    public void ShouldComputeSquarefreePartAndFactorMultiplicity()
    {
        SquarefreeFieldChar0<BigRational> squarefree = new(new BigRational());
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> repeated = ring.Univariate(0).Subtract(ring.FromInteger(1));
        repeated = repeated.Multiply(repeated);

        GenPolynomial<BigRational> part = squarefree.BaseSquarefreePart(repeated);
        SortedDictionary<GenPolynomial<BigRational>, long> factors = squarefree.BaseSquarefreeFactors(repeated);

        part.ShouldBe(ring.Univariate(0).Subtract(ring.FromInteger(1)));
        factors.ShouldHaveSingleItem();
        factors.Keys.Any(factor => factor.Equals(part)).ShouldBeTrue();
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
