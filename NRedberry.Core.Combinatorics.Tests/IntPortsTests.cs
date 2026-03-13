using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class IntPortsTests
{
    [Fact]
    public void ShouldEnumerateCartesianTuplesAndTrackUpdateDepth()
    {
        IntTuplesPort port = new(2, 3);
        int[]? firstReference = port.Take();
        Assert.NotNull(firstReference);
        int[] first = (int[])firstReference.Clone();
        int firstDepth = port.GetLastUpdateDepth();
        int[]? secondReference = port.Take();
        Assert.NotNull(secondReference);
        int[] second = (int[])secondReference.Clone();
        int secondDepth = port.GetLastUpdateDepth();
        int[]? thirdReference = port.Take();
        Assert.NotNull(thirdReference);
        int[] third = (int[])thirdReference.Clone();
        int[]? fourthReference = port.Take();
        Assert.NotNull(fourthReference);
        int[] fourth = (int[])fourthReference.Clone();
        int fourthDepth = port.GetLastUpdateDepth();

        Assert.Equal([0, 0], first);
        Assert.Equal(0, firstDepth);
        Assert.Equal([0, 1], second);
        Assert.Equal(1, secondDepth);
        Assert.Equal([0, 2], third);
        Assert.Equal([1, 0], fourth);
        Assert.Equal(0, fourthDepth);
    }

    [Fact]
    public void ShouldExposeUnsafeOutputPortContract()
    {
        IOutputPortUnsafe<int[]> port = new IntTuplesPort(1);
        int[]? first = port.Take();

        Assert.NotNull(first);
        Assert.Equal([0], first);
        Assert.Null(port.Take());
    }

    [Fact]
    public void ShouldEnumerateDistinctTuplesWithoutRepeatingValues()
    {
        IntDistinctTuplesPort port = new();
        int[] reference = port.GetReference();

        Assert.Empty(reference);
    }

    [Fact]
    public void ShouldEnumeratePermutationSpan()
    {
        IntPermutationsSpanGenerator generator = new([1, 0]);

        int[]? firstReference = generator.Take();
        Assert.NotNull(firstReference);
        int[] first = (int[])firstReference.Clone();
        int[]? second = generator.Take();
        generator.Reset();
        int[]? resetFirst = generator.Take();

        Assert.Null(second);
        Assert.NotNull(resetFirst);
        Assert.Equal("0,1", string.Join(",", first));
        Assert.Equal("0,1", string.Join(",", resetFirst));
    }
}
