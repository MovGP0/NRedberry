using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateIdentityPermutationTests
{
    [Fact(DisplayName = "Should throw for negative degree")]
    public void ShouldThrowForNegativeDegree()
    {
        var exception = Assert.Throws<ArgumentException>(() => _ = GroupPermutations.CreateIdentityPermutation(-1));

        Assert.Equal("degree", exception.ParamName);
    }

    [Fact(DisplayName = "Should create identity permutation with requested degree")]
    public void ShouldCreateIdentityPermutationWithRequestedDegree()
    {
        const int degree = 6;

        Permutation permutation = GroupPermutations.CreateIdentityPermutation(degree);

        Assert.True(permutation.IsIdentity);
        Assert.False(permutation.IsAntisymmetry);
        Assert.Equal(degree, permutation.Degree);
        Assert.Equal(degree, permutation.Length);
        Assert.Equal([0, 1, 2, 3, 4, 5], permutation.OneLine());
    }

    [Fact(DisplayName = "Should cache identities for degrees below cache limit")]
    public void ShouldCacheIdentitiesForDegreesBelowCacheLimit()
    {
        Permutation first = GroupPermutations.CreateIdentityPermutation(127);
        Permutation second = GroupPermutations.CreateIdentityPermutation(127);

        Assert.Same(first, second);
    }

    [Fact(DisplayName = "Should not cache identities for degrees at or above cache limit")]
    public void ShouldNotCacheIdentitiesForDegreesAtOrAboveCacheLimit()
    {
        Permutation first = GroupPermutations.CreateIdentityPermutation(128);
        Permutation second = GroupPermutations.CreateIdentityPermutation(128);

        Assert.NotSame(first, second);
        Assert.Equal(first, second);
    }

    [Fact(DisplayName = "Should return default identity permutation")]
    public void ShouldReturnDefaultIdentityPermutation()
    {
        Permutation fromGetter = GroupPermutations.GetIdentityPermutation();
        Permutation fromFactory = GroupPermutations.CreateIdentityPermutation(GroupPermutations.DefaultIdentityLength);

        Assert.Same(fromFactory, fromGetter);
        Assert.True(fromGetter.IsIdentity);
        Assert.Equal(GroupPermutations.DefaultIdentityLength, fromGetter.Degree);
    }
}
