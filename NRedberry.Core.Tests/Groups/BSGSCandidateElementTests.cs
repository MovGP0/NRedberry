using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
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
        element.OrbitSize.ShouldBe(2);
        element.BelongsToOrbit(0).ShouldBeTrue();
        element.BelongsToOrbit(1).ShouldBeTrue();
        element.BelongsToOrbit(2).ShouldBeFalse();
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
        element.OrbitSize.ShouldBe(4);
        element.BelongsToOrbit(2).ShouldBeTrue();
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
        basePointStabilizers.Count.ShouldBe(1);
        basePointStabilizers[0].ShouldBeSameAs(identity);
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
        original.OrbitSize.ShouldBe(2);
        clone.OrbitSize.ShouldBe(4);
        original.BelongsToOrbit(2).ShouldBeFalse();
        clone.BelongsToOrbit(2).ShouldBeTrue();
    }
}
