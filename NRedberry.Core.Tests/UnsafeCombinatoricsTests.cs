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

        var typed = permutation.ShouldBeOfType<PermutationOneLineInt>();
        typed.IsIdentity.ShouldBeTrue();
        typed.Degree.ShouldBe(3);
        typed.OneLine().ShouldBe(new[] { 0, 1, 2 });
    }

    [Fact]
    public void ShouldCreateNonIdentityPermutation()
    {
        Permutation permutation = UnsafeCombinatorics.CreateUnsafe([1, 0]);

        var typed = permutation.ShouldBeOfType<PermutationOneLineInt>();
        typed.IsIdentity.ShouldBeFalse();
        typed.Degree.ShouldBe(2);
        typed.OneLine().ShouldBe(new[] { 1, 0 });
    }

    [Fact]
    public void ShouldCreateUnsafeSymmetry()
    {
        Symmetry symmetry = UnsafeCombinatorics.CreateUnsafe([1, 0], true);

        symmetry.IsAntisymmetry.ShouldBeTrue();
        symmetry.Degree.ShouldBe(2);
        symmetry.OneLine().ShouldBe(new[] { 1, 0 });
    }
}
