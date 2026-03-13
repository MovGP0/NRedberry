using System.Numerics;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class PermutationContractsTests
{
    [Fact]
    public void ShouldExposePermutationOperationsThroughSymmetry()
    {
        Permutation permutation = new Symmetry([1, 2, 0], false);
        int[] values = [10, 20, 30];

        Assert.Equal([1, 2, 0], permutation.OneLine());
        Assert.Equal([20, 30, 10], permutation.Permute(values));
        Assert.Equal(1, permutation.ImageOf(0));
        Assert.Equal(new BigInteger(3), permutation.Order);
        Assert.False(permutation.IsIdentity);
        Assert.Equal([2, 0, 1], permutation.Inverse().OneLine());
    }
}

public sealed class PermutationPriorityTupleTests
{
    [Fact]
    public void ShouldCompareUsingPermutationOnly()
    {
        PermutationPriorityTuple left = new([1, 0]);
        PermutationPriorityTuple right = new([1, 0]);
        right.Priority = 5;

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Equal("[1, 0] : 1", left.ToString());
    }
}

public sealed class PermutationsGeneratorTests
{
    [Fact]
    public void ShouldWrapPermutationGeneratorAsPermutationSequence()
    {
        PermutationsGenerator<Permutation> generator = new(2);

        Assert.True(generator.MoveNext());
        Assert.Equal([0, 1], generator.Current.OneLine());
        Assert.True(generator.MoveNext());
        Assert.Equal([1, 0], generator.Current.OneLine());
        Assert.False(generator.MoveNext());
    }
}
