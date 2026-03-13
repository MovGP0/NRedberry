using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class EmptyIteratorTests
{
    [Fact]
    public void ShouldNeverAdvanceAndShouldThrowForCurrent()
    {
        EmptyIterator<int> iterator = EmptyIterator<int>.Instance;

        Assert.False(iterator.MoveNext());
        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
    }

    [Fact]
    public void ShouldAllowResetAndDispose()
    {
        EmptyIterator<string> iterator = EmptyIterator<string>.Instance;

        iterator.Reset();
        iterator.Dispose();

        Assert.False(iterator.MoveNext());
    }
}
