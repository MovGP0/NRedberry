using System.Collections.ObjectModel;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsInternalDegreeTests
{
    [Fact(DisplayName = "Should return zero for empty int array")]
    public void ShouldReturnZeroForEmptyIntArray()
    {
        int degree = GroupPermutations.InternalDegree(Array.Empty<int>());

        Assert.Equal(0, degree);
    }

    [Fact(DisplayName = "Should return max value plus one for int array")]
    public void ShouldReturnMaxValuePlusOneForIntArray()
    {
        int degree = GroupPermutations.InternalDegree([2, 0, 5, 1]);

        Assert.Equal(6, degree);
    }

    [Fact(DisplayName = "Should return zero for empty short array")]
    public void ShouldReturnZeroForEmptyShortArray()
    {
        short degree = GroupPermutations.InternalDegree(Array.Empty<short>());

        Assert.Equal((short)0, degree);
    }

    [Fact(DisplayName = "Should return max value plus one for short array")]
    public void ShouldReturnMaxValuePlusOneForShortArray()
    {
        short degree = GroupPermutations.InternalDegree([(short)3, (short)1, (short)4]);

        Assert.Equal((short)5, degree);
    }

    [Fact(DisplayName = "Should return zero for empty sbyte array")]
    public void ShouldReturnZeroForEmptySbyteArray()
    {
        sbyte degree = GroupPermutations.InternalDegree([]);

        Assert.Equal((sbyte)0, degree);
    }

    [Fact(DisplayName = "Should return max value plus one for sbyte array")]
    public void ShouldReturnMaxValuePlusOneForSbyteArray()
    {
        sbyte degree = GroupPermutations.InternalDegree([(sbyte)6, (sbyte)2, (sbyte)1]);

        Assert.Equal((sbyte)7, degree);
    }

    [Fact(DisplayName = "Should return zero for empty permutation list")]
    public void ShouldReturnZeroForEmptyPermutationList()
    {
        List<Permutation> permutations = [];

        int degree = GroupPermutations.InternalDegree(permutations);

        Assert.Equal(0, degree);
    }

    [Fact(DisplayName = "Should return max degree for permutation list")]
    public void ShouldReturnMaxDegreeForPermutationList()
    {
        List<Permutation> permutations =
        [
            GroupPermutations.CreatePermutation(1, 0),
            GroupPermutations.CreatePermutation(2, 0, 1),
            GroupPermutations.CreatePermutation(3, 1, 2, 0)
        ];

        int degree = GroupPermutations.InternalDegree(permutations);

        Assert.Equal(4, degree);
    }

    [Fact(DisplayName = "Should return max degree for read-only collection")]
    public void ShouldReturnMaxDegreeForReadOnlyCollection()
    {
        IReadOnlyCollection<Permutation> permutations = new ReadOnlyCollection<Permutation>(
        [
            GroupPermutations.CreatePermutation(1, 0),
            GroupPermutations.CreatePermutation(2, 0, 1),
            GroupPermutations.CreatePermutation(3, 0, 1, 2)
        ]);

        int degree = GroupPermutations.InternalDegree(permutations);

        Assert.Equal(4, degree);
    }
}
