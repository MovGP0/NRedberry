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

        Assert.Equal(ring.Univariate(0).Subtract(ring.FromInteger(1)), part);
        Assert.Single(factors);
        Assert.Contains(part, factors.Keys);
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
