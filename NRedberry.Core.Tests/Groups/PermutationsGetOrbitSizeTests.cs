using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsGetOrbitSizeTests
{
    [Fact(DisplayName = "Should return orbit size with explicit degree")]
    public void ShouldReturnOrbitSizeWithExplicitDegree()
    {
        Permutation[] generators =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(5, 0, 1))
        ];

        int orbitSize = GroupPermutations.GetOrbitSize(generators, 0, 5);

        Assert.Equal(2, orbitSize);
    }

    [Fact(DisplayName = "Should infer degree and return orbit size")]
    public void ShouldInferDegreeAndReturnOrbitSize()
    {
        Permutation[] generators =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(4))
        ];

        int orbitSize = GroupPermutations.GetOrbitSize(generators, 2);

        Assert.Equal(4, orbitSize);
    }

    [Fact(DisplayName = "Should return singleton orbit size when generators are null and degree is provided")]
    public void ShouldReturnSingletonOrbitSizeWhenGeneratorsAreNullAndDegreeIsProvided()
    {
        int orbitSize = GroupPermutations.GetOrbitSize(null!, 3, 10);

        Assert.Equal(1, orbitSize);
    }

    [Fact(DisplayName = "Should throw when generators are null and degree is inferred")]
    public void ShouldThrowWhenGeneratorsAreNullAndDegreeIsInferred()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => _ = GroupPermutations.GetOrbitSize(null!, 0));

        Assert.Equal("source", exception.ParamName);
    }
}
