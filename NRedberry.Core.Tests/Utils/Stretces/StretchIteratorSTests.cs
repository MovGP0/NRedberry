using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchIteratorSTests
{
    [Fact]
    public void ShouldEnumerateGroupedShortStretches()
    {
        StretchIteratorS iterator = new(new short[] { 1, 2, 2, 2, 3, 3 });

        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(0, 1), iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(1, 3), iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(4, 2), iterator.Current);
        Assert.False(iterator.MoveNext());
    }

    [Fact]
    public void ShouldResetShortIterator()
    {
        StretchIteratorS iterator = new(new short[] { 5, 5, 5 });

        Assert.True(iterator.MoveNext());
        iterator.Reset();
        Assert.True(iterator.MoveNext());
        Assert.Equal(new Stretch(0, 3), iterator.Current);
    }
}
