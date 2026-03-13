using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsSelectTests
{
    [Fact]
    public void ShouldSelectDistinctSortedPositions()
    {
        string[] actual = ArraysUtils.Select(["a", "b", "c", "d"], [3, 1, 3]);

        Assert.Equal(["b", "d"], actual);
    }

    [Fact]
    public void ShouldThrowWhenArrayIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ArraysUtils.Select<string>(null!, [0]));
    }
}
