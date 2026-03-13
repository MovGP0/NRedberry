using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class FactorizationTests
{
    [Fact]
    public void ShouldDispatchIsIrreducibleThroughInterface()
    {
        Factorization<BigRational> factorization = new LinearPolynomialFactorization();
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> linear = ring.Univariate(0).Sum(ring.FromInteger(1));

        Assert.True(factorization.IsIrreducible(linear));
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

file sealed class LinearPolynomialFactorization : Factorization<BigRational>
{
    public bool IsIrreducible(GenPolynomial<BigRational> P)
    {
        return !P.IsZero() && P.Degree() <= 1;
    }
}
