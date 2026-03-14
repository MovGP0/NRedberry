using System.Collections;
using System.Linq;
using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class BitArrayExtensionsTests
{
    [Fact]
    public void ShouldExposeEmptyBitArray()
    {
        BitArrayExtensions.Empty.Cast<bool>().ShouldBeEmpty();
    }

    [Fact]
    public void ShouldCopyRequestedRange()
    {
        string[] actual = new[] { "a", "b", "c", "d" }.CopyOfRange(1, 3);

        actual.ShouldBe(["b", "c"]);
    }

    [Fact]
    public void ShouldThrowWhenRangeIsInvalid()
    {
        Should.Throw<ArgumentException>(() => new[] { 1, 2, 3 }.CopyOfRange(2, 1));
    }

    [Fact]
    public void ShouldAppendBitArrays()
    {
        BitArray first = new([true, false, true]);
        BitArray second = new([false, true]);

        BitArray actual = first.Append(second);

        actual.Count.ShouldBe(5);
        actual[0].ShouldBeTrue();
        actual[1].ShouldBeFalse();
        actual[2].ShouldBeTrue();
        actual[3].ShouldBeFalse();
        actual[4].ShouldBeTrue();
    }
}
