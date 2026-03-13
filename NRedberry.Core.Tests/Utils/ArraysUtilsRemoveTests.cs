using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsRemoveTests
{
    [Fact]
    public void ShouldRemoveSingleElement()
    {
        int[] actual = ArraysUtils.Remove([1, 2, 3], 1);

        Assert.Equal([1, 3], actual);
    }

    [Fact]
    public void ShouldRemoveDistinctSortedPositions()
    {
        string[] actual = ArraysUtils.Remove(["a", "b", "c", "d"], [3, 1, 3]);

        Assert.Equal(["a", "c"], actual);
    }

    [Fact]
    public void ShouldThrowWhenPositionIsOutsideArray()
    {
        Assert.Throws<IndexOutOfRangeException>(() => ArraysUtils.Remove([1, 2, 3], [3]));
    }
}
