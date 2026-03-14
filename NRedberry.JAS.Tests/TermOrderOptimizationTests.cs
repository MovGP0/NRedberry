using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class TermOrderOptimizationTests
{
    [Fact]
    public void ShouldBuildDegreeMatricesAndPermutationMetadata()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 2, ["x", "y"]);
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(new BigRational(2), ExpVector.Create([0L, 1L]));

        List<GenPolynomial<BigInteger>>? matrix = TermOrderOptimization.DegreeMatrix(polynomial);
        List<GenPolynomial<BigInteger>>? aggregated = TermOrderOptimization.DegreeMatrix([polynomial, ring.Univariate(1)]);
        List<int> permutation = TermOrderOptimization.OptimalPermutation(matrix!);
        List<int> sortedPermutation = [.. permutation];

        sortedPermutation.Sort();

        matrix.ShouldNotBeNull();
        matrix!.Count.ShouldBe(2);
        aggregated.ShouldNotBeNull();
        aggregated!.ShouldNotBeEmpty();
        sortedPermutation.ShouldBe([0, 1]);
    }

    [Fact]
    public void ShouldPermuteArraysExponentsRingsAndPolynomials()
    {
        List<int> permutation = [1, 0];
        TermOrder weightedOrder = new([[1L, 0L], [0L, 1L]]);
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 2, weightedOrder, ["x", "y"]);
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(new BigRational(2), ExpVector.Create([0L, 1L]));

        string[] permutedStrings = TermOrderOptimization.StringArrayPermutation(permutation, ["x", "y"]);
        long[] permutedLongs = TermOrderOptimization.LongArrayPermutation(permutation, [10L, 20L]);
        ExpVector permutedExponent = TermOrderOptimization.Permutation(permutation, ExpVector.Create([1L, 2L]));
        GenPolynomialRing<BigRational>? permutedRing = TermOrderOptimization.Permutation(permutation, ring);
        GenPolynomial<BigRational>? permutedPolynomial = TermOrderOptimization.Permutation(permutation, permutedRing!, polynomial);
        List<GenPolynomial<BigRational>>? permutedList =
            TermOrderOptimization.Permutation(permutation, permutedRing!, [polynomial]);
        List<int> inverse = TermOrderOptimization.InversePermutation(permutation);
        OptimizedPolynomialList<BigRational> optimized = TermOrderOptimization.OptimizeTermOrder(ring, [polynomial]);

        permutedStrings.ShouldBe(["y", "x"]);
        permutedLongs.ShouldBe([20L, 10L]);
        permutedExponent.GetVal().ShouldBe([2L, 1L]);
        permutedRing!.GetVars()!.ShouldBe(["y", "x"]);
        permutedPolynomial!.Coefficient(ExpVector.Create([1L, 0L])).ToString().ShouldBe("2");
        permutedPolynomial.Coefficient(ExpVector.Create([0L, 1L])).ToString().ShouldBe("1");
        permutedList!.ShouldHaveSingleItem();
        inverse.ShouldBe([1, 0]);
        optimized.Polynomials.ShouldHaveSingleItem();
        optimized.Perm.Count.ShouldBe(2);
    }
}
