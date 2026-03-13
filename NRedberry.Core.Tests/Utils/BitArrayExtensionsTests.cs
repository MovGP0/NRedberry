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
        Assert.Empty(BitArrayExtensions.Empty.Cast<bool>());
    }

    [Fact]
    public void ShouldCopyRequestedRange()
    {
        string[] actual = new[] { "a", "b", "c", "d" }.CopyOfRange(1, 3);

        Assert.Equal(["b", "c"], actual);
    }

    [Fact]
    public void ShouldThrowWhenRangeIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => new[] { 1, 2, 3 }.CopyOfRange(2, 1));
    }

    [Fact]
    public void ShouldAppendBitArrays()
    {
        BitArray first = new([true, false, true]);
        BitArray second = new([false, true]);

        BitArray actual = first.Append(second);

        Assert.Equal(5, actual.Count);
        Assert.True(actual[0]);
        Assert.False(actual[1]);
        Assert.True(actual[2]);
        Assert.False(actual[3]);
        Assert.True(actual[4]);
    }
}
