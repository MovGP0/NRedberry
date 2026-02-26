using NRedberry.IndexGeneration;
using Xunit;

namespace NRedberry.Core.Tests.IndexGeneration;

public sealed class IntGeneratorTests
{
    [Fact]
    public void DefaultGetNextShouldStartAtZeroAndIncrement()
    {
        var generator = new IntGenerator();

        Assert.Equal(0, generator.GetNext());
        Assert.Equal(1, generator.GetNext());
        Assert.Equal(2, generator.GetNext());
    }

    [Fact]
    public void EngagedDataShouldSkipOccupiedNumbersAndDeduplicateInputs()
    {
        var generator = new IntGenerator([5, 2, 2, 1, 1]);

        Assert.Equal(0, generator.GetNext());
        Assert.Equal(3, generator.GetNext());
        Assert.Equal(4, generator.GetNext());
        Assert.Equal(6, generator.GetNext());
    }

    [Fact]
    public void ContainsShouldTrackProducedAndEngagedIndices()
    {
        var generator = new IntGenerator([2, 5, 5]);

        Assert.Equal(0, generator.GetNext());

        Assert.True(generator.Contains(0));
        Assert.True(generator.Contains(2));
        Assert.True(generator.Contains(5));
        Assert.False(generator.Contains(6));
    }

    [Fact]
    public void CloneShouldPreserveStateAndAdvanceIndependently()
    {
        var generator = new IntGenerator([1, 3]);

        Assert.Equal(0, generator.GetNext());

        var clone = generator.Clone();

        Assert.Equal(2, generator.GetNext());
        Assert.Equal(4, generator.GetNext());

        Assert.Equal(2, clone.GetNext());
        Assert.False(clone.Contains(4));
        Assert.True(generator.Contains(4));

        Assert.Equal(4, clone.GetNext());
    }
}
