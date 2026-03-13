using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class OptimizedPolynomialListTests
{
    [Fact]
    public void ShouldExposePermutationInStringRepresentation()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        OptimizedPolynomialList<BigRational> optimized = new([1, 0], ring, [ring.Univariate(0)]);

        Assert.Equal([1, 0], optimized.Perm);
        Assert.Contains("permutation = 1, 0", optimized.ToString(), System.StringComparison.Ordinal);
    }
}
