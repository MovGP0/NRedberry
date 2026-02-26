using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateBlockTranspositionTests
{
    [Fact(DisplayName = "ShouldCreateExpectedPermutationWhenLength1IsTwoAndLength2IsThree")]
    public void ShouldCreateExpectedPermutationWhenLength1IsTwoAndLength2IsThree()
    {
        int[] actual = GroupPermutations.CreateBlockTransposition(2, 3);

        Assert.Equal(new[] { 2, 3, 4, 0, 1 }, actual);
    }

    [Fact(DisplayName = "ShouldCreateExpectedPermutationWhenLength1IsZeroAndLength2IsThree")]
    public void ShouldCreateExpectedPermutationWhenLength1IsZeroAndLength2IsThree()
    {
        int[] actual = GroupPermutations.CreateBlockTransposition(0, 3);

        Assert.Equal(new[] { 0, 1, 2 }, actual);
    }

    [Fact(DisplayName = "ShouldCreateExpectedPermutationWhenLength1IsThreeAndLength2IsZero")]
    public void ShouldCreateExpectedPermutationWhenLength1IsThreeAndLength2IsZero()
    {
        int[] actual = GroupPermutations.CreateBlockTransposition(3, 0);

        Assert.Equal(new[] { 0, 1, 2 }, actual);
    }

    [Fact(DisplayName = "ShouldCreateExpectedPermutationWhenBlockLengthsAreEqual")]
    public void ShouldCreateExpectedPermutationWhenBlockLengthsAreEqual()
    {
        int[] actual = GroupPermutations.CreateBlockTransposition(3, 3);

        Assert.Equal(new[] { 3, 4, 5, 0, 1, 2 }, actual);
    }

    [Theory(DisplayName = "ShouldThrowArgumentExceptionWhenAnyBlockLengthIsNegative")]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(-1, -1)]
    public void ShouldThrowArgumentExceptionWhenAnyBlockLengthIsNegative(int length1, int length2)
    {
        Assert.Throws<ArgumentException>(() => _ = GroupPermutations.CreateBlockTransposition(length1, length2));
    }
}
