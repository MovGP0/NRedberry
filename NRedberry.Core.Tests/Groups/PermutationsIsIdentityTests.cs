using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsIsIdentityTests
{
    [Fact(DisplayName = "IsIdentity int should return true for empty permutation")]
    public void IsIdentityIntShouldReturnTrueForEmptyPermutation()
    {
        int[] permutation = [];

        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeTrue();
    }

    [Fact(DisplayName = "IsIdentity int should return true for identity permutation")]
    public void IsIdentityIntShouldReturnTrueForIdentityPermutation()
    {
        int[] permutation = [0, 1, 2, 3, 4];

        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeTrue();
    }

    [Theory(DisplayName = "IsIdentity int should return false for non identity permutation")]
    [InlineData(new[] { 1, 0, 2, 3 })]
    [InlineData(new[] { 0, 2, 1, 3 })]
    [InlineData(new[] { 0, 1, 2, 4, 3 })]
    public void IsIdentityIntShouldReturnFalseForNonIdentityPermutation(int[] permutation)
    {
        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeFalse();
    }

    [Fact(DisplayName = "IsIdentity short should return true for empty permutation")]
    public void IsIdentityShortShouldReturnTrueForEmptyPermutation()
    {
        short[] permutation = [];

        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeTrue();
    }

    [Fact(DisplayName = "IsIdentity short should return true for identity permutation")]
    public void IsIdentityShortShouldReturnTrueForIdentityPermutation()
    {
        short[] permutation = [0, 1, 2, 3];

        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeTrue();
    }

    [Theory(DisplayName = "IsIdentity short should return false for non identity permutation")]
    [InlineData(new short[] { 1, 0, 2 })]
    [InlineData(new short[] { 0, 2, 1 })]
    [InlineData(new short[] { 0, 1, 3, 2 })]
    public void IsIdentityShortShouldReturnFalseForNonIdentityPermutation(short[] permutation)
    {
        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeFalse();
    }

    [Fact(DisplayName = "IsIdentity sbyte should return true for empty permutation")]
    public void IsIdentitySbyteShouldReturnTrueForEmptyPermutation()
    {
        sbyte[] permutation = [];

        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeTrue();
    }

    [Fact(DisplayName = "IsIdentity sbyte should return true for identity permutation")]
    public void IsIdentitySbyteShouldReturnTrueForIdentityPermutation()
    {
        sbyte[] permutation = [0, 1, 2, 3];

        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeTrue();
    }

    [Theory(DisplayName = "IsIdentity sbyte should return false for non identity permutation")]
    [InlineData(new sbyte[] { 1, 0, 2 })]
    [InlineData(new sbyte[] { 0, 2, 1 })]
    [InlineData(new sbyte[] { 0, 1, 3, 2 })]
    public void IsIdentitySbyteShouldReturnFalseForNonIdentityPermutation(sbyte[] permutation)
    {
        bool isIdentity = GroupPermutations.IsIdentity(permutation);

        isIdentity.ShouldBeFalse();
    }
}
