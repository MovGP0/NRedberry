using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysTests
{
    [Fact]
    public void ShouldCopyRange()
    {
        int[] actual = Arrays.copyOfRange([1, 2, 3, 4], 1, 3);

        Assert.Equal([2, 3], actual);
    }

    [Fact]
    public void ShouldCopyAndPadWithZeros()
    {
        int[] actual = Arrays.copyOf([1, 2], 4);

        Assert.Equal([1, 2, 0, 0], actual);
    }

    [Fact]
    public void ShouldFillArrayWithValue()
    {
        int[] actual = [1, 2, 3];

        Arrays.fill(actual, 7);

        Assert.Equal([7, 7, 7], actual);
    }

    [Fact]
    public void ShouldReturnIndexFromBinarySearch()
    {
        int actual = Arrays.BinarySearch([1, 3, 5, 7], 5);

        Assert.Equal(2, actual);
    }

    [Fact]
    public void ShouldReturnInsertionPointFromBinarySearch()
    {
        int actual = Arrays.BinarySearch([1, 3, 5, 7], 4);

        Assert.Equal(-3, actual);
    }
}
