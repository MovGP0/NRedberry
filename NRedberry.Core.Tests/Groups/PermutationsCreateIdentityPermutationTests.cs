using NRedberry.Core.Combinatorics;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateIdentityPermutationTests
{
    [Fact(DisplayName = "Should throw for negative degree")]
    public void ShouldThrowForNegativeDegree()
    {
        var exception = Should.Throw<ArgumentException>(() => _ = GroupPermutations.CreateIdentityPermutation(-1));

        exception.ParamName.ShouldBe("degree");
    }

    [Fact(DisplayName = "Should create identity permutation with requested degree")]
    public void ShouldCreateIdentityPermutationWithRequestedDegree()
    {
        const int degree = 6;

        Permutation permutation = GroupPermutations.CreateIdentityPermutation(degree);

        permutation.IsIdentity.ShouldBeTrue();
        permutation.IsAntisymmetry.ShouldBeFalse();
        permutation.Degree.ShouldBe(degree);
        permutation.Length.ShouldBe(degree);
        permutation.OneLine().ShouldBe([0, 1, 2, 3, 4, 5]);
    }

    [Fact(DisplayName = "Should cache identities for degrees below cache limit")]
    public void ShouldCacheIdentitiesForDegreesBelowCacheLimit()
    {
        Permutation first = GroupPermutations.CreateIdentityPermutation(127);
        Permutation second = GroupPermutations.CreateIdentityPermutation(127);

        second.ShouldBeSameAs(first);
    }

    [Fact(DisplayName = "Should not cache identities for degrees at or above cache limit")]
    public void ShouldNotCacheIdentitiesForDegreesAtOrAboveCacheLimit()
    {
        Permutation first = GroupPermutations.CreateIdentityPermutation(128);
        Permutation second = GroupPermutations.CreateIdentityPermutation(128);

        second.ShouldNotBeSameAs(first);
        second.ShouldBe(first);
    }

    [Fact(DisplayName = "Should return default identity permutation")]
    public void ShouldReturnDefaultIdentityPermutation()
    {
        Permutation fromGetter = GroupPermutations.GetIdentityPermutation();
        Permutation fromFactory = GroupPermutations.CreateIdentityPermutation(GroupPermutations.DefaultIdentityLength);

        fromGetter.ShouldBeSameAs(fromFactory);
        fromGetter.IsIdentity.ShouldBeTrue();
        fromGetter.Degree.ShouldBe(GroupPermutations.DefaultIdentityLength);
    }
}
