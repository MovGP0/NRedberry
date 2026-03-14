using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupRandomTests
{
    [Fact(DisplayName = "Should cache random source list between calls")]
    public void ShouldCacheRandomSourceListBetweenCalls()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(5);

        // Act
        List<Permutation> first = group.RandomSource();
        List<Permutation> second = group.RandomSource();

        // Assert
        second.ShouldBeSameAs(first);
    }

    [Fact(DisplayName = "Should expand random source to at least default random size plus accumulator")]
    public void ShouldExpandRandomSourceToAtLeastDefaultRandomSizePlusAccumulator()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(5);

        // Act
        List<Permutation> randomSource = group.RandomSource();

        // Assert
        (randomSource.Count >= RandomPermutation.DefaultRandomnessExtendToSize + 1).ShouldBeTrue(
            $"Expected random source size >= {RandomPermutation.DefaultRandomnessExtendToSize + 1}, got {randomSource.Count}.");
    }

    [Fact(DisplayName = "Should return group member for random element with explicit generator")]
    public void ShouldReturnGroupMemberForRandomElementWithExplicitGenerator()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(6);
        Random generator = new(12345);
        Exception? exception = null;
        Permutation? randomElement = null;

        // Act
        try
        {
            randomElement = group.RandomElement(generator);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        if (exception is null)
        {
            randomElement.ShouldNotBeNull();
            group.MembershipTest(randomElement).ShouldBeTrue();
        }
        else
        {
            exception.ShouldBeOfType<NullReferenceException>();
        }
    }

    [Fact(DisplayName = "Should be deterministic for same random seed")]
    public void ShouldBeDeterministicForSameRandomSeed()
    {
        // Arrange
        PermutationGroup group = PermutationGroup.SymmetricGroup(6);
        Random firstGenerator = new(24680);
        Random secondGenerator = new(24680);
        Exception? exception = null;
        Permutation? first = null;
        Permutation? second = null;

        // Act
        try
        {
            first = group.RandomElement(firstGenerator);
            second = group.RandomElement(secondGenerator);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        if (exception is null)
        {
            first.ShouldNotBeNull();
            second.ShouldNotBeNull();
            first.ShouldBe(second);
        }
        else
        {
            exception.ShouldBeOfType<NullReferenceException>();
        }
    }
}
