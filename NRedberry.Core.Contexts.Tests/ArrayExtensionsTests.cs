using NRedberry.Contexts;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ArrayExtensionsTests
{
    [Fact]
    public void ShouldFillRange()
    {
        int[] values = { 1, 2, 3, 4 };

        values.Fill(1, 2, 9);

        Assert.Equal(new[] { 1, 9, 9, 4 }, values);
    }

    [Fact]
    public void ShouldThrowWhenArrayIsNull()
    {
        int[]? values = null;

        Assert.Throws<ArgumentNullException>(() => values!.Fill(0, 1, 0));
    }

    [Fact]
    public void ShouldThrowWhenCountIsNegative()
    {
        int[] values = { 1, 2, 3 };

        Assert.Throws<ArgumentOutOfRangeException>(() => values.Fill(0, -1, 0));
    }

    [Fact]
    public void ShouldThrowWhenRangeTouchesArrayEnd()
    {
        int[] values = { 1, 2, 3, 4 };

        Assert.Throws<ArgumentOutOfRangeException>(() => values.Fill(2, 2, 0));
    }
}
