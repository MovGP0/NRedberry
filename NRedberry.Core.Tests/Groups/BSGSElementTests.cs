using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class BSGSElementTests
{
    [Fact(DisplayName = "Should expose constructor state")]
    public void ShouldExposeConstructorState()
    {
        // Arrange
        Permutation identity = GroupPermutations.CreateIdentityPermutation(3);
        SchreierVector vector = new(4);
        vector[0] = -1;
        List<int> orbit = [0];

        // Act
        BSGSElement element = new(0, [identity], vector, orbit);

        // Assert
        Assert.Equal(0, element.BasePoint);
        Assert.Equal(1, element.OrbitSize);
        Assert.Equal(3, element.InternalDegree);
    }

    [Fact(DisplayName = "Should throw for point outside orbit")]
    public void ShouldThrowForPointOutsideOrbit()
    {
        // Arrange
        BSGSCandidateElement candidate = new(
            0,
            [GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(2, 0, 1))]);
        BSGSElement element = candidate.AsBSGSElement();

        // Act + Assert
        Assert.Throws<ArgumentException>(() => element.GetInverseTransversalOf(10));
    }

    [Fact(DisplayName = "Should return self in AsBSGSElement")]
    public void ShouldReturnSelfInAsBsgsElement()
    {
        // Arrange
        BSGSCandidateElement candidate = new(
            0,
            [GroupPermutations.CreateIdentityPermutation(2)]);
        BSGSElement element = candidate.AsBSGSElement();

        // Act
        BSGSElement converted = element.AsBSGSElement();

        // Assert
        Assert.Same(element, converted);
    }
}
