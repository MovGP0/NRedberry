using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsRemoveTests
{
    [Fact]
    public void ShouldRemoveSingleElement()
    {
        int[] actual = ArraysUtils.Remove([1, 2, 3], 1);

        actual.ShouldBe([1, 3]);
    }

    [Fact]
    public void ShouldRemoveDistinctSortedPositions()
    {
        string[] actual = ArraysUtils.Remove(["a", "b", "c", "d"], [3, 1, 3]);

        actual.ShouldBe(["a", "c"]);
    }

    [Fact]
    public void ShouldThrowWhenPositionIsOutsideArray()
    {
        Should.Throw<IndexOutOfRangeException>(() => ArraysUtils.Remove([1, 2, 3], [3]));
    }
}
