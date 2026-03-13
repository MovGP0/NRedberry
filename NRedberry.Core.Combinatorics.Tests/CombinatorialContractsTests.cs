using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class CombinatorialContractsTests
{
    [Fact]
    public void ShouldExposeGeneratorEnumeratorContract()
    {
        IIntCombinatorialGenerator generator = new IntCombinationPermutationGenerator(3, 2);

        Assert.Same(generator, generator.GetEnumerator());
        Assert.True(generator.MoveNext());
        Assert.Equal([0, 1], generator.Current);
    }

    [Fact]
    public void ShouldExposePortTakeAndResetContract()
    {
        IIntCombinatorialPort port = new IntCombinationPermutationGenerator(3, 2);

        int[]? firstReference = port.Take();
        int[]? secondReference = port.Take();
        port.Reset();
        int[]? resetFirst = port.Take();

        Assert.NotNull(firstReference);
        Assert.NotNull(secondReference);

        int[] first = (int[])firstReference.Clone();
        Assert.NotNull(resetFirst);

        Assert.Equal([0, 1], first);
        Assert.Equal([0, 1], resetFirst);
        Assert.Same(secondReference, port.GetReference());
        Assert.Same(resetFirst, port.GetReference());
    }
}

public sealed class InconsistentGeneratorsExceptionTests
{
    [Fact]
    public void ShouldExposeStandardExceptionConstructors()
    {
        InvalidOperationException inner = new("inner");
        InconsistentGeneratorsException withoutMessage = new();
        InconsistentGeneratorsException withMessage = new("broken");
        InconsistentGeneratorsException withInner = new("outer", inner);

        Assert.Contains(nameof(InconsistentGeneratorsException), withoutMessage.Message, StringComparison.Ordinal);
        Assert.Equal("broken", withMessage.Message);
        Assert.Same(inner, withInner.InnerException);
        Assert.Equal("outer", withInner.Message);
    }
}
