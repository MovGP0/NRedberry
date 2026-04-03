using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class IntPermutationsGeneratorTests
{
    [Fact]
    public void ShouldEnumerateAllPermutationsOfDimensionThree()
    {
        IntPermutationsGenerator generator = new(3);

        List<int[]> visited = [];
        int[]? current;
        while ((current = generator.Take()) is not null)
        {
            visited.Add((int[])current.Clone());
        }

        List<int[]> expected =
        [
            [0, 1, 2],
            [0, 2, 1],
            [1, 0, 2],
            [1, 2, 0],
            [2, 0, 1],
            [2, 1, 0],
        ];

        visited.ShouldBe(expected);
    }

    [Fact]
    public void ShouldResetAndWalkBackwards()
    {
        IntPermutationsGenerator generator = new(3);

        _ = generator.Take();
        _ = generator.Take();
        int[] previous = (int[])generator.Previous().Clone();
        generator.Reset();
        int[]? first = generator.Take();

        first.ShouldNotBeNull();
        previous.ShouldBe([0, 1, 2]);
        first.ShouldBe([0, 1, 2]);
    }
}
