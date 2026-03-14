using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsAddAllTests
{
    [Fact]
    public void ShouldConcatenateTwoArrays()
    {
        int[] actual = ArraysUtils.AddAll([1, 2], 3, 4);

        actual.ShouldBe([1, 2, 3, 4]);
    }

    [Fact]
    public void ShouldConcatenateMultipleArrays()
    {
        int[] actual = ArraysUtils.AddAll([1, 2], [3], [], [4, 5]);

        actual.ShouldBe([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void ShouldReturnEmptyWhenNoArraysAreProvided()
    {
        int[] actual = ArraysUtils.AddAll();

        actual.ShouldBeEmpty();
    }
}
