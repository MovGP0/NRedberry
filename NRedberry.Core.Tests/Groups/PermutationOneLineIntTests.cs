using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationOneLineIntTests
{
    [Fact(DisplayName = "Should throw for inconsistent one-line permutation")]
    public void ShouldThrowForInconsistentOneLinePermutation()
    {
        Assert.Throws<ArgumentException>(() => _ = new PermutationOneLineInt(0, 0));
    }

    [Fact(DisplayName = "Should throw for antisymmetry with odd-order permutation")]
    public void ShouldThrowForAntisymmetryWithOddOrderPermutation()
    {
        Assert.Throws<ArgumentException>(() => _ = new PermutationOneLineInt(true, 1, 2, 0));
    }

    [Fact(DisplayName = "Should clone one-line array in constructor and output")]
    public void ShouldCloneOneLineArrayInConstructorAndOutput()
    {
        int[] source = [1, 0, 2];
        var permutation = new PermutationOneLineInt(source);

        source[0] = 0;
        int[] oneLine = permutation.OneLine();
        oneLine[1] = 1;

        Assert.Equal(new[] { 1, 0, 2 }, permutation.OneLine());
        Assert.Equal([1, 0, 2], permutation.OneLineImmutable().ToArray());
    }

    [Fact(DisplayName = "Should compute degree and map out-of-range indices as identity")]
    public void ShouldComputeDegreeAndMapOutOfRangeIndicesAsIdentity()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0, 3);

        Assert.Equal(4, permutation.Degree);
        Assert.Equal(3, permutation.NewIndexOf(3));
        Assert.Equal(3, permutation.ImageOf(3));
    }

    [Fact(DisplayName = "Should compute inverse and recover indices under inverse mapping")]
    public void ShouldComputeInverseAndRecoverIndicesUnderInverseMapping()
    {
        var permutation = new PermutationOneLineInt(2, 0, 1);

        var inverse = Assert.IsType<PermutationOneLineInt>(permutation.Inverse());

        Assert.Equal(new[] { 1, 2, 0 }, inverse.OneLine());
        Assert.Equal(2, permutation.NewIndexOfUnderInverse(1));
        Assert.Equal(10, permutation.NewIndexOfUnderInverse(10));
    }

    [Fact(DisplayName = "Should compose and compose with inverse consistently")]
    public void ShouldComposeAndComposeWithInverseConsistently()
    {
        var left = new PermutationOneLineInt(1, 2, 0);
        var right = new PermutationOneLineInt(2, 0, 1);

        var composition = Assert.IsType<PermutationOneLineInt>(left.Composition(right));
        var withInverse = Assert.IsType<PermutationOneLineInt>(left.CompositionWithInverse(right));

        Assert.True(composition.IsIdentity);
        Assert.Equal(right.OneLine(), withInverse.OneLine());
    }

    [Fact(DisplayName = "Should power permutation for positive zero and negative exponents")]
    public void ShouldPowerPermutationForPositiveZeroAndNegativeExponents()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0);

        var squared = Assert.IsType<PermutationOneLineInt>(permutation.Pow(2));
        var zeroPower = permutation.Pow(0);
        var negativePower = permutation.Pow(-1);

        Assert.Equal(new[] { 2, 0, 1 }, squared.OneLine());
        Assert.True(zeroPower.IsIdentity);
        Assert.Equal(permutation.Inverse().OneLine(), negativePower.OneLine());
    }

    [Fact(DisplayName = "Should toggle antisymmetry through negate and to-symmetry")]
    public void ShouldToggleAntisymmetryThroughNegateAndToSymmetry()
    {
        var symmetry = new PermutationOneLineInt(1, 0, 2);

        var antisymmetry = Assert.IsType<PermutationOneLineInt>(symmetry.Negate());
        var backToSymmetry = Assert.IsType<PermutationOneLineInt>(antisymmetry.ToSymmetry());

        Assert.True(antisymmetry.IsAntisymmetry);
        Assert.False(backToSymmetry.IsAntisymmetry);
        Assert.Equal(symmetry.OneLine(), backToSymmetry.OneLine());
    }

    [Fact(DisplayName = "Should permute arrays lists and index sets")]
    public void ShouldPermuteArraysListsAndIndexSets()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0);

        int[] permutedInts = permutation.Permute(new[] { 10, 20, 30 });
        char[] permutedChars = permutation.Permute(new[] { 'a', 'b', 'c' });
        string[] permutedStrings = permutation.Permute(new[] { "x", "y", "z" });
        List<int> permutedList = permutation.Permute(new List<int> { 7, 8, 9 });
        int[] image = permutation.ImageOf([0, 1, 2, 5]);

        Assert.Equal(new[] { 20, 30, 10 }, permutedInts);
        Assert.Equal(new[] { 'b', 'c', 'a' }, permutedChars);
        Assert.Equal(new[] { "y", "z", "x" }, permutedStrings);
        Assert.Equal(new[] { 8, 9, 7 }, permutedList);
        Assert.Equal(new[] { 1, 2, 0, 5 }, image);
    }

    [Fact(DisplayName = "Should move permutation right and preserve sign and mapping shift")]
    public void ShouldMovePermutationRightAndPreserveSignAndMappingShift()
    {
        var permutation = new PermutationOneLineInt(true, 1, 0);

        var moved = Assert.IsType<PermutationOneLineInt>(permutation.MoveRight(3));

        Assert.True(moved.IsAntisymmetry);
        Assert.Equal(5, moved.Degree);
        Assert.Equal(new[] { 0, 1, 2, 4, 3 }, moved.OneLine());
    }

    [Fact(DisplayName = "Should expose parity cycle lengths and cycle-string representation")]
    public void ShouldExposeParityCycleLengthsAndCycleStringRepresentation()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0, 3);

        Assert.Equal(0, permutation.Parity);
        Assert.Equal(new[] { 3 }, permutation.LengthsOfCycles);
        Assert.Equal("+{0, 1, 2}", permutation.ToStringCycles());
    }

    [Fact(DisplayName = "Should support equality hash code and comparison contracts")]
    public void ShouldSupportEqualityHashCodeAndComparisonContracts()
    {
        var left = new PermutationOneLineInt(1, 0, 2);
        var same = new PermutationOneLineInt(1, 0, 2);
        var antisymmetry = new PermutationOneLineInt(true, 1, 0, 2);
        var different = new PermutationOneLineInt(0, 2, 1);

        Assert.Equal(left, same);
        Assert.Equal(left.GetHashCode(), same.GetHashCode());
        Assert.False(left.Equals(antisymmetry));
        Assert.False(left.Equals(different));
        Assert.True(left.CompareTo(different) > 0);
        Assert.True(antisymmetry.CompareTo(left) < 0);
    }

    [Fact(DisplayName = "Should return same identity instance and create identity for non-identity")]
    public void ShouldReturnSameIdentityInstanceAndCreateIdentityForNonIdentity()
    {
        var identity = new PermutationOneLineInt(0, 1, 2);
        var nonIdentity = new PermutationOneLineInt(1, 0, 2);

        Assert.Same(identity, identity.Identity);
        Assert.True(nonIdentity.Identity.IsIdentity);
        Assert.Equal(nonIdentity.Length, nonIdentity.Identity.Length);
    }
}
