using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsSelectTests
{
    [Fact]
    public void ShouldSelectDistinctSortedPositions()
    {
        string[] actual = ArraysUtils.Select(["a", "b", "c", "d"], [3, 1, 3]);

        actual.ShouldBe(["b", "d"]);
    }

    [Fact]
    public void ShouldThrowWhenArrayIsNull()
    {
        Should.Throw<ArgumentNullException>(() => ArraysUtils.Select<string>(null!, [0]));
    }
}
