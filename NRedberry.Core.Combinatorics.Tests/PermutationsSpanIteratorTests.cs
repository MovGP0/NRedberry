using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class PermutationsSpanIteratorTests
{
    [Fact]
    public void ShouldEnumerateGeneratedSpanUntilDuplicatesExhaustIt()
    {
        PermutationsSpanIterator<Symmetry> iterator = new([new Symmetry([1, 0], false)]);

        Assert.True(iterator.MoveNext());
        Assert.Equal([0, 1], iterator.Current.OneLine());
        Assert.False(iterator.MoveNext());
    }

    [Fact]
    public void ShouldNotSupportReset()
    {
        PermutationsSpanIterator<Symmetry> iterator = new([new Symmetry([1, 0], false)]);

        Assert.Throws<NotSupportedException>(() => iterator.Reset());
    }
}
