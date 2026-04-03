using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ArrayExtensionsTests
{
    [Fact]
    public void ShouldFillRange()
    {
        int[] values = [1, 2, 3, 4];

        values.Fill(1, 2, 9);

        values.ShouldBe([1, 9, 9, 4]);
    }

    [Fact]
    public void ShouldThrowWhenArrayIsNull()
    {
        int[]? values = null;

        Should.Throw<ArgumentNullException>(() => values!.Fill(0, 1, 0));
    }

    [Fact]
    public void ShouldThrowWhenCountIsNegative()
    {
        int[] values = [1, 2, 3];

        Should.Throw<ArgumentOutOfRangeException>(() => values.Fill(0, -1, 0));
    }

    [Fact]
    public void ShouldFillRangeWhenItTouchesArrayEnd()
    {
        int[] values = [1, 2, 3, 4];

        values.Fill(2, 2, 0);

        values.ShouldBe([1, 2, 0, 0]);
    }
}
