using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class BSGSCandidateElementTests
{
    [Fact(DisplayName = "Should initialize orbit from stabilizers")]
    public void ShouldInitializeOrbitFromStabilizers()
    {
        // Arrange
        List<Permutation> stabilizers =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(2, 0, 1))
        ];

        // Act
        BSGSCandidateElement element = new(0, stabilizers);

        // Assert
        Assert.Equal(2, element.OrbitSize);
        Assert.True(element.BelongsToOrbit(0));
        Assert.True(element.BelongsToOrbit(1));
        Assert.False(element.BelongsToOrbit(2));
    }

    [Fact(DisplayName = "Should expand orbit when adding stabilizer")]
    public void ShouldExpandOrbitWhenAddingStabilizer()
    {
        // Arrange
        List<Permutation> stabilizers =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1))
        ];
        BSGSCandidateElement element = new(0, stabilizers);
        Permutation cycle = GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(3));

        // Act
        element.AddStabilizer(cycle);

        // Assert
        Assert.Equal(4, element.OrbitSize);
        Assert.True(element.BelongsToOrbit(2));
    }

    [Fact(DisplayName = "Should return stabilizers fixing base point")]
    public void ShouldReturnStabilizersFixingBasePoint()
    {
        // Arrange
        Permutation identity = GroupPermutations.CreateIdentityPermutation(3);
        Permutation transposition = GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1));
        List<Permutation> stabilizers =
        [
            identity,
            transposition
        ];
        BSGSCandidateElement element = new(0, stabilizers);

        // Act
        List<Permutation> basePointStabilizers = element.GetStabilizersOfThisBasePoint();

        // Assert
        Assert.Single(basePointStabilizers);
        Assert.Same(identity, basePointStabilizers[0]);
    }

    [Fact(DisplayName = "Should clone independently from source element")]
    public void ShouldCloneIndependentlyFromSourceElement()
    {
        // Arrange
        List<Permutation> stabilizers =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(3, 0, 1))
        ];
        BSGSCandidateElement original = new(0, stabilizers);
        BSGSCandidateElement clone = original.Clone();
        Permutation cycle = GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(3));

        // Act
        clone.AddStabilizer(cycle);

        // Assert
        Assert.Equal(2, original.OrbitSize);
        Assert.Equal(4, clone.OrbitSize);
        Assert.False(original.BelongsToOrbit(2));
        Assert.True(clone.BelongsToOrbit(2));
    }
}
