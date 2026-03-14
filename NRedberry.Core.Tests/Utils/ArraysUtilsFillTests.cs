using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsFillTests
{
    [Fact]
    public void ShouldFillSpecifiedRange()
    {
        List<int> values = [1, 2, 3, 4];

        ArraysUtils.Fill(values, 1, 3, 9);

        values.ShouldBe([1, 9, 9, 4]);
    }

    [Fact]
    public void ShouldFillEntireList()
    {
        List<int> values = [1, 2, 3];

        ArraysUtils.Fill(values, 7);

        values.ShouldBe([7, 7, 7]);
    }
}
