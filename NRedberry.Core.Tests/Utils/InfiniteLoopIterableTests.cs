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

        first.MoveNext().ShouldBeTrue();
        second.MoveNext().ShouldBeTrue();
        first.Current.ShouldBe(1);
        second.Current.ShouldBe(1);

        first.MoveNext().ShouldBeTrue();
        first.Current.ShouldBe(2);
        first.MoveNext().ShouldBeTrue();
        first.Current.ShouldBe(1);
    }
}
