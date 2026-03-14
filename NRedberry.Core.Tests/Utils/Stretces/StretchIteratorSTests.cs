using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchIteratorSTests
{
    [Fact]
    public void ShouldEnumerateGroupedShortStretches()
    {
        StretchIteratorS iterator = new([1, 2, 2, 2, 3, 3]);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(0, 1));
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(1, 3));
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(4, 2));
        iterator.MoveNext().ShouldBeFalse();
    }

    [Fact]
    public void ShouldResetShortIterator()
    {
        StretchIteratorS iterator = new([5, 5, 5]);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Reset();
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(0, 3));
    }
}
