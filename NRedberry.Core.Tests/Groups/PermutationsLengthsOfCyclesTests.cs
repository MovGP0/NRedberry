using Shouldly;
using GroupPermutations = NRedberry.Groups.Permutations;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsLengthsOfCyclesTests
{
    [Fact]
    public void ShouldReturnEmptyCycleLengthsForIdentityAcrossOverloads()
    {
        int[] intLengths = GroupPermutations.LengthsOfCycles([0, 1, 2, 3]);
        int[] shortLengths = GroupPermutations.LengthsOfCycles(new short[] { 0, 1, 2, 3 });
        int[] sbyteLengths = GroupPermutations.LengthsOfCycles(new sbyte[] { 0, 1, 2, 3 });

        intLengths.ShouldBeEmpty();
        shortLengths.ShouldBeEmpty();
        sbyteLengths.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldReturnOnlyNontrivialCycleLengthsAcrossOverloads()
    {
        int[] intPermutation = [1, 2, 0, 4, 3, 5, 7, 6];
        short[] shortPermutation = [1, 2, 0, 4, 3, 5, 7, 6];
        sbyte[] sbytePermutation = [1, 2, 0, 4, 3, 5, 7, 6];

        int[] intLengths = GroupPermutations.LengthsOfCycles(intPermutation);
        int[] shortLengths = GroupPermutations.LengthsOfCycles(shortPermutation);
        int[] sbyteLengths = GroupPermutations.LengthsOfCycles(sbytePermutation);

        Array.Sort(intLengths);
        Array.Sort(shortLengths);
        Array.Sort(sbyteLengths);

        intLengths.ShouldBe([2, 2, 3]);
        shortLengths.ShouldBe([2, 2, 3]);
        sbyteLengths.ShouldBe([2, 2, 3]);
    }

    [Fact]
    public void ShouldMatchCycleLengthsOfConvertOneLineToCyclesForAllOverloads()
    {
        var random = new Random(123456);
        for (int i = 0; i < 100; i++)
        {
            int degree = 1 + random.Next(25);
            int[] permutation = GroupPermutations.RandomPermutation(degree, random);

            int[][] cycles = GroupPermutations.ConvertOneLineToCycles(permutation);
            int[] expectedLengths = new int[cycles.Length];
            for (int j = 0; j < cycles.Length; j++)
            {
                expectedLengths[j] = cycles[j].Length;
            }

            Array.Sort(expectedLengths);

            short[] shortPermutation = Array.ConvertAll(permutation, static value => (short)value);
            sbyte[] sbytePermutation = Array.ConvertAll(permutation, static value => (sbyte)value);

            int[] intLengths = GroupPermutations.LengthsOfCycles(permutation);
            int[] shortLengths = GroupPermutations.LengthsOfCycles(shortPermutation);
            int[] sbyteLengths = GroupPermutations.LengthsOfCycles(sbytePermutation);

            Array.Sort(intLengths);
            Array.Sort(shortLengths);
            Array.Sort(sbyteLengths);

            intLengths.ShouldBe(expectedLengths);
            shortLengths.ShouldBe(expectedLengths);
            sbyteLengths.ShouldBe(expectedLengths);
        }
    }
}
