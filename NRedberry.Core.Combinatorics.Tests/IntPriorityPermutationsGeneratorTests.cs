using NRedberry.Core.Combinatorics.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class IntPriorityPermutationsGeneratorTests
{
    [Fact]
    public void ShouldReplayNicedPermutationsAfterReset()
    {
        IntPriorityPermutationsGenerator generator = new(2);

        int[]? firstReference = generator.Take();
        Assert.NotNull(firstReference);
        int[] first = (int[])firstReference.Clone();
        generator.Nice();
        int[]? secondReference = generator.Take();
        Assert.NotNull(secondReference);
        int[] second = (int[])secondReference.Clone();
        generator.Nice();

        generator.Reset();
        int[]? replayFirstReference = generator.Take();
        Assert.NotNull(replayFirstReference);
        int[] replayFirst = (int[])replayFirstReference.Clone();
        int[]? replaySecondReference = generator.Take();
        Assert.NotNull(replaySecondReference);
        int[] replaySecond = (int[])replaySecondReference.Clone();
        int[]? exhausted = generator.Take();

        Assert.Equal([0, 1], first);
        Assert.Equal([1, 0], second);
        Assert.Equal([0, 1], replayFirst);
        Assert.Equal([1, 0], replaySecond);
        Assert.Null(exhausted);
        Assert.Equal([1, 0], generator.GetReference());
    }
}
