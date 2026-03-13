using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchIteratorTests
{
    [Fact]
    public void ShouldGroupAdjacentValuesFromProvider()
    {
        StretchIterator iterator = new([1, 1, 2, 3, 3], IIntObjectProvider.HashProvider);

        Assert.True(iterator.HasNext());
        Assert.Equal(new Stretch(0, 2), iterator.Next());
        Assert.True(iterator.HasNext());
        Assert.Equal(new Stretch(2, 1), iterator.Next());
        Assert.True(iterator.HasNext());
        Assert.Equal(new Stretch(3, 2), iterator.Next());
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void ShouldEnumerateHashBasedStretches()
    {
        Stretch[] actual = StretchIterator.GoHash([1, 1, 2, 2, 3]).ToArray();

        Assert.Equal([new Stretch(0, 2), new Stretch(2, 2), new Stretch(4, 1)], actual);
    }

    [Fact]
    public void ShouldThrowWhenRemoveIsRequested()
    {
        StretchIterator iterator = new(["a"], IIntObjectProvider.HashProvider);

        Assert.Throws<NotSupportedException>(() => iterator.Remove());
    }
}
