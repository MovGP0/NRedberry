using System.Linq;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationOneLineByteTests
{
    [Fact]
    public void ShouldCloneInputAndExposeCoreProperties()
    {
        sbyte[] source = [(sbyte)1, (sbyte)0, (sbyte)2];
        var permutation = new PermutationOneLineByte(false, source);
        source[0] = 0;

        Assert.Equal(new[] { 1, 0, 2 }, permutation.OneLine());
        Assert.Equal(new[] { 1, 0, 2 }, permutation.OneLineImmutable().ToArray());
        Assert.Equal(new sbyte[] { 1, 0, 2 }, permutation.ToArray());
        Assert.Equal(3, permutation.Length);
        Assert.Equal(3, permutation.Degree);
        Assert.False(permutation.IsIdentity);
        Assert.False(permutation.IsAntisymmetry);
    }

    [Fact]
    public void ShouldThrowForIncorrectOrInconsistentConstruction()
    {
        Assert.Throws<ArgumentException>(() => _ = new PermutationOneLineByte(false, (sbyte)1, (sbyte)1));
        Assert.Throws<ArgumentException>(() => _ = new PermutationOneLineByte(true, (sbyte)1, (sbyte)2, (sbyte)0));
    }

    [Fact]
    public void ShouldPermuteArraysAndLists()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);

        Assert.Equal(new[] { 20, 10, 30 }, permutation.Permute(new[] { 10, 20, 30 }));
        Assert.Equal(new[] { 'b', 'a', 'c' }, permutation.Permute(new[] { 'a', 'b', 'c' }));
        Assert.Equal(new[] { "b", "a", "c" }, permutation.Permute(new[] { "a", "b", "c" }));
        Assert.Equal(new[] { 6, 5, 7 }, permutation.Permute(new List<int> { 5, 6, 7 }));
        Assert.Equal(new[] { 1, 0, 2, 5 }, permutation.ImageOf(new[] { 0, 1, 2, 5 }));
    }

    [Fact]
    public void ShouldSupportInverseAndCompositionWithInverse()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)2, (sbyte)0, (sbyte)1);

        Permutation inverse = permutation.Inverse();

        Assert.Equal(new[] { 1, 2, 0 }, inverse.OneLine());
        Assert.True(permutation.Composition(inverse).IsIdentity);
        Assert.True(permutation.CompositionWithInverse(permutation).IsIdentity);
        Assert.Equal(1, permutation.NewIndexOfUnderInverse(0));
        Assert.Equal(2, permutation.NewIndexOfUnderInverse(1));
        Assert.Equal(0, permutation.NewIndexOfUnderInverse(2));
        Assert.Equal(7, permutation.NewIndexOfUnderInverse(7));
    }

    [Fact]
    public void ShouldComputePowersForPositiveZeroAndNegativeExponents()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)2, (sbyte)0);

        Assert.True(permutation.Pow(0).IsIdentity);
        Assert.Equal(new[] { 2, 0, 1 }, permutation.Pow(2).OneLine());
        Assert.Equal(permutation.Inverse().OneLine(), permutation.Pow(-1).OneLine());
        Assert.True(permutation.Pow(3).IsIdentity);
    }

    [Fact]
    public void ShouldHandleSignOperations()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);

        Permutation negated = permutation.Negate();
        Permutation symmetry = negated.ToSymmetry();

        Assert.True(negated.IsAntisymmetry);
        Assert.Equal(permutation.OneLine(), negated.OneLine());
        Assert.False(symmetry.IsAntisymmetry);
        Assert.True(permutation.Equals(symmetry));

        var oddOrderPermutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)2, (sbyte)0);
        Assert.Throws<InconsistentGeneratorsException>(() => _ = oddOrderPermutation.Negate());
    }

    [Fact]
    public void ShouldConvertToLargerRepresentationsByRequestedLength()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0);

        Assert.IsType<PermutationOneLineShort>(permutation.ToLargerRepresentation(short.MaxValue));
        Assert.IsType<PermutationOneLineInt>(permutation.ToLargerRepresentation(short.MaxValue + 1));
        Assert.IsType<PermutationOneLineShort>(permutation.ToShortRepresentation());
        Assert.IsType<PermutationOneLineInt>(permutation.ToIntRepresentation());
    }

    [Fact]
    public void ShouldMoveRightAndShiftAction()
    {
        var permutation = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);

        Permutation moved = permutation.MoveRight(2);

        Assert.Same(permutation, permutation.MoveRight(0));
        Assert.Equal(new[] { 0, 1, 3, 2, 4 }, moved.OneLine());
        Assert.Equal(5, moved.Degree);
    }

    [Fact]
    public void ShouldSupportEqualityHashCodeAndComparison()
    {
        var first = new PermutationOneLineByte(false, (sbyte)1, (sbyte)0, (sbyte)2);
        Permutation second = global::NRedberry.Groups.Permutations.CreatePermutation(1, 0, 2);
        var identity = new PermutationOneLineByte(false, (sbyte)0, (sbyte)1, (sbyte)2);
        var antisymmetry = new PermutationOneLineByte(true, (sbyte)1, (sbyte)0, (sbyte)2);

        Assert.True(first.Equals(second));
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        Assert.False(first.Equals(identity));
        Assert.Equal(1, first.CompareTo(antisymmetry));
        Assert.Equal(-1, antisymmetry.CompareTo(first));
    }
}
