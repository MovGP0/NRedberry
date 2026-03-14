using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PolynomialComparatorTests
{
    [Fact]
    public void ShouldReverseComparisonWhenRequested()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> xSquared = x.Multiply(x);
        PolynomialComparator<BigRational> forward = new(new TermOrder(), false);
        PolynomialComparator<BigRational> reverse = new(new TermOrder(), true);

        int forwardResult = forward.Compare(xSquared, x);
        int reverseResult = reverse.Compare(xSquared, x);

        forwardResult.ShouldNotBe(0);
        reverseResult.ShouldBe(-forwardResult);
    }
}
