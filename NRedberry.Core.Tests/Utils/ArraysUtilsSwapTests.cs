using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsSwapTests
{
    [Fact]
    public void ShouldSwapIntegers()
    {
        int[] values = [1, 2, 3];

        ArraysUtils.Swap(values, 0, 2);

        values.ShouldBe([3, 2, 1]);
    }

    [Fact]
    public void ShouldSwapLongs()
    {
        long[] values = [1L, 2L, 3L];

        ArraysUtils.Swap(values, 0, 1);

        values.ShouldBe([2L, 1L, 3L]);
    }

    [Fact]
    public void ShouldSwapGenericValues()
    {
        string[] values = ["a", "b", "c"];

        ArraysUtils.Swap(values, 1, 2);

        values.ShouldBe(["a", "c", "b"]);
    }
}
