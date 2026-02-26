using NRedberry.Groups;
using Xunit;

using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateCycleTests
{
    [Fact(DisplayName = "Should throw for negative dimension")]
    public void ShouldThrowForNegativeDimension()
    {
        var exception = Assert.Throws<ArgumentException>(() => _ = GroupPermutations.CreateCycle(-1));

        Assert.Equal("dimension", exception.ParamName);
    }

    [Fact(DisplayName = "Should return empty cycle for zero dimension")]
    public void ShouldReturnEmptyCycleForZeroDimension()
    {
        int[] cycle = GroupPermutations.CreateCycle(0);

        Assert.Empty(cycle);
    }

    [Fact(DisplayName = "Should return identity mapping for one dimension")]
    public void ShouldReturnIdentityMappingForOneDimension()
    {
        int[] cycle = GroupPermutations.CreateCycle(1);

        Assert.Equal([0], cycle);
    }

    [Fact(DisplayName = "Should create expected one-line mapping for positive dimension")]
    public void ShouldCreateExpectedOneLineMappingForPositiveDimension()
    {
        int[] cycle = GroupPermutations.CreateCycle(5);

        Assert.Equal([4, 0, 1, 2, 3], cycle);
    }

    [Fact(DisplayName = "Should map each position to previous element in cycle")]
    public void ShouldMapEachPositionToPreviousElementInCycle()
    {
        const int dimension = 6;
        int[] cycle = GroupPermutations.CreateCycle(dimension);

        for (int i = 1; i < dimension; i++)
        {
            Assert.Equal(i - 1, cycle[i]);
        }

        Assert.Equal(dimension - 1, cycle[0]);
    }
}
