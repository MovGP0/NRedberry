using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests;

public sealed class UnsafeCombinatoricsTests
{
    [Fact]
    public void ShouldCreateIdentityPermutation()
    {
        Permutation permutation = UnsafeCombinatorics.CreateUnsafe([0, 1, 2]);

        var typed = Assert.IsType<PermutationOneLineInt>(permutation);
        Assert.True(typed.IsIdentity);
        Assert.Equal(3, typed.Degree);
        Assert.Equal(new[] { 0, 1, 2 }, typed.OneLine());
    }

    [Fact]
    public void ShouldCreateNonIdentityPermutation()
    {
        Permutation permutation = UnsafeCombinatorics.CreateUnsafe([1, 0]);

        var typed = Assert.IsType<PermutationOneLineInt>(permutation);
        Assert.False(typed.IsIdentity);
        Assert.Equal(2, typed.Degree);
        Assert.Equal(new[] { 1, 0 }, typed.OneLine());
    }

    [Fact]
    public void ShouldCreateUnsafeSymmetry()
    {
        Symmetry symmetry = UnsafeCombinatorics.CreateUnsafe([1, 0], true);

        Assert.True(symmetry.IsAntisymmetry);
        Assert.Equal(2, symmetry.Degree);
        Assert.Equal(new[] { 1, 0 }, symmetry.OneLine());
    }
}
