using GroupPermutations = NRedberry.Groups.Permutations;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsConvertCyclesToOneLineTests
{
    [Fact]
    public void ShouldReturnEmptyPermutationWhenCyclesAreEmpty()
    {
        int[][] cycles = [];

        int[] result = GroupPermutations.ConvertCyclesToOneLine(cycles);

        Assert.Empty(result);
    }

    [Fact]
    public void ShouldConvertSingleCycleToOneLineWhenCycleContainsThreePoints()
    {
        int[][] cycles =
        [
            [0, 2, 1]
        ];

        int[] result = GroupPermutations.ConvertCyclesToOneLine(cycles);

        Assert.Equal(new[] { 2, 0, 1 }, result);
    }

    [Fact]
    public void ShouldConvertDisjointCyclesAndLeaveUnmentionedPointsFixed()
    {
        int[][] cycles =
        [
            [0, 2],
            [3, 4, 5]
        ];

        int[] result = GroupPermutations.ConvertCyclesToOneLine(cycles);

        Assert.Equal(new[] { 2, 1, 0, 4, 5, 3 }, result);
    }

    [Fact]
    public void ShouldThrowArgumentExceptionWhenCycleContainsSinglePoint()
    {
        int[][] cycles =
        [
            [1]
        ];

        ArgumentException exception = Assert.Throws<ArgumentException>(() => GroupPermutations.ConvertCyclesToOneLine(cycles));

        Assert.Contains("Illegal use of cycle notation", exception.Message);
    }

    [Fact]
    public void ShouldReconstructOriginalPermutationForConvertOneLineToCyclesOutput()
    {
        int[] oneLine = [2, 0, 1, 4, 5, 3];
        int[][] cycles = GroupPermutations.ConvertOneLineToCycles(oneLine);

        int[] reconstructed = GroupPermutations.ConvertCyclesToOneLine(cycles);

        Assert.Equal(oneLine, reconstructed);
    }
}
