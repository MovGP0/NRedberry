using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.Indices;
using TensorIndices = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesUtilsAdditionalTests
{
    [Fact]
    public void ShouldCreateIndexAndRoundTripCoreComponents()
    {
        const IndexType type = IndexType.Matrix4;
        int index = IndicesUtils.CreateIndex(0x12345, type, true);

        IndicesUtils.GetState(index).ShouldBeTrue();
        IndicesUtils.GetNameWithoutType(index).ShouldBe(0x2345);
        IndicesUtils.GetType(index).ShouldBe(type.GetType_());
        IndicesUtils.GetTypeEnum(index).ShouldBe(IndexTypeMethods.GetType(type.GetType_()));
        IndicesUtils.GetNameWithType(index).ShouldBe((type.GetType_() << 24) | 0x2345);
    }

    [Fact]
    public void ShouldSetTypeAndStateWithoutChangingName()
    {
        int original = IndicesUtils.CreateIndex(0x2345, IndexType.LatinLower, true);
        int changedType = IndicesUtils.SetType(IndexType.Matrix2, original);
        int lowered = IndicesUtils.SetState(false, changedType);

        IndicesUtils.GetNameWithoutType(changedType).ShouldBe(IndicesUtils.GetNameWithoutType(original));
        IndicesUtils.GetTypeEnum(changedType).ShouldBe(IndexType.Matrix2);
        IndicesUtils.GetState(changedType).ShouldBeTrue();
        IndicesUtils.GetState(lowered).ShouldBeFalse();
    }

    [Fact]
    public void ShouldRaiseLowerAndInverseState()
    {
        int lower = IndicesUtils.CreateIndex(7, IndexType.GreekLower, false);
        int raised = IndicesUtils.Raise(lower);
        int loweredAgain = IndicesUtils.Lower(raised);
        int inverted = IndicesUtils.InverseIndexState(lower);

        IndicesUtils.GetState(raised).ShouldBeTrue();
        IndicesUtils.GetState(loweredAgain).ShouldBeFalse();
        inverted.ShouldBe(raised);
        IndicesUtils.InverseIndexState(inverted).ShouldBe(lower);
    }

    [Fact]
    public void ShouldCompareTypesNamesStatesAndContractions()
    {
        int lowerLatinA = IndicesUtils.CreateIndex(10, IndexType.LatinLower, false);
        int upperLatinA = IndicesUtils.CreateIndex(10, IndexType.LatinLower, true);
        int upperLatinB = IndicesUtils.CreateIndex(11, IndexType.LatinLower, true);
        int upperGreekA = IndicesUtils.CreateIndex(10, IndexType.GreekLower, true);

        IndicesUtils.HasEqualTypeAndName(lowerLatinA, upperLatinA).ShouldBeTrue();
        IndicesUtils.HasEqualTypeAndName(lowerLatinA, upperLatinB).ShouldBeFalse();

        IndicesUtils.HasEqualTypes(lowerLatinA, upperLatinA).ShouldBeTrue();
        IndicesUtils.HasEqualTypes(lowerLatinA, upperGreekA).ShouldBeFalse();

        IndicesUtils.HasEqualTypesAndStates(lowerLatinA, upperLatinA).ShouldBeFalse();
        IndicesUtils.HasEqualTypesAndStates(upperLatinA, upperLatinB).ShouldBeTrue();

        IndicesUtils.AreContracted(lowerLatinA, upperLatinA).ShouldBeTrue();
        IndicesUtils.AreContracted(lowerLatinA, upperLatinB).ShouldBeFalse();

        IndicesUtils.HaveEqualStates(lowerLatinA, upperLatinA).ShouldBeFalse();
        IndicesUtils.HaveEqualStates(upperLatinA, upperLatinB).ShouldBeTrue();
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

        IndicesUtils.IsPermutationConsistentWithIndices(indices, [1, 0, 2]).ShouldBeTrue();
        IndicesUtils.IsPermutationConsistentWithIndices(indices, [2, 1, 0]).ShouldBeFalse();
        IndicesUtils.IsPermutationConsistentWithIndices(indices, [1, 0]).ShouldBeFalse();
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

        IndicesUtils.IsPermutationConsistentWithIndices(indices, valid).ShouldBeTrue();
        IndicesUtils.IsPermutationConsistentWithIndices(indices, invalidByType).ShouldBeFalse();
        IndicesUtils.IsPermutationConsistentWithIndices(indices, tooLargeDegree).ShouldBeFalse();
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

        IndicesUtils.EqualsRegardlessOrder(first, second).ShouldBeTrue();
        IndicesUtils.EqualsRegardlessOrder(first, differentMultiplicity).ShouldBeFalse();
        IndicesUtils.EqualsRegardlessOrder(first, [first[0], first[1]]).ShouldBeFalse();
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

        IndicesUtils.EqualsRegardlessOrder(indices, [source[2], source[0], source[1]]).ShouldBeTrue();
        IndicesUtils.EqualsRegardlessOrder(indices, [source[0], source[1]]).ShouldBeFalse();
        IndicesUtils.EqualsRegardlessOrder(indices, [source[0], source[1], source[1]]).ShouldBeFalse();
        IndicesUtils.EqualsRegardlessOrder(EmptyIndices.EmptyIndicesInstance, []).ShouldBeTrue();
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

        fromArray.ShouldBe(expected);
        fromImmutable.ShouldBe(expected);
        fromIndices.ShouldBe(expected);
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

        intersections.ShouldBe([
            IndicesUtils.GetNameWithType(upperLatinA),
            IndicesUtils.GetNameWithType(lowerGreekB)
        ]);
        IndicesUtils.GetIntersections([], [upperLatinA]).ShouldBeEmpty();
    }
}
