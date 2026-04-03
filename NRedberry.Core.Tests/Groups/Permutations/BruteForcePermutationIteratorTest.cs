using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups.Permutations;

public sealed class BruteForcePermutationIteratorTests
{
    [Fact(DisplayName = "Should not loop indefinitely")]
    public void ShouldNotLoopIndefinitely()
    {
        // Arrange
        List<Permutation> generators =
        [
            GroupPermutations.CreateIdentityPermutation(3)
        ];
        BruteForcePermutationIterator iterator = new(generators);
        int iterations = 0;
        const int maxIterations = 10;
        bool finished = false;

        // Act
        while (iterations < maxIterations)
        {
            if (!iterator.MoveNext())
            {
                finished = true;
                break;
            }

            iterations++;
        }

        // Assert
        finished.ShouldBeTrue();
        iterations.ShouldBe(1);
    }

    [Fact(DisplayName = "Should throw for inconsistent generators")]
    public void ShouldThrowForInconsistentGenerators()
    {
        // Arrange
        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1)),
            GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(3))
        ];
        BruteForcePermutationIterator iterator = new(generators);

        // Act + Assert
        Should.Throw<InconsistentGeneratorsException>(() =>
        {
            while (iterator.MoveNext())
            {
            }
        });
    }
}
