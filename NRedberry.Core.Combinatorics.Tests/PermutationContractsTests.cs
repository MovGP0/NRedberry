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

        permutation.OneLine().ShouldBe([1, 2, 0]);
        permutation.Permute(values).ShouldBe([20, 30, 10]);
        permutation.ImageOf(0).ShouldBe(1);
        permutation.Order.ShouldBe(new BigInteger(3));
        permutation.IsIdentity.ShouldBeFalse();
        permutation.Inverse().OneLine().ShouldBe([2, 0, 1]);
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

        left.ShouldBe(right);
        left.GetHashCode().ShouldBe(right.GetHashCode());
        left.ToString().ShouldBe("[1, 0] : 1");
    }
}

public sealed class PermutationsGeneratorTests
{
    [Fact]
    public void ShouldWrapPermutationGeneratorAsPermutationSequence()
    {
        PermutationsGenerator<Permutation> generator = new(2);

        generator.MoveNext().ShouldBeTrue();
        generator.Current.OneLine().ShouldBe([0, 1]);
        generator.MoveNext().ShouldBeTrue();
        generator.Current.OneLine().ShouldBe([1, 0]);
        generator.MoveNext().ShouldBeFalse();
    }
}
