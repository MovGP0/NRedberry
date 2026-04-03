using System.Collections.ObjectModel;
using NRedberry.Core.Combinatorics;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsInternalDegreeTests
{
    [Fact(DisplayName = "Should return zero for empty int array")]
    public void ShouldReturnZeroForEmptyIntArray()
    {
        int degree = GroupPermutations.InternalDegree(Array.Empty<int>());

        degree.ShouldBe(0);
    }

    [Fact(DisplayName = "Should return max value plus one for int array")]
    public void ShouldReturnMaxValuePlusOneForIntArray()
    {
        int degree = GroupPermutations.InternalDegree([2, 0, 5, 1]);

        degree.ShouldBe(6);
    }

    [Fact(DisplayName = "Should return zero for empty short array")]
    public void ShouldReturnZeroForEmptyShortArray()
    {
        short degree = GroupPermutations.InternalDegree(Array.Empty<short>());

        degree.ShouldBe((short)0);
    }

    [Fact(DisplayName = "Should return max value plus one for short array")]
    public void ShouldReturnMaxValuePlusOneForShortArray()
    {
        short degree = GroupPermutations.InternalDegree([(short)3, (short)1, (short)4]);

        degree.ShouldBe((short)5);
    }

    [Fact(DisplayName = "Should return zero for empty sbyte array")]
    public void ShouldReturnZeroForEmptySbyteArray()
    {
        sbyte degree = GroupPermutations.InternalDegree(Array.Empty<sbyte>());

        degree.ShouldBe((sbyte)0);
    }

    [Fact(DisplayName = "Should return max value plus one for sbyte array")]
    public void ShouldReturnMaxValuePlusOneForSbyteArray()
    {
        sbyte degree = GroupPermutations.InternalDegree([(sbyte)6, (sbyte)2, (sbyte)1]);

        degree.ShouldBe((sbyte)7);
    }

    [Fact(DisplayName = "Should return zero for empty permutation list")]
    public void ShouldReturnZeroForEmptyPermutationList()
    {
        List<Permutation> permutations = [];

        int degree = GroupPermutations.InternalDegree(permutations);

        degree.ShouldBe(0);
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

        degree.ShouldBe(4);
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

        degree.ShouldBe(4);
    }
}
