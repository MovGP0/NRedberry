using NRedberry.Core.Combinatorics;
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

        orbitSize.ShouldBe(2);
    }

    [Fact(DisplayName = "Should infer degree and return orbit size")]
    public void ShouldInferDegreeAndReturnOrbitSize()
    {
        Permutation[] generators =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateCycle(4))
        ];

        int orbitSize = GroupPermutations.GetOrbitSize(generators, 2);

        orbitSize.ShouldBe(4);
    }

    [Fact(DisplayName = "Should return singleton orbit size when generators are null and degree is provided")]
    public void ShouldReturnSingletonOrbitSizeWhenGeneratorsAreNullAndDegreeIsProvided()
    {
        int orbitSize = GroupPermutations.GetOrbitSize(null!, 3, 10);

        orbitSize.ShouldBe(1);
    }

    [Fact(DisplayName = "Should throw when generators are null and degree is inferred")]
    public void ShouldThrowWhenGeneratorsAreNullAndDegreeIsInferred()
    {
        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => _ = GroupPermutations.GetOrbitSize(null!, 0));

        exception.ParamName.ShouldBe("source");
    }
}
