using NRedberry.Core.Combinatorics.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

public sealed class IntPriorityPermutationsGeneratorTests
{
    [Fact]
    public void ShouldReplayNicedPermutationsAfterReset()
    {
        IntPriorityPermutationsGenerator generator = new(2);

        int[]? firstReference = generator.Take();
        firstReference.ShouldNotBeNull();
        int[] first = (int[])firstReference.Clone();
        generator.Nice();
        int[]? secondReference = generator.Take();
        secondReference.ShouldNotBeNull();
        int[] second = (int[])secondReference.Clone();
        generator.Nice();

        generator.Reset();
        int[]? replayFirstReference = generator.Take();
        replayFirstReference.ShouldNotBeNull();
        int[] replayFirst = (int[])replayFirstReference.Clone();
        int[]? replaySecondReference = generator.Take();
        replaySecondReference.ShouldNotBeNull();
        int[] replaySecond = (int[])replaySecondReference.Clone();
        int[]? exhausted = generator.Take();

        first.ShouldBe([0, 1]);
        second.ShouldBe([1, 0]);
        replayFirst.ShouldBe([0, 1]);
        replaySecond.ShouldBe([1, 0]);
        exhausted.ShouldBeNull();
        generator.GetReference().ShouldBe([1, 0]);
    }
}
