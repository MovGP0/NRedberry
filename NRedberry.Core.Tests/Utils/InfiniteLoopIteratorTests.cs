using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class InfiniteLoopIteratorTests
{
    [Fact]
    public void ShouldCycleThroughItemsIndefinitely()
    {
        InfiniteLoopIterator<int> iterator = new(1, 2, 3);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(1);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(2);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(3);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(1);
    }

    [Fact]
    public void ShouldResetToInitialPosition()
    {
        InfiniteLoopIterator<string> iterator = new("a", "b");

        Should.Throw<InvalidOperationException>(() => _ = iterator.Current);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe("a");

        iterator.Reset();

        Should.Throw<InvalidOperationException>(() => _ = iterator.Current);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe("a");
    }

    [Fact]
    public void ShouldReturnFalseForEmptySequence()
    {
        InfiniteLoopIterator<int> iterator = new();

        iterator.MoveNext().ShouldBeFalse();
    }
}
