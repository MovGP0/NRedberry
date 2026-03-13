using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchIteratorITests
{
    [Fact]
    public void ShouldEnumerateIntegerStretchesAndSupportReset()
    {
        StretchIteratorI iterator = new([1, 1, 2, 2, 2]);

        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(0, 2), iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(2, 3), iterator.Current);
        Assert.False(iterator.MoveNext());

        iterator.Reset();
        Assert.True(iterator.HasNext());
        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(0, 2), iterator.Current);
    }

    [Fact]
    public void ShouldThrowForUnsupportedRemoveAndInvalidCurrent()
    {
        StretchIteratorI iterator = new([1, 2]);

        Assert.Throws<InvalidOperationException>(() => _ = iterator.Current);
        Assert.Throws<NotSupportedException>(() => iterator.Remove());
    }
}
