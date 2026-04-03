using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationOneLineByteTests
{
    [Fact]
    public void ShouldCloneInputAndExposeCoreProperties()
    {
        sbyte[] source = [(sbyte)1, (sbyte)0, (sbyte)2];
        var permutation = new PermutationOneLineByte(false, source);
        source[0] = 0;

        permutation.OneLine().ShouldBe([1, 0, 2]);
        permutation.OneLineImmutable().ToArray().ShouldBe([1, 0, 2]);
        permutation.ToArray().ShouldBe([(sbyte)1, (sbyte)0, (sbyte)2]);
        permutation.Length.ShouldBe(3);
        permutation.Degree.ShouldBe(3);
        permutation.IsIdentity.ShouldBeFalse();
        permutation.IsAntisymmetry.ShouldBeFalse();
    }

    [Fact]
    public void ShouldThrowForIncorrectOrInconsistentConstruction()
    {
        Should.Throw<ArgumentException>(() => _ = new PermutationOneLineByte(false, (sbyte)1, (sbyte)1));
        Should.Throw<ArgumentException>(() => _ = new PermutationOneLineByte(true, (sbyte)1, (sbyte)2, (sbyte)0));
    }

    [Fact]
    public void ShouldPermuteArraysAndLists()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);

        permutation.Permute(new int[] { 10, 20, 30 }).ShouldBe([20, 10, 30]);
        permutation.Permute(new char[] { 'a', 'b', 'c' }).ShouldBe(['b', 'a', 'c']);
        permutation.Permute(new string[] { "a", "b", "c" }).ShouldBe(["b", "a", "c"]);
        permutation.Permute(new List<int> { 5, 6, 7 }).ShouldBe([6, 5, 7]);
        permutation.ImageOf([0, 1, 2, 5]).ShouldBe([1, 0, 2, 5]);
    }

    [Fact]
    public void ShouldSupportInverseAndCompositionWithInverse()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)2, (sbyte)0, (sbyte)1);

        Permutation inverse = permutation.Inverse();

        inverse.OneLine().ShouldBe([1, 2, 0]);
        permutation.Composition(inverse).IsIdentity.ShouldBeTrue();
        permutation.CompositionWithInverse(permutation).IsIdentity.ShouldBeTrue();
        permutation.NewIndexOfUnderInverse(0).ShouldBe(1);
        permutation.NewIndexOfUnderInverse(1).ShouldBe(2);
        permutation.NewIndexOfUnderInverse(2).ShouldBe(0);
        permutation.NewIndexOfUnderInverse(7).ShouldBe(7);
    }

    [Fact]
    public void ShouldComputePowersForPositiveZeroAndNegativeExponents()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)2, (sbyte)0);

        permutation.Pow(0).IsIdentity.ShouldBeTrue();
        permutation.Pow(2).OneLine().ShouldBe([2, 0, 1]);
        permutation.Pow(-1).OneLine().ShouldBe(permutation.Inverse().OneLine());
        permutation.Pow(3).IsIdentity.ShouldBeTrue();
    }

    [Fact]
    public void ShouldHandleSignOperations()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);

        Permutation negated = permutation.Negate();
        Permutation symmetry = negated.ToSymmetry();

        negated.IsAntisymmetry.ShouldBeTrue();
        negated.OneLine().ShouldBe(permutation.OneLine());
        symmetry.IsAntisymmetry.ShouldBeFalse();
        permutation.Equals(symmetry).ShouldBeTrue();

        var oddOrderPermutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)2, (sbyte)0);
        Should.Throw<InconsistentGeneratorsException>(() => _ = oddOrderPermutation.Negate());
    }

    [Fact]
    public void ShouldConvertToLargerRepresentationsByRequestedLength()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0);

        permutation.ToLargerRepresentation(short.MaxValue).ShouldBeOfType<PermutationOneLineShort>();
        permutation.ToLargerRepresentation(short.MaxValue + 1).ShouldBeOfType<PermutationOneLineInt>();
        permutation.ToShortRepresentation().ShouldBeOfType<PermutationOneLineShort>();
        permutation.ToIntRepresentation().ShouldBeOfType<PermutationOneLineInt>();
    }

    [Fact]
    public void ShouldMoveRightAndShiftAction()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);

        Permutation moved = permutation.MoveRight(2);

        permutation.MoveRight(0).ShouldBeSameAs(permutation);
        moved.OneLine().ShouldBe([0, 1, 3, 2, 4]);
        moved.Degree.ShouldBe(5);
    }

    [Fact]
    public void ShouldSupportEqualityHashCodeAndComparison()
    {
        var first = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);
        Permutation second = global::NRedberry.Groups.Permutations.CreatePermutation(1, 0, 2);
        var identity = new PermutationOneLineByte(false, (sbyte)0, (sbyte)1, (sbyte)2);
        var antisymmetry = new PermutationOneLineByte(true, (sbyte)1, (sbyte)0, (sbyte)2);

        first.Equals(second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
        first.Equals(identity).ShouldBeFalse();
        first.CompareTo(antisymmetry).ShouldBe(1);
        antisymmetry.CompareTo(first).ShouldBe(-1);
    }
}
