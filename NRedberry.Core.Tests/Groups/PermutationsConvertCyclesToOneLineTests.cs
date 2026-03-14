using Shouldly;
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

        result.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldConvertSingleCycleToOneLineWhenCycleContainsThreePoints()
    {
        int[][] cycles =
        [
            [0, 2, 1]
        ];

        int[] result = GroupPermutations.ConvertCyclesToOneLine(cycles);

        result.ShouldBe([2, 0, 1]);
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

        result.ShouldBe([2, 1, 0, 4, 5, 3]);
    }

    [Fact]
    public void ShouldThrowArgumentExceptionWhenCycleContainsSinglePoint()
    {
        int[][] cycles =
        [
            [1]
        ];

        ArgumentException exception = Should.Throw<ArgumentException>(() => GroupPermutations.ConvertCyclesToOneLine(cycles));

        exception.Message.ShouldContain("Illegal use of cycle notation");
    }

    [Fact]
    public void ShouldReconstructOriginalPermutationForConvertOneLineToCyclesOutput()
    {
        int[] oneLine = [2, 0, 1, 4, 5, 3];
        int[][] cycles = GroupPermutations.ConvertOneLineToCycles(oneLine);

        int[] reconstructed = GroupPermutations.ConvertCyclesToOneLine(cycles);

        reconstructed.ShouldBe(oneLine);
    }
}
