using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysTests
{
    [Fact]
    public void ShouldCopyRange()
    {
        int[] actual = Arrays.copyOfRange([1, 2, 3, 4], 1, 3);

        actual.ShouldBe([2, 3]);
    }

    [Fact]
    public void ShouldCopyAndPadWithZeros()
    {
        int[] actual = Arrays.copyOf([1, 2], 4);

        actual.ShouldBe([1, 2, 0, 0]);
    }

    [Fact]
    public void ShouldFillArrayWithValue()
    {
        int[] actual = [1, 2, 3];

        Arrays.fill(actual, 7);

        actual.ShouldBe([7, 7, 7]);
    }

    [Fact]
    public void ShouldReturnIndexFromBinarySearch()
    {
        int actual = Arrays.BinarySearch([1, 3, 5, 7], 5);

        actual.ShouldBe(2);
    }

    [Fact]
    public void ShouldReturnInsertionPointFromBinarySearch()
    {
        int actual = Arrays.BinarySearch([1, 3, 5, 7], 4);

        actual.ShouldBe(-3);
    }
}
