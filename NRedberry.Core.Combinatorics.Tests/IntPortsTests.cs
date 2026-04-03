using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class IntPortsTests
{
    [Fact]
    public void ShouldEnumerateCartesianTuplesAndTrackUpdateDepth()
    {
        IntTuplesPort port = new(2, 3);
        int[]? firstReference = port.Take();
        firstReference.ShouldNotBeNull();
        int[] first = (int[])firstReference!.Clone();
        int firstDepth = port.GetLastUpdateDepth();
        int[]? secondReference = port.Take();
        secondReference.ShouldNotBeNull();
        int[] second = (int[])secondReference!.Clone();
        int secondDepth = port.GetLastUpdateDepth();
        int[]? thirdReference = port.Take();
        thirdReference.ShouldNotBeNull();
        int[] third = (int[])thirdReference!.Clone();
        int[]? fourthReference = port.Take();
        fourthReference.ShouldNotBeNull();
        int[] fourth = (int[])fourthReference!.Clone();
        int fourthDepth = port.GetLastUpdateDepth();

        first.ShouldBe([0, 0]);
        firstDepth.ShouldBe(0);
        second.ShouldBe([0, 1]);
        secondDepth.ShouldBe(1);
        third.ShouldBe([0, 2]);
        fourth.ShouldBe([1, 0]);
        fourthDepth.ShouldBe(0);
    }

    [Fact]
    public void ShouldExposeUnsafeOutputPortContract()
    {
        IOutputPortUnsafe<int[]> port = new IntTuplesPort(1);
        int[]? first = port.Take();

        first.ShouldNotBeNull();
        first.ShouldBe([0]);
        port.Take().ShouldBeNull();
    }

    [Fact]
    public void ShouldEnumerateDistinctTuplesWithoutRepeatingValues()
    {
        IntDistinctTuplesPort port = new();
        int[] reference = port.GetReference();

        reference.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldEnumeratePermutationSpan()
    {
        IntPermutationsSpanGenerator generator = new([1, 0]);

        int[]? firstReference = generator.Take();
        firstReference.ShouldNotBeNull();
        int[] first = (int[])firstReference!.Clone();
        int[]? second = generator.Take();
        generator.Reset();
        int[]? resetFirst = generator.Take();

        second.ShouldBeNull();
        resetFirst.ShouldNotBeNull();
        string.Join(",", first).ShouldBe("0,1");
        string.Join(",", resetFirst).ShouldBe("0,1");
    }
}
