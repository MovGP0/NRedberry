using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeRingChar0Tests
{
    [Fact]
    public void ShouldComputeBaseSquarefreePartAndMultiplicities()
    {
        SquarefreeRingChar0<JasBigInteger> squarefree = new(new JasBigInteger());
        GenPolynomialRing<JasBigInteger> ring = CreateRing();
        GenPolynomial<JasBigInteger> x = ring.Univariate(0);
        GenPolynomial<JasBigInteger> repeated = x.Subtract(ring.FromInteger(1));
        repeated = repeated.Multiply(repeated).Multiply(x.Sum(ring.FromInteger(2)));

        GenPolynomial<JasBigInteger> part = squarefree.BaseSquarefreePart(repeated);
        SortedDictionary<GenPolynomial<JasBigInteger>, long> factors = squarefree.BaseSquarefreeFactors(repeated);

        Assert.Equal(x.Subtract(ring.FromInteger(1)).Multiply(x.Sum(ring.FromInteger(2))), part);
        Assert.Equal(2, factors.Count);
        Assert.Equal(2L, factors.Single(entry => entry.Key.Equals(x.Subtract(ring.FromInteger(1)))).Value);
        Assert.Equal(1L, factors.Single(entry => entry.Key.Equals(x.Sum(ring.FromInteger(2)))).Value);
    }

    [Fact]
    public void ShouldRejectFieldCoefficientFactory()
    {
        Assert.Throws<ArgumentException>(
            () => new SquarefreeRingChar0<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>(
                new NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational()));
    }

    private static GenPolynomialRing<JasBigInteger> CreateRing()
    {
        return new GenPolynomialRing<JasBigInteger>(
            new JasBigInteger(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
