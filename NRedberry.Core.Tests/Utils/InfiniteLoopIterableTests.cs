using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class InfiniteLoopIterableTests
{
    [Fact]
    public void ShouldCreateFreshLoopingEnumeratorEachTime()
    {
        InfiniteLoopIterable<int> iterable = new(1, 2);

        using IEnumerator<int> first = iterable.GetEnumerator();
        using IEnumerator<int> second = iterable.GetEnumerator();

        Assert.True(first.MoveNext());
        Assert.True(second.MoveNext());
        Assert.Equal(1, first.Current);
        Assert.Equal(1, second.Current);

        Assert.True(first.MoveNext());
        Assert.Equal(2, first.Current);
        Assert.True(first.MoveNext());
        Assert.Equal(1, first.Current);
    }
}
