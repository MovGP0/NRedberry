using Xunit;

using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateTranspositionTests
{
    [Fact(DisplayName = "Should throw for negative dimension in single-argument overload")]
    public void ShouldThrowForNegativeDimensionInSingleArgumentOverload()
    {
        var exception = Assert.Throws<ArgumentException>(() => _ = GroupPermutations.CreateTransposition(-1));

        Assert.Equal("dimension", exception.ParamName);
    }

    [Fact(DisplayName = "Should return empty mapping for zero dimension in single-argument overload")]
    public void ShouldReturnEmptyMappingForZeroDimensionInSingleArgumentOverload()
    {
        int[] transposition = GroupPermutations.CreateTransposition(0);

        Assert.Empty(transposition);
    }

    [Fact(DisplayName = "Should return identity mapping for one dimension in single-argument overload")]
    public void ShouldReturnIdentityMappingForOneDimensionInSingleArgumentOverload()
    {
        int[] transposition = GroupPermutations.CreateTransposition(1);

        Assert.Equal([0], transposition);
    }

    [Fact(DisplayName = "Should swap first two entries in single-argument overload when dimension greater than one")]
    public void ShouldSwapFirstTwoEntriesInSingleArgumentOverloadWhenDimensionGreaterThanOne()
    {
        int[] transposition = GroupPermutations.CreateTransposition(5);

        Assert.Equal([1, 0, 2, 3, 4], transposition);
    }

    [Fact(DisplayName = "Should swap requested positions in three-argument overload")]
    public void ShouldSwapRequestedPositionsInThreeArgumentOverload()
    {
        int[] transposition = GroupPermutations.CreateTransposition(6, 1, 4);

        Assert.Equal([0, 4, 2, 3, 1, 5], transposition);
    }

    [Fact(DisplayName = "Should return identity when positions are equal in three-argument overload")]
    public void ShouldReturnIdentityWhenPositionsAreEqualInThreeArgumentOverload()
    {
        int[] transposition = GroupPermutations.CreateTransposition(4, 2, 2);

        Assert.Equal([0, 1, 2, 3], transposition);
    }

    [Fact(DisplayName = "Should throw for negative dimension in three-argument overload")]
    public void ShouldThrowForNegativeDimensionInThreeArgumentOverload()
    {
        var exception = Assert.Throws<ArgumentException>(() => _ = GroupPermutations.CreateTransposition(-1, 0, 0));

        Assert.Equal("dimension", exception.ParamName);
    }

    [Theory(DisplayName = "Should throw for negative positions in three-argument overload")]
    [InlineData(3, -1, 1)]
    [InlineData(3, 1, -1)]
    [InlineData(3, -1, -1)]
    public void ShouldThrowForNegativePositionsInThreeArgumentOverload(int dimension, int position1, int position2)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = GroupPermutations.CreateTransposition(dimension, position1, position2));
    }

    [Theory(DisplayName = "Should throw for positions outside dimension in three-argument overload")]
    [InlineData(3, 3, 1)]
    [InlineData(3, 1, 3)]
    [InlineData(3, 5, 0)]
    public void ShouldThrowForPositionsOutsideDimensionInThreeArgumentOverload(int dimension, int position1, int position2)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = GroupPermutations.CreateTransposition(dimension, position1, position2));
    }
}
