using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchIteratorTests
{
    [Fact]
    public void ShouldGroupAdjacentValuesFromProvider()
    {
        StretchIterator iterator = new([1, 1, 2, 3, 3], IIntObjectProvider.HashProvider);

        iterator.HasNext().ShouldBeTrue();
        iterator.Next().ShouldBe(new Stretch(0, 2));
        iterator.HasNext().ShouldBeTrue();
        iterator.Next().ShouldBe(new Stretch(2, 1));
        iterator.HasNext().ShouldBeTrue();
        iterator.Next().ShouldBe(new Stretch(3, 2));
        iterator.HasNext().ShouldBeFalse();
    }

    [Fact]
    public void ShouldEnumerateHashBasedStretches()
    {
        Stretch[] actual = StretchIterator.GoHash([1, 1, 2, 2, 3]).ToArray();

        actual.ShouldBe([new Stretch(0, 2), new Stretch(2, 2), new Stretch(4, 1)]);
    }

    [Fact]
    public void ShouldThrowWhenRemoveIsRequested()
    {
        StretchIterator iterator = new(["a"], IIntObjectProvider.HashProvider);

        Should.Throw<NotSupportedException>(() => iterator.Remove());
    }
}
