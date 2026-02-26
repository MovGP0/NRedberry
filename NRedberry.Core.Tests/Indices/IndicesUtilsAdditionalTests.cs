using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.Indices;
using Xunit;
using TensorIndices = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesUtilsAdditionalTests
{
    [Fact]
    public void ShouldCreateIndexAndRoundTripCoreComponents()
    {
        const IndexType type = IndexType.Matrix4;
        int index = IndicesUtils.CreateIndex(0x12345, type, true);

        Assert.True(IndicesUtils.GetState(index));
        Assert.Equal(0x2345, IndicesUtils.GetNameWithoutType(index));
        Assert.Equal(type.GetType_(), IndicesUtils.GetType(index));
        Assert.Equal(IndexTypeMethods.GetType(type.GetType_()), IndicesUtils.GetTypeEnum(index));
        Assert.Equal(
            (type.GetType_() << 24) | 0x2345,
            IndicesUtils.GetNameWithType(index));
    }

    [Fact]
    public void ShouldSetTypeAndStateWithoutChangingName()
    {
        int original = IndicesUtils.CreateIndex(0x2345, IndexType.LatinLower, true);
        int changedType = IndicesUtils.SetType(IndexType.Matrix2, original);
        int lowered = IndicesUtils.SetState(false, changedType);

        Assert.Equal(IndicesUtils.GetNameWithoutType(original), IndicesUtils.GetNameWithoutType(changedType));
        Assert.Equal(IndexType.Matrix2, IndicesUtils.GetTypeEnum(changedType));
        Assert.True(IndicesUtils.GetState(changedType));
        Assert.False(IndicesUtils.GetState(lowered));
    }

    [Fact]
    public void ShouldRaiseLowerAndInverseState()
    {
        int lower = IndicesUtils.CreateIndex(7, IndexType.GreekLower, false);
        int raised = IndicesUtils.Raise(lower);
        int loweredAgain = IndicesUtils.Lower(raised);
        int inverted = IndicesUtils.InverseIndexState(lower);

        Assert.True(IndicesUtils.GetState(raised));
        Assert.False(IndicesUtils.GetState(loweredAgain));
        Assert.Equal(raised, inverted);
        Assert.Equal(lower, IndicesUtils.InverseIndexState(inverted));
    }

    [Fact]
    public void ShouldCompareTypesNamesStatesAndContractions()
    {
        int lowerLatinA = IndicesUtils.CreateIndex(10, IndexType.LatinLower, false);
        int upperLatinA = IndicesUtils.CreateIndex(10, IndexType.LatinLower, true);
        int upperLatinB = IndicesUtils.CreateIndex(11, IndexType.LatinLower, true);
        int upperGreekA = IndicesUtils.CreateIndex(10, IndexType.GreekLower, true);

        Assert.True(IndicesUtils.HasEqualTypeAndName(lowerLatinA, upperLatinA));
        Assert.False(IndicesUtils.HasEqualTypeAndName(lowerLatinA, upperLatinB));

        Assert.True(IndicesUtils.HasEqualTypes(lowerLatinA, upperLatinA));
        Assert.False(IndicesUtils.HasEqualTypes(lowerLatinA, upperGreekA));

        Assert.False(IndicesUtils.HasEqualTypesAndStates(lowerLatinA, upperLatinA));
        Assert.True(IndicesUtils.HasEqualTypesAndStates(upperLatinA, upperLatinB));

        Assert.True(IndicesUtils.AreContracted(lowerLatinA, upperLatinA));
        Assert.False(IndicesUtils.AreContracted(lowerLatinA, upperLatinB));

        Assert.False(IndicesUtils.HaveEqualStates(lowerLatinA, upperLatinA));
        Assert.True(IndicesUtils.HaveEqualStates(upperLatinA, upperLatinB));
    }

    [Fact]
    public void ShouldValidatePermutationConsistencyWithIntArrayPermutation()
    {
        int[] indices =
        [
            IndicesUtils.CreateIndex(1, IndexType.LatinLower, false),
            IndicesUtils.CreateIndex(2, IndexType.LatinLower, true),
            IndicesUtils.CreateIndex(3, IndexType.GreekUpper, false)
        ];

        Assert.True(IndicesUtils.IsPermutationConsistentWithIndices(indices, [1, 0, 2]));
        Assert.False(IndicesUtils.IsPermutationConsistentWithIndices(indices, [2, 1, 0]));
        Assert.False(IndicesUtils.IsPermutationConsistentWithIndices(indices, [1, 0]));
    }

    [Fact]
    public void ShouldValidatePermutationConsistencyWithPermutationObject()
    {
        int[] indices =
        [
            IndicesUtils.CreateIndex(1, IndexType.Matrix1, false),
            IndicesUtils.CreateIndex(2, IndexType.Matrix1, true),
            IndicesUtils.CreateIndex(3, IndexType.GreekUpper, false)
        ];

        Permutation valid = Permutations.CreatePermutation(1, 0, 2);
        Permutation invalidByType = Permutations.CreatePermutation(2, 1, 0);
        Permutation tooLargeDegree = Permutations.CreatePermutation(1, 0, 2, 3);

        Assert.True(IndicesUtils.IsPermutationConsistentWithIndices(indices, valid));
        Assert.False(IndicesUtils.IsPermutationConsistentWithIndices(indices, invalidByType));
        Assert.False(IndicesUtils.IsPermutationConsistentWithIndices(indices, tooLargeDegree));
    }

    [Fact]
    public void ShouldCompareArraysRegardlessOrder()
    {
        int[] first =
        [
            IndicesUtils.CreateIndex(1, IndexType.LatinLower, false),
            IndicesUtils.CreateIndex(2, IndexType.LatinLower, true),
            IndicesUtils.CreateIndex(2, IndexType.GreekLower, false)
        ];
        int[] second =
        [
            first[2],
            first[0],
            first[1]
        ];
        int[] differentMultiplicity =
        [
            first[0],
            first[0],
            first[2]
        ];

        Assert.True(IndicesUtils.EqualsRegardlessOrder(first, second));
        Assert.False(IndicesUtils.EqualsRegardlessOrder(first, differentMultiplicity));
        Assert.False(IndicesUtils.EqualsRegardlessOrder(first, [first[0], first[1]]));
    }

    [Fact]
    public void ShouldCompareIndicesAndArrayRegardlessOrder()
    {
        int[] source =
        [
            IndicesUtils.CreateIndex(4, IndexType.LatinUpper, false),
            IndicesUtils.CreateIndex(5, IndexType.GreekLower, true),
            IndicesUtils.CreateIndex(6, IndexType.Matrix3, false)
        ];
        TensorIndices indices = IndicesFactory.CreateSimple(null, source);

        Assert.True(IndicesUtils.EqualsRegardlessOrder(indices, [source[2], source[0], source[1]]));
        Assert.False(IndicesUtils.EqualsRegardlessOrder(indices, [source[0], source[1]]));
        Assert.False(IndicesUtils.EqualsRegardlessOrder(indices, [source[0], source[1], source[1]]));
        Assert.True(IndicesUtils.EqualsRegardlessOrder(EmptyIndices.EmptyIndicesInstance, []));
    }

    [Fact]
    public void ShouldGetIndicesNamesFromAllOverloads()
    {
        int lower = IndicesUtils.CreateIndex(101, IndexType.LatinLower, false);
        int upper = IndicesUtils.CreateIndex(202, IndexType.GreekUpper, true);
        int[] source = [lower, upper];
        int[] expected =
        [
            IndicesUtils.GetNameWithType(lower),
            IndicesUtils.GetNameWithType(upper)
        ];

        int[] fromArray = IndicesUtils.GetIndicesNames(source);
        int[] fromImmutable = IndicesUtils.GetIndicesNames(source.ToImmutableArray());
        int[] fromIndices = IndicesUtils.GetIndicesNames(IndicesFactory.CreateSimple(null, source));

        Assert.Equal(expected, fromArray);
        Assert.Equal(expected, fromImmutable);
        Assert.Equal(expected, fromIndices);
    }

    [Fact]
    public void ShouldGetIntersectionsForContractedPairs()
    {
        int lowerLatinA = IndicesUtils.CreateIndex(1, IndexType.LatinLower, false);
        int upperLatinA = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int upperGreekB = IndicesUtils.CreateIndex(2, IndexType.GreekUpper, true);
        int lowerGreekB = IndicesUtils.CreateIndex(2, IndexType.GreekUpper, false);

        int[] intersections = IndicesUtils.GetIntersections(
            [lowerLatinA, upperGreekB],
            [upperLatinA, lowerGreekB]);

        Assert.Equal(
            [
                IndicesUtils.GetNameWithType(upperLatinA),
                IndicesUtils.GetNameWithType(lowerGreekB)
            ],
            intersections);
        Assert.Empty(IndicesUtils.GetIntersections([], [upperLatinA]));
    }
}
