using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class CombinatoricsTests
{
    [Fact]
    public void ShouldCreateExpectedGeneratorImplementations()
    {
        IIntCombinatorialGenerator permutations = Combinatorics.CreateIntGenerator(2, 2);
        IIntCombinatorialGenerator combinations = Combinatorics.CreateIntGenerator(3, 2);

        Assert.IsType<IntPermutationsGenerator>(permutations);
        Assert.IsType<IntCombinationPermutationGenerator>(combinations);
    }

    [Fact]
    public void ShouldCreateBasicPermutationHelpers()
    {
        Assert.Equal([0, 1, 2, 3], Combinatorics.CreateIdentity(4));
        Assert.Equal([1, 0, 2, 3], Combinatorics.CreateTransposition(4));
        Assert.Equal([0, 2, 1, 3], Combinatorics.CreateTransposition(4, 1, 2));
        Assert.Equal([3, 0, 1, 2], Combinatorics.CreateCycle(4));
    }

    [Fact]
    public void ShouldValidatePermutationsAcrossSupportedOverloads()
    {
        int[] permutation = [2, 0, 1];
        long[] longPermutation = [2, 0, 1];

        Assert.True(Combinatorics.TestPermutationCorrectness(permutation));
        Assert.True(Combinatorics.TestPermutationCorrectness(longPermutation));
        Assert.False(Combinatorics.TestPermutationCorrectness([0, 0, 1]));
        Assert.False(Combinatorics.TestPermutationCorrectness([0L, 2L, 2L]));
    }

    [Fact]
    public void ShouldShuffleReorderAndInvertPermutationData()
    {
        long[] permutation = [2, 0, 1];

        Assert.Equal([1L, 2L, 0L], Combinatorics.Inverse(permutation));
        Assert.Equal(["c", "a", "b"], Combinatorics.Shuffle(["a", "b", "c"], permutation));
        Assert.Equal([30L, 10L, 20L], Combinatorics.Reorder([10L, 20L, 30L], permutation));
    }
}
