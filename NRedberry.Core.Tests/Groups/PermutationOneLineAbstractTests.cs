using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationOneLineAbstractTests
{
    [Fact]
    public void ShouldReturnSelfAsIdentityWhenAlreadyIdentity()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreateIdentityPermutation(3));

        Permutation identity = permutation.Identity;

        Assert.Same(permutation, identity);
    }

    [Fact]
    public void ShouldCreateIdentityWithSameLengthForNonIdentity()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(2, 0, 1));

        Permutation identity = permutation.Identity;

        Assert.NotSame(permutation, identity);
        Assert.True(identity.IsIdentity);
        Assert.Equal(permutation.Length, identity.Length);
        Assert.Equal(new[] { 0, 1, 2 }, identity.OneLine());
    }

    [Fact]
    public void ShouldUseInverseForNegativePower()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(1, 2, 0));

        Permutation actual = permutation.Pow(-1);
        Permutation expected = permutation.Inverse();

        Assert.Equal(expected, actual);
        Assert.Equal(expected.OneLine(), actual.OneLine());
    }

    [Fact]
    public void ShouldReturnSelfOnPowerForIdentityPermutation()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreateIdentityPermutation(4));

        Permutation power = permutation.Pow(-7);

        Assert.Same(permutation, power);
    }

    [Fact]
    public void ShouldComposeWithInverseUsingIdentityShortcuts()
    {
        PermutationOneLineAbstract identity = CreateTestDouble(GroupPermutations.CreateIdentityPermutation(3));
        Permutation other = GroupPermutations.CreatePermutation(2, 0, 1);
        PermutationOneLineAbstract nonIdentity = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2));

        Permutation firstActual = identity.CompositionWithInverse(other);
        Permutation firstExpected = other.Inverse();
        Permutation secondActual = nonIdentity.CompositionWithInverse(GroupPermutations.CreateIdentityPermutation(3));

        Assert.Equal(firstExpected, firstActual);
        Assert.Same(nonIdentity, secondActual);
    }

    [Fact]
    public void ShouldCalculateConjugateByDefinition()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(1, 2, 0));
        Permutation element = GroupPermutations.CreatePermutation(0, 2, 1);

        Permutation actual = permutation.Conjugate(element);
        Permutation expected = permutation.Inverse().Composition(element, permutation);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldCalculateCommutatorByDefinition()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(1, 2, 0));
        Permutation element = GroupPermutations.CreatePermutation(0, 2, 1);

        Permutation actual = permutation.Commutator(element);
        Permutation expected = permutation.Inverse().Composition(element.Inverse(), permutation, element);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldPermuteArraysAndList()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(2, 0, 1));
        int[] intValues = new[] { 10, 20, 30 };
        char[] charValues = new[] { 'a', 'b', 'c' };
        string[] genericValues = new[] { "alpha", "beta", "gamma" };
        List<string> listValues = new List<string> { "alpha", "beta", "gamma" };
        int[] imageInput = new[] { 0, 2, 1 };

        int[] intResult = permutation.Permute(intValues);
        char[] charResult = permutation.Permute(charValues);
        string[] genericResult = permutation.Permute(genericValues);
        List<string> listResult = permutation.Permute(listValues);
        int[] image = permutation.ImageOf(imageInput);

        Assert.Equal(new[] { 30, 10, 20 }, intResult);
        Assert.Equal(new[] { 'c', 'a', 'b' }, charResult);
        Assert.Equal(new[] { "gamma", "alpha", "beta" }, genericResult);
        Assert.Equal(new[] { "gamma", "alpha", "beta" }, listResult);
        Assert.Equal(new[] { 2, 1, 0 }, image);
    }

    [Fact]
    public void ShouldCloneInputsForIdentityPermutation()
    {
        PermutationOneLineAbstract identity = CreateTestDouble(GroupPermutations.CreateIdentityPermutation(3));
        int[] source = [1, 2, 3];
        List<string> listSource = ["x", "y", "z"];

        int[] clonedArray = identity.Permute(source);
        List<string> clonedList = identity.Permute(listSource);

        Assert.Equal(source, clonedArray);
        Assert.NotSame(source, clonedArray);
        Assert.Equal(listSource, clonedList);
        Assert.NotSame(listSource, clonedList);
    }

    [Fact]
    public void ShouldReturnDisjointCyclesWithoutFixedPoints()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2, 4, 3));

        int[][] cycles = permutation.Cycles();

        Assert.Equal(2, cycles.Length);
        Assert.Equal(new[] { 0, 1 }, cycles[0]);
        Assert.Equal(new[] { 3, 4 }, cycles[1]);
    }

    [Fact]
    public void ShouldCompareToByAntisymmetryAndImages()
    {
        PermutationOneLineAbstract antisymmetry = CreateTestDouble(GroupPermutations.CreatePermutation(true, 1, 0, 2));
        PermutationOneLineAbstract symmetry = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2));
        PermutationOneLineAbstract left = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2));
        PermutationOneLineAbstract right = CreateTestDouble(GroupPermutations.CreatePermutation(2, 0, 1));

        Assert.Equal(1, left.CompareTo(null));
        Assert.True(antisymmetry.CompareTo(symmetry) < 0);
        Assert.True(left.CompareTo(right) < 0);
        Assert.True(right.CompareTo(left) > 0);
    }

    [Fact]
    public void ShouldSupportEqualityHashCodeAndOperators()
    {
        PermutationOneLineAbstract left = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2));
        PermutationOneLineAbstract equal = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2));
        PermutationOneLineAbstract differentSign = CreateTestDouble(GroupPermutations.CreatePermutation(true, 1, 0, 2));

        Assert.True(left.Equals(equal));
        Assert.True(left == equal);
        Assert.False(left != equal);
        Assert.Equal(left.GetHashCode(), equal.GetHashCode());
        Assert.False(left.Equals(differentSign));
    }

    [Fact]
    public void ShouldFormatAndEnumerateUsingOneLineData()
    {
        PermutationOneLineAbstract permutation = CreateTestDouble(GroupPermutations.CreatePermutation(1, 0, 2, 4, 3));

        string oneLine = permutation.ToStringOneLine();
        string cycles = permutation.ToStringCycles();
        string text = permutation.ToString();
        List<int> values = [..permutation];

        Assert.Equal("+1, 0, 2, 4, 3", oneLine);
        Assert.Equal("+{0, 1}, {3, 4}", cycles);
        Assert.Equal(cycles, text);
        Assert.Equal(new[] { 1, 0, 2, 4, 3 }, values);
    }

    private static PermutationOneLineAbstract CreateTestDouble(Permutation permutation)
    {
        return new PermutationOneLineAbstractTestDouble(permutation);
    }
}

internal sealed class PermutationOneLineAbstractTestDouble : PermutationOneLineAbstract
{
    private readonly Permutation _permutation;

    internal PermutationOneLineAbstractTestDouble(Permutation permutation)
        : base(permutation.IsIdentity, permutation.IsAntisymmetry)
    {
        _permutation = permutation;
    }

    public override int[] OneLine()
    {
        return _permutation.OneLine();
    }

    public override ImmutableArray<int> OneLineImmutable()
    {
        return _permutation.OneLineImmutable();
    }

    public override int NewIndexOf(int i)
    {
        return _permutation.NewIndexOf(i);
    }

    public override int ImageOf(int i)
    {
        return _permutation.ImageOf(i);
    }

    public override Permutation ToSymmetry()
    {
        return _permutation.ToSymmetry();
    }

    public override Permutation Negate()
    {
        return _permutation.Negate();
    }

    public override Permutation Composition(Permutation other)
    {
        return _permutation.Composition(other);
    }

    public override Permutation Composition(Permutation a, Permutation b)
    {
        return _permutation.Composition(a, b);
    }

    public override Permutation Composition(Permutation a, Permutation b, Permutation c)
    {
        return _permutation.Composition(a, b, c);
    }

    public override Permutation Inverse()
    {
        return _permutation.Inverse();
    }

    public override BigInteger Order
    {
        get
        {
            return _permutation.Order;
        }
    }

    public override bool OrderIsOdd
    {
        get
        {
            return _permutation.OrderIsOdd;
        }
    }

    public override int Degree
    {
        get
        {
            return _permutation.Degree;
        }
    }

    public override int Length
    {
        get
        {
            return _permutation.Length;
        }
    }

    public override int Parity
    {
        get
        {
            return _permutation.Parity;
        }
    }

    public override Permutation MoveRight(int size)
    {
        return _permutation.MoveRight(size);
    }

    public override int[] LengthsOfCycles
    {
        get
        {
            return _permutation.LengthsOfCycles;
        }
    }

    public override int NewIndexOfUnderInverse(int i)
    {
        return _permutation.NewIndexOfUnderInverse(i);
    }
}
