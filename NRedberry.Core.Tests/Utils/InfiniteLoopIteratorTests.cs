using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class InfiniteLoopIteratorTests
{
    [Fact]
    public void ShouldCycleThroughItemsIndefinitely()
    {
        InfiniteLoopIterator<int> iterator = new(1, 2, 3);

        Assert.True(iterator.MoveNext());
        Assert.Equal(1, iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(2, iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(3, iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(1, iterator.Current);
    }

    [Fact]
    public void ShouldResetToInitialPosition()
    {
        InfiniteLoopIterator<string> iterator = new("a", "b");

        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal("a", iterator.Current);

        iterator.Reset();

        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal("a", iterator.Current);
    }

    [Fact]
    public void ShouldReturnFalseForEmptySequence()
    {
        InfiniteLoopIterator<int> iterator = new();

        Assert.False(iterator.MoveNext());
    }
}
