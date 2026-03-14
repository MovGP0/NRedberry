using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchIteratorITests
{
    [Fact]
    public void ShouldEnumerateIntegerStretchesAndSupportReset()
    {
        StretchIteratorI iterator = new([1, 1, 2, 2, 2]);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(0, 2));
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(2, 3));
        iterator.MoveNext().ShouldBeFalse();

        iterator.Reset();
        iterator.HasNext().ShouldBeTrue();
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(new Stretch(0, 2));
    }

    [Fact]
    public void ShouldThrowForUnsupportedRemoveAndInvalidCurrent()
    {
        StretchIteratorI iterator = new([1, 2]);

        Should.Throw<InvalidOperationException>(() => _ = iterator.Current);
        Should.Throw<NotSupportedException>(() => iterator.Remove());
    }
}
