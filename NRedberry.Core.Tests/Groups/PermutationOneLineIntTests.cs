using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationOneLineIntTests
{
    [Fact(DisplayName = "Should throw for inconsistent one-line permutation")]
    public void ShouldThrowForInconsistentOneLinePermutation()
    {
        Should.Throw<ArgumentException>(() => _ = new PermutationOneLineInt(0, 0));
    }

    [Fact(DisplayName = "Should throw for antisymmetry with odd-order permutation")]
    public void ShouldThrowForAntisymmetryWithOddOrderPermutation()
    {
        Should.Throw<ArgumentException>(() => _ = new PermutationOneLineInt(true, 1, 2, 0));
    }

    [Fact(DisplayName = "Should clone one-line array in constructor and output")]
    public void ShouldCloneOneLineArrayInConstructorAndOutput()
    {
        int[] source = [1, 0, 2];
        var permutation = new PermutationOneLineInt(source);

        source[0] = 0;
        int[] oneLine = permutation.OneLine();
        oneLine[1] = 1;

        permutation.OneLine().ShouldBe([1, 0, 2]);
        permutation.OneLineImmutable().ToArray().ShouldBe([1, 0, 2]);
    }

    [Fact(DisplayName = "Should compute degree and map out-of-range indices as identity")]
    public void ShouldComputeDegreeAndMapOutOfRangeIndicesAsIdentity()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0, 3);

        permutation.Degree.ShouldBe(4);
        permutation.NewIndexOf(3).ShouldBe(3);
        permutation.ImageOf(3).ShouldBe(3);
    }

    [Fact(DisplayName = "Should compute inverse and recover indices under inverse mapping")]
    public void ShouldComputeInverseAndRecoverIndicesUnderInverseMapping()
    {
        var permutation = new PermutationOneLineInt(2, 0, 1);

        var inverse = permutation.Inverse().ShouldBeOfType<PermutationOneLineInt>();

        inverse.OneLine().ShouldBe([1, 2, 0]);
        permutation.NewIndexOfUnderInverse(1).ShouldBe(2);
        permutation.NewIndexOfUnderInverse(10).ShouldBe(10);
    }

    [Fact(DisplayName = "Should compose and compose with inverse consistently")]
    public void ShouldComposeAndComposeWithInverseConsistently()
    {
        var left = new PermutationOneLineInt(1, 2, 0);
        var right = new PermutationOneLineInt(2, 0, 1);

        var composition = left.Composition(right).ShouldBeOfType<PermutationOneLineInt>();
        var withInverse = left.CompositionWithInverse(right).ShouldBeOfType<PermutationOneLineInt>();

        composition.IsIdentity.ShouldBeTrue();
        withInverse.OneLine().ShouldBe(right.OneLine());
    }

    [Fact(DisplayName = "Should power permutation for positive zero and negative exponents")]
    public void ShouldPowerPermutationForPositiveZeroAndNegativeExponents()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0);

        var squared = permutation.Pow(2).ShouldBeOfType<PermutationOneLineInt>();
        var zeroPower = permutation.Pow(0);
        var negativePower = permutation.Pow(-1);

        squared.OneLine().ShouldBe([2, 0, 1]);
        zeroPower.IsIdentity.ShouldBeTrue();
        negativePower.OneLine().ShouldBe(permutation.Inverse().OneLine());
    }

    [Fact(DisplayName = "Should toggle antisymmetry through negate and to-symmetry")]
    public void ShouldToggleAntisymmetryThroughNegateAndToSymmetry()
    {
        var symmetry = new PermutationOneLineInt(1, 0, 2);

        var antisymmetry = symmetry.Negate().ShouldBeOfType<PermutationOneLineInt>();
        var backToSymmetry = antisymmetry.ToSymmetry().ShouldBeOfType<PermutationOneLineInt>();

        antisymmetry.IsAntisymmetry.ShouldBeTrue();
        backToSymmetry.IsAntisymmetry.ShouldBeFalse();
        backToSymmetry.OneLine().ShouldBe(symmetry.OneLine());
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

        permutedInts.ShouldBe([20, 30, 10]);
        permutedChars.ShouldBe(['b', 'c', 'a']);
        permutedStrings.ShouldBe(["y", "z", "x"]);
        permutedList.ShouldBe([8, 9, 7]);
        image.ShouldBe([1, 2, 0, 5]);
    }

    [Fact(DisplayName = "Should move permutation right and preserve sign and mapping shift")]
    public void ShouldMovePermutationRightAndPreserveSignAndMappingShift()
    {
        var permutation = new PermutationOneLineInt(true, 1, 0);

        var moved = permutation.MoveRight(3).ShouldBeOfType<PermutationOneLineInt>();

        moved.IsAntisymmetry.ShouldBeTrue();
        moved.Degree.ShouldBe(5);
        moved.OneLine().ShouldBe([0, 1, 2, 4, 3]);
    }

    [Fact(DisplayName = "Should expose parity cycle lengths and cycle-string representation")]
    public void ShouldExposeParityCycleLengthsAndCycleStringRepresentation()
    {
        var permutation = new PermutationOneLineInt(1, 2, 0, 3);

        permutation.Parity.ShouldBe(0);
        permutation.LengthsOfCycles.ShouldBe([3]);
        permutation.ToStringCycles().ShouldBe("+{0, 1, 2}");
    }

    [Fact(DisplayName = "Should support equality hash code and comparison contracts")]
    public void ShouldSupportEqualityHashCodeAndComparisonContracts()
    {
        var left = new PermutationOneLineInt(1, 0, 2);
        var same = new PermutationOneLineInt(1, 0, 2);
        var antisymmetry = new PermutationOneLineInt(true, 1, 0, 2);
        var different = new PermutationOneLineInt(0, 2, 1);

        left.ShouldBe(same);
        left.GetHashCode().ShouldBe(same.GetHashCode());
        left.Equals(antisymmetry).ShouldBeFalse();
        left.Equals(different).ShouldBeFalse();
        (left.CompareTo(different) > 0).ShouldBeTrue();
        (antisymmetry.CompareTo(left) < 0).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should return same identity instance and create identity for non-identity")]
    public void ShouldReturnSameIdentityInstanceAndCreateIdentityForNonIdentity()
    {
        var identity = new PermutationOneLineInt(0, 1, 2);
        var nonIdentity = new PermutationOneLineInt(1, 0, 2);

        identity.Identity.ShouldBeSameAs(identity);
        nonIdentity.Identity.IsIdentity.ShouldBeTrue();
        nonIdentity.Identity.Length.ShouldBe(nonIdentity.Length);
    }
}
