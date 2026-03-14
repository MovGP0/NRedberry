using Shouldly;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class IntCombinationsGeneratorTests
{
    [Fact]
    public void ShouldEnumerateCombinationsInLexicographicOrder()
    {
        IntCombinationsGenerator generator = new(4, 2);

        List<int[]> visited = [];
        int[]? current;
        while ((current = generator.Take()) is not null)
        {
            visited.Add((int[])current.Clone());
        }

        int[][] expected =
        [
            [0, 1],
            [0, 2],
            [0, 3],
            [1, 2],
            [1, 3],
            [2, 3],
        ];

        visited.ShouldBe(expected);
    }

    [Fact]
    public void ShouldExposeEnumerableContractViaBaseGenerator()
    {
        IntCombinatorialGenerator generator = new IntCombinationsGenerator(3, 2);
        IEnumerator<int[]> enumerator = generator.GetEnumerator();

        enumerator.ShouldBeSameAs(generator);
        enumerator.MoveNext().ShouldBeTrue();
        enumerator.Current.ShouldBe([0, 1]);
    }
}
