using Shouldly;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class PermutationsSpanIteratorTests
{
    [Fact]
    public void ShouldEnumerateGeneratedSpanUntilDuplicatesExhaustIt()
    {
        PermutationsSpanIterator<Symmetry> iterator = new([new Symmetry([1, 0], false)]);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.OneLine().ShouldBe([0, 1]);
        iterator.MoveNext().ShouldBeFalse();
    }

    [Fact]
    public void ShouldNotSupportReset()
    {
        PermutationsSpanIterator<Symmetry> iterator = new([new Symmetry([1, 0], false)]);

        Should.Throw<NotSupportedException>(() => iterator.Reset());
    }
}
