using Shouldly;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class CombinatoricsTests
{
    [Fact]
    public void ShouldCreateExpectedGeneratorImplementations()
    {
        IIntCombinatorialGenerator permutations = Combinatorics.CreateIntGenerator(2, 2);
        IIntCombinatorialGenerator combinations = Combinatorics.CreateIntGenerator(3, 2);

        permutations.ShouldBeOfType<IntPermutationsGenerator>();
        combinations.ShouldBeOfType<IntCombinationPermutationGenerator>();
    }

    [Fact]
    public void ShouldCreateBasicPermutationHelpers()
    {
        Combinatorics.CreateIdentity(4).ShouldBe([0, 1, 2, 3]);
        Combinatorics.CreateTransposition(4).ShouldBe([1, 0, 2, 3]);
        Combinatorics.CreateTransposition(4, 1, 2).ShouldBe([0, 2, 1, 3]);
        Combinatorics.CreateCycle(4).ShouldBe([3, 0, 1, 2]);
    }

    [Fact]
    public void ShouldValidatePermutationsAcrossSupportedOverloads()
    {
        int[] permutation = [2, 0, 1];
        long[] longPermutation = [2, 0, 1];

        Combinatorics.TestPermutationCorrectness(permutation).ShouldBeTrue();
        Combinatorics.TestPermutationCorrectness(longPermutation).ShouldBeTrue();
        Combinatorics.TestPermutationCorrectness([0, 0, 1]).ShouldBeFalse();
        Combinatorics.TestPermutationCorrectness([0L, 2L, 2L]).ShouldBeFalse();
    }

    [Fact]
    public void ShouldShuffleReorderAndInvertPermutationData()
    {
        long[] permutation = [2, 0, 1];

        Combinatorics.Inverse(permutation).ShouldBe([1L, 2L, 0L]);
        Combinatorics.Shuffle(["a", "b", "c"], permutation).ShouldBe(["c", "a", "b"]);
        Combinatorics.Reorder([10L, 20L, 30L], permutation).ShouldBe([30L, 10L, 20L]);
    }
}
