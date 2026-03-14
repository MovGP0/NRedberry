using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class EmptyIteratorTests
{
    [Fact]
    public void ShouldNeverAdvanceAndShouldThrowForCurrent()
    {
        EmptyIterator<int> iterator = EmptyIterator<int>.Instance;

        iterator.MoveNext().ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => _ = iterator.Current);
    }

    [Fact]
    public void ShouldAllowResetAndDispose()
    {
        EmptyIterator<string> iterator = EmptyIterator<string>.Instance;

        iterator.Reset();
        iterator.Dispose();

        iterator.MoveNext().ShouldBeFalse();
    }
}
