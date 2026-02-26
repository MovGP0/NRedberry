using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationOneLineShortTests
{
    [Fact(DisplayName = "Should clone source array and expose one-line data")]
    public void ShouldCloneSourceArrayAndExposeOneLineData()
    {
        // Arrange
        short[] source = [1, 0, 2];
        PermutationOneLineShort permutation = new(false, source);

        // Act
        source[0] = 2;
        int[] oneLine = permutation.OneLine();
        oneLine[1] = 2;
        ImmutableArray<int> oneLineImmutable = permutation.OneLineImmutable();

        // Assert
        Assert.Equal([1, 0, 2], permutation.OneLine());
        Assert.Equal([1, 0, 2], oneLineImmutable.ToArray());
        Assert.False(permutation.IsIdentity);
        Assert.False(permutation.IsAntisymmetry);
        Assert.Equal(3, permutation.Degree);
        Assert.Equal(3, permutation.Length);
    }

    [Fact(DisplayName = "Should reject inconsistent antisymmetric permutation")]
    public void ShouldRejectInconsistentAntisymmetricPermutation()
    {
        // Act + Assert
        Assert.Throws<ArgumentException>(() => new PermutationOneLineShort(true, 0, 1, 2));
        Assert.Throws<InconsistentGeneratorsException>(() => new PermutationOneLineShort(false, true, 3, [0, 1, 2]));
    }

    [Fact(DisplayName = "Should convert to int representation preserving action")]
    public void ShouldConvertToIntRepresentationPreservingAction()
    {
        // Arrange
        PermutationOneLineShort shortPermutation = new(false, 2, 0, 1);

        // Act
        PermutationOneLineInt intPermutation = shortPermutation.ToIntRepresentation();

        // Assert
        Assert.Equal(shortPermutation.OneLine(), intPermutation.OneLine());
        Assert.Equal(shortPermutation.Degree, intPermutation.Degree);
        Assert.Equal(shortPermutation.IsAntisymmetry, intPermutation.IsAntisymmetry);
        Assert.Equal(shortPermutation.NewIndexOf(0), intPermutation.NewIndexOf(0));
    }

    [Fact(DisplayName = "Should apply index lookup and permutation methods")]
    public void ShouldApplyIndexLookupAndPermutationMethods()
    {
        // Arrange
        PermutationOneLineShort permutation = new(false, 1, 0, 2);
        int[] set = [0, 1, 3];
        int[] values = [10, 20, 30];
        char[] chars = ['a', 'b', 'c'];
        string[] strings = ["A", "B", "C"];
        List<string> list = ["A", "B", "C"];

        // Act + Assert
        Assert.Equal(1, permutation.NewIndexOf(0));
        Assert.Equal(0, permutation.ImageOf(1));
        Assert.Equal(4, permutation.NewIndexOf(4));
        Assert.Equal(4, permutation.NewIndexOfUnderInverse(4));
        Assert.Equal([1, 0, 3], permutation.ImageOf(set));
        Assert.Equal([20, 10, 30], permutation.Permute(values));
        Assert.Equal(['b', 'a', 'c'], permutation.Permute(chars));
        Assert.Equal(["B", "A", "C"], permutation.Permute(strings));
        Assert.Equal(["B", "A", "C"], permutation.Permute(list));
    }

    [Fact(DisplayName = "Should compute composition inverse and powers")]
    public void ShouldComputeCompositionInverseAndPowers()
    {
        // Arrange
        PermutationOneLineShort left = new(false, 1, 0, 2);
        PermutationOneLineShort right = new(false, 0, 2, 1);

        // Act
        Permutation composition = left.Composition(right);
        Permutation inverse = left.Inverse();
        Permutation withInverse = left.CompositionWithInverse(right);
        Permutation pow2 = left.Pow(2);
        Permutation powMinus1 = right.Pow(-1);

        // Assert
        Assert.Equal([2, 0, 1], composition.OneLine());
        Assert.Equal([1, 0, 2], inverse.OneLine());
        Assert.Equal(left.Composition(right.Inverse()).OneLine(), withInverse.OneLine());
        Assert.True(pow2.IsIdentity);
        Assert.Equal(right.Inverse().OneLine(), powMinus1.OneLine());
    }

    [Fact(DisplayName = "Should return symmetry views and identity correctly")]
    public void ShouldReturnSymmetryViewsAndIdentityCorrectly()
    {
        // Arrange
        PermutationOneLineShort symmetry = new(false, 1, 0, 2);
        PermutationOneLineShort antisymmetry = new(false, true, 2, [1, 0, 2], true);

        // Act
        Permutation symFromSym = symmetry.ToSymmetry();
        Permutation symFromAntisym = antisymmetry.ToSymmetry();
        Permutation negated = symmetry.Negate();
        Permutation identity = symmetry.Identity;

        // Assert
        Assert.Same(symmetry, symFromSym);
        Assert.False(symFromAntisym.IsAntisymmetry);
        Assert.True(negated.IsAntisymmetry);
        Assert.False(negated.IsIdentity);
        Assert.True(identity.IsIdentity);
        Assert.Equal([0, 1, 2], identity.OneLine());
    }

    [Fact(DisplayName = "Should move right and preserve shifted mapping")]
    public void ShouldMoveRightAndPreserveShiftedMapping()
    {
        // Arrange
        PermutationOneLineShort permutation = new(false, 1, 0, 2);
        int[] values = [10, 11, 12, 13, 14];

        // Act
        Permutation moved = permutation.MoveRight(2);

        // Assert
        Assert.Equal(5, moved.Degree);
        Assert.Equal([0, 1, 3, 2, 4], moved.OneLine());
        Assert.Equal([10, 11, 13, 12, 14], moved.Permute(values));
    }

    [Fact(DisplayName = "Should implement equality hashcode comparison and formatting")]
    public void ShouldImplementEqualityHashCodeComparisonAndFormatting()
    {
        // Arrange
        PermutationOneLineShort first = new(false, 1, 0, 2);
        PermutationOneLineShort equal = new(false, 1, 0, 2);
        PermutationOneLineShort bigger = new(false, 2, 0, 1);
        PermutationOneLineShort antisymmetry = new(false, true, 2, [1, 0, 2], true);

        // Act + Assert
        Assert.Equal(first, equal);
        Assert.Equal(first.GetHashCode(), equal.GetHashCode());
        Assert.True(first.CompareTo(bigger) < 0);
        Assert.True(antisymmetry.CompareTo(first) < 0);
        Assert.Equal("+1, 0, 2", first.ToStringOneLine());
        Assert.Equal(first.ToStringCycles(), first.ToString());
        Assert.Equal([2], first.LengthsOfCycles);
        Assert.Equal([1, 0, 2], first.Select(value => (int)value).ToArray());
    }
}
