using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class IntCombinationPermutationGeneratorTests
{
    [Fact]
    public void ShouldEnumerateCombinationPermutationsInExpectedOrder()
    {
        IntCombinationPermutationGenerator generator = new(3, 2);

        List<int[]> visited = [];
        int[]? current;
        while ((current = generator.Take()) is not null)
        {
            visited.Add((int[])current.Clone());
        }

        Assert.Equal(
            [
                [0, 1],
                [1, 0],
                [0, 2],
                [2, 0],
                [1, 2],
                [2, 1],
            ],
            visited);
    }

    [Fact]
    public void ShouldResetPortSequenceAndKeepReferenceStable()
    {
        IntCombinationPermutationGenerator generator = new(3, 2);

        int[] reference = generator.GetReference();
        int[]? first = generator.Take();
        _ = generator.Take();

        generator.Reset();
        int[]? resetFirst = generator.Take();

        Assert.NotNull(resetFirst);

        Assert.Same(reference, generator.GetReference());
        Assert.Same(reference, first);
        Assert.Equal([0, 1], resetFirst);
    }
}
