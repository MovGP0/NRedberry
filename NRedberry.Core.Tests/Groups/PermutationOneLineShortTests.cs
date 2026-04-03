using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

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
        permutation.OneLine().ShouldBe([1, 0, 2]);
        oneLineImmutable.ToArray().ShouldBe([1, 0, 2]);
        permutation.IsIdentity.ShouldBeFalse();
        permutation.IsAntisymmetry.ShouldBeFalse();
        permutation.Degree.ShouldBe(3);
        permutation.Length.ShouldBe(3);
    }

    [Fact(DisplayName = "Should reject inconsistent antisymmetric permutation")]
    public void ShouldRejectInconsistentAntisymmetricPermutation()
    {
        // Act + Assert
        Should.Throw<ArgumentException>(() => new PermutationOneLineShort(true, 0, 1, 2));
        Should.Throw<InconsistentGeneratorsException>(() => new PermutationOneLineShort(false, true, 3, [0, 1, 2]));
    }

    [Fact(DisplayName = "Should convert to int representation preserving action")]
    public void ShouldConvertToIntRepresentationPreservingAction()
    {
        // Arrange
        PermutationOneLineShort shortPermutation = new(false, 2, 0, 1);

        // Act
        PermutationOneLineInt intPermutation = shortPermutation.ToIntRepresentation();

        // Assert
        intPermutation.OneLine().ShouldBe(shortPermutation.OneLine());
        intPermutation.Degree.ShouldBe(shortPermutation.Degree);
        intPermutation.IsAntisymmetry.ShouldBe(shortPermutation.IsAntisymmetry);
        intPermutation.NewIndexOf(0).ShouldBe(shortPermutation.NewIndexOf(0));
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
        permutation.NewIndexOf(0).ShouldBe(1);
        permutation.ImageOf(1).ShouldBe(0);
        permutation.NewIndexOf(4).ShouldBe(4);
        permutation.NewIndexOfUnderInverse(4).ShouldBe(4);
        permutation.ImageOf(set).ShouldBe([1, 0, 3]);
        permutation.Permute(values).ShouldBe([20, 10, 30]);
        permutation.Permute(chars).ShouldBe(['b', 'a', 'c']);
        permutation.Permute(strings).ShouldBe(["B", "A", "C"]);
        permutation.Permute(list).ShouldBe(["B", "A", "C"]);
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
        composition.OneLine().ShouldBe([2, 0, 1]);
        inverse.OneLine().ShouldBe([1, 0, 2]);
        withInverse.OneLine().ShouldBe(left.Composition(right.Inverse()).OneLine());
        pow2.IsIdentity.ShouldBeTrue();
        powMinus1.OneLine().ShouldBe(right.Inverse().OneLine());
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
        symFromSym.ShouldBeSameAs(symmetry);
        symFromAntisym.IsAntisymmetry.ShouldBeFalse();
        negated.IsAntisymmetry.ShouldBeTrue();
        negated.IsIdentity.ShouldBeFalse();
        identity.IsIdentity.ShouldBeTrue();
        identity.OneLine().ShouldBe([0, 1, 2]);
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
        moved.Degree.ShouldBe(5);
        moved.OneLine().ShouldBe([0, 1, 3, 2, 4]);
        moved.Permute(values).ShouldBe([10, 11, 13, 12, 14]);
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
        first.ShouldBe(equal);
        first.GetHashCode().ShouldBe(equal.GetHashCode());
        first.CompareTo(bigger).ShouldBeLessThan(0);
        antisymmetry.CompareTo(first).ShouldBeLessThan(0);
        first.ToStringOneLine().ShouldBe("+1, 0, 2");
        first.ToString().ShouldBe(first.ToStringCycles());
        first.LengthsOfCycles.ShouldBe([2]);
        first.Select(value => (int)value).ToArray().ShouldBe([1, 0, 2]);
    }
}
