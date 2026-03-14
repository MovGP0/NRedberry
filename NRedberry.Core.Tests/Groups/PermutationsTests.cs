using System.Numerics;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsTests
{
    [Theory(DisplayName = "Should compute parity for known int permutations")]
    [InlineData(new[] { 0, 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 0, 2, 3 }, 1)]
    [InlineData(new[] { 1, 0, 3, 2 }, 0)]
    [InlineData(new[] { 3, 2, 0, 1 }, 1)]
    public void ShouldComputeParityForKnownIntPermutations(int[] permutation, int expectedParity)
    {
        int actual = GroupPermutations.Parity(permutation);

        Assert.Equal(expectedParity, actual);
    }

    [Fact(DisplayName = "Should compute parity for short and sbyte overloads")]
    public void ShouldComputeParityForShortAndSbyteOverloads()
    {
        short[] shortPermutation = [1, 0, 2];
        sbyte[] sbytePermutation = [2, 0, 1];

        int shortParity = GroupPermutations.Parity(shortPermutation);
        int sbyteParity = GroupPermutations.Parity(sbytePermutation);

        Assert.Equal(1, shortParity);
        Assert.Equal(0, sbyteParity);
    }

    [Fact(DisplayName = "Should compute order as lcm of cycle lengths")]
    public void ShouldComputeOrderAsLcmOfCycleLengths()
    {
        int[] permutation = [1, 2, 0, 4, 3];

        BigInteger order = GroupPermutations.OrderOfPermutation(permutation);

        Assert.Equal(new BigInteger(6), order);
    }

    [Fact(DisplayName = "Should evaluate odd order for all overloads")]
    public void ShouldEvaluateOddOrderForAllOverloads()
    {
        int[] oddOrder = [1, 2, 0, 3];
        short[] evenOrderShort = [1, 0, 2];
        sbyte[] oddOrderSbyte = [2, 0, 1];

        Assert.True(GroupPermutations.OrderOfPermutationIsOdd(oddOrder));
        Assert.False(GroupPermutations.OrderOfPermutationIsOdd(evenOrderShort));
        Assert.True(GroupPermutations.OrderOfPermutationIsOdd(oddOrderSbyte));
    }

    [Fact(DisplayName = "Should return nontrivial cycle lengths only")]
    public void ShouldReturnNontrivialCycleLengthsOnly()
    {
        int[] permutation = [1, 2, 0, 3, 5, 4];

        int[] lengths = GroupPermutations.LengthsOfCycles(permutation);
        Array.Sort(lengths);

        Assert.Equal(new[] { 2, 3 }, lengths);
    }

    [Fact(DisplayName = "Should validate identity for all overloads")]
    public void ShouldValidateIdentityForAllOverloads()
    {
        Assert.True(GroupPermutations.IsIdentity([0, 1, 2]));
        Assert.False(GroupPermutations.IsIdentity([1, 0, 2]));
        Assert.True(GroupPermutations.IsIdentity(new short[] { 0, 1, 2 }));
        Assert.False(GroupPermutations.IsIdentity(new short[] { 0, 2, 1 }));
        Assert.True(GroupPermutations.IsIdentity(new sbyte[] { 0, 1, 2 }));
        Assert.False(GroupPermutations.IsIdentity(new sbyte[] { 2, 1, 0 }));
    }

    [Fact(DisplayName = "Should build inverse permutation")]
    public void ShouldBuildInversePermutation()
    {
        int[] permutation = [2, 0, 1];

        int[] inverse = GroupPermutations.Inverse(permutation);

        Assert.Equal(new[] { 1, 2, 0 }, inverse);
    }

    [Fact(DisplayName = "Should compute internal degree for array overloads")]
    public void ShouldComputeInternalDegreeForArrayOverloads()
    {
        int intDegree = GroupPermutations.InternalDegree([0, 4, 1]);
        short shortDegree = GroupPermutations.InternalDegree(new short[] { 0, 2, 1 });
        sbyte sbyteDegree = GroupPermutations.InternalDegree(new sbyte[] { 1, 0 });

        Assert.Equal(5, intDegree);
        Assert.Equal((short)3, shortDegree);
        Assert.Equal((sbyte)2, sbyteDegree);
    }

    [Fact(DisplayName = "Should compute internal degree for permutation collections")]
    public void ShouldComputeInternalDegreeForPermutationCollections()
    {
        List<Permutation> list =
        [
            GroupPermutations.CreatePermutation(1, 0),
            GroupPermutations.CreatePermutation(2, 0, 1)
        ];

        int listDegree = GroupPermutations.InternalDegree(list);
        int readOnlyDegree = GroupPermutations.InternalDegree((IReadOnlyCollection<Permutation>)list);

        Assert.Equal(3, listDegree);
        Assert.Equal(3, readOnlyDegree);
    }

    [Fact(DisplayName = "Should validate permutation correctness and sign constraints")]
    public void ShouldValidatePermutationCorrectnessAndSignConstraints()
    {
        Assert.True(GroupPermutations.TestPermutationCorrectness([2, 0, 1]));
        Assert.False(GroupPermutations.TestPermutationCorrectness([1, 1, 0]));
        Assert.False(GroupPermutations.TestPermutationCorrectness([0, 3, 1]));

        Assert.False(GroupPermutations.TestPermutationCorrectness([2, 0, 1], true));
        Assert.True(GroupPermutations.TestPermutationCorrectness([1, 0, 2], true));
    }

    [Fact(DisplayName = "Should generate deterministic random permutation with seeded random")]
    public void ShouldGenerateDeterministicRandomPermutationWithSeededRandom()
    {
        var firstRandom = new Random(12345);
        var secondRandom = new Random(12345);

        int[] first = GroupPermutations.RandomPermutation(10, firstRandom);
        int[] second = GroupPermutations.RandomPermutation(10, secondRandom);

        Assert.Equal(first, second);
        Assert.True(GroupPermutations.TestPermutationCorrectness(first));
    }

    [Fact(DisplayName = "Should throw for negative random permutation dimension")]
    public void ShouldThrowForNegativeRandomPermutationDimension()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupPermutations.RandomPermutation(-1, new Random(1)));
    }

    [Fact(DisplayName = "Should shuffle arrays deterministically with seeded random")]
    public void ShouldShuffleArraysDeterministicallyWithSeededRandom()
    {
        int[] leftInt = [0, 1, 2, 3, 4, 5];
        int[] rightInt = [0, 1, 2, 3, 4, 5];
        object[] leftObject = ["a", "b", "c", "d"];
        object[] rightObject = ["a", "b", "c", "d"];

        GroupPermutations.Shuffle(leftInt, new Random(7));
        GroupPermutations.Shuffle(rightInt, new Random(7));
        GroupPermutations.Shuffle(leftObject, new Random(11));
        GroupPermutations.Shuffle(rightObject, new Random(11));

        Assert.Equal(rightInt, leftInt);
        Assert.Equal(rightObject, leftObject);
        Assert.True(GroupPermutations.TestPermutationCorrectness(leftInt));
    }

    [Fact(DisplayName = "Should permute int and generic arrays")]
    public void ShouldPermuteIntAndGenericArrays()
    {
        int[] intArray = [10, 20, 30];
        string[] stringArray = ["a", "b", "c"];
        int[] permutation = [2, 0, 1];

        int[] permutedInts = GroupPermutations.Permute(intArray, permutation);
        string[] permutedStrings = GroupPermutations.Permute(stringArray, permutation);

        Assert.Equal(new[] { 30, 10, 20 }, permutedInts);
        Assert.Equal(new[] { "c", "a", "b" }, permutedStrings);
    }

    [Fact(DisplayName = "Should permute list and guard invalid arguments")]
    public void ShouldPermuteListAndGuardInvalidArguments()
    {
        List<int> list = [10, 20, 30];

        List<int> permuted = GroupPermutations.Permute(list, [1, 2, 0]);

        Assert.Equal(new[] { 20, 30, 10 }, permuted);
        Assert.Throws<ArgumentException>(() => GroupPermutations.Permute(new[] { 1, 2 }, [0]));
        Assert.Throws<ArgumentException>(() => GroupPermutations.Permute(new[] { 1, 2, 3 }, [0, 1, 1]));
    }
}
