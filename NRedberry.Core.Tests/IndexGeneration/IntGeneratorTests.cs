using NRedberry.IndexGeneration;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.IndexGeneration;

public sealed class IntGeneratorTests
{
    [Fact]
    public void DefaultGetNextShouldStartAtZeroAndIncrement()
    {
        var generator = new IntGenerator();

        generator.GetNext().ShouldBe(0);
        generator.GetNext().ShouldBe(1);
        generator.GetNext().ShouldBe(2);
    }

    [Fact]
    public void EngagedDataShouldSkipOccupiedNumbersAndDeduplicateInputs()
    {
        var generator = new IntGenerator([5, 2, 2, 1, 1]);

        generator.GetNext().ShouldBe(0);
        generator.GetNext().ShouldBe(3);
        generator.GetNext().ShouldBe(4);
        generator.GetNext().ShouldBe(6);
    }

    [Fact]
    public void ContainsShouldTrackProducedAndEngagedIndices()
    {
        var generator = new IntGenerator([2, 5, 5]);

        generator.GetNext().ShouldBe(0);

        generator.Contains(0).ShouldBeTrue();
        generator.Contains(2).ShouldBeTrue();
        generator.Contains(5).ShouldBeTrue();
        generator.Contains(6).ShouldBeFalse();
    }

    [Fact]
    public void CloneShouldPreserveStateAndAdvanceIndependently()
    {
        var generator = new IntGenerator([1, 3]);

        generator.GetNext().ShouldBe(0);

        var clone = generator.Clone();

        generator.GetNext().ShouldBe(2);
        generator.GetNext().ShouldBe(4);

        clone.GetNext().ShouldBe(2);
        clone.Contains(4).ShouldBeFalse();
        generator.Contains(4).ShouldBeTrue();

        clone.GetNext().ShouldBe(4);
    }
}
