using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class SingleIteratorTests
{
    [Fact]
    public void ShouldYieldSingleElementExactlyOnce()
    {
        using SingleIterator<int> iterator = new(42);

        Assert.True(iterator.MoveNext());
        Assert.Equal(42, iterator.Current);
        Assert.False(iterator.MoveNext());
    }

    [Fact]
    public void ShouldResetToInitialState()
    {
        using SingleIterator<string> iterator = new("value");

        Assert.True(iterator.MoveNext());
        iterator.Reset();
        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal("value", iterator.Current);
    }
}
