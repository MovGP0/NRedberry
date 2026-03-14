using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class SingleIteratorTests
{
    [Fact]
    public void ShouldYieldSingleElementExactlyOnce()
    {
        using SingleIterator<int> iterator = new(42);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(42);
        iterator.MoveNext().ShouldBeFalse();
    }

    [Fact]
    public void ShouldResetToInitialState()
    {
        using SingleIterator<string> iterator = new("value");

        iterator.MoveNext().ShouldBeTrue();
        iterator.Reset();
        Should.Throw<InvalidOperationException>(() => _ = iterator.Current);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe("value");
    }
}
