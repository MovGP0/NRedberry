using System;
using System.Linq;
using NRedberry;
using NRedberry.Indices;
using NRedberry.Tensors;
using Xunit;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class SortedIndicesTests
{
    [Fact]
    public void ConstructorShouldSortData()
    {
        int lowerLatin5 = IndicesUtils.CreateIndex(5, IndexType.LatinLower, false);
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerGreek2 = IndicesUtils.CreateIndex(2, IndexType.GreekLower, false);
        int upperGreek1 = IndicesUtils.CreateIndex(1, IndexType.GreekLower, true);

        SortedIndices indices = new([lowerLatin5, upperGreek1, lowerGreek2, upperLatin1]);

        indices.AllIndices.ToArray().ShouldBe([upperLatin1, upperGreek1, lowerLatin5, lowerGreek2]);
    }

    [Fact]
    public void SizeAndTypeIndexerShouldReturnIndicesByTypeWithUpperFirstThenLower()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int upperLatin2 = IndicesUtils.CreateIndex(2, IndexType.LatinLower, true);
        int lowerLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, false);
        int lowerLatin5 = IndicesUtils.CreateIndex(5, IndexType.LatinLower, false);
        int lowerGreek1 = IndicesUtils.CreateIndex(1, IndexType.GreekLower, false);

        SortedIndices indices = new([lowerLatin5, upperLatin2, lowerGreek1, lowerLatin1, upperLatin1]);

        indices.Size(IndexType.LatinLower).ShouldBe(4);
        indices.Size(IndexType.GreekLower).ShouldBe(1);
        indices.Size(IndexType.Matrix4).ShouldBe(0);

        indices[IndexType.LatinLower, 0].ShouldBe(upperLatin1);
        indices[IndexType.LatinLower, 1].ShouldBe(upperLatin2);
        indices[IndexType.LatinLower, 2].ShouldBe(lowerLatin1);
        indices[IndexType.LatinLower, 3].ShouldBe(lowerLatin5);
        Should.Throw<IndexOutOfRangeException>(() => _ = indices[IndexType.LatinLower, 4]);

        indices[IndexType.GreekLower, 0].ShouldBe(lowerGreek1);
        Should.Throw<IndexOutOfRangeException>(() => _ = indices[IndexType.GreekLower, 1]);
    }

    [Fact]
    public void GetOfTypeShouldReturnEmptyThisOrNew()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerLatin2 = IndicesUtils.CreateIndex(2, IndexType.LatinLower, false);
        int lowerGreek1 = IndicesUtils.CreateIndex(1, IndexType.GreekLower, false);

        SortedIndices mixed = new([lowerGreek1, lowerLatin2, upperLatin1]);
        IndicesContract empty = mixed.GetOfType(IndexType.Matrix4);
        IndicesContract subset = mixed.GetOfType(IndexType.LatinLower);

        empty.ShouldBeSameAs(IndicesFactory.EmptyIndices);
        subset.ShouldBeOfType<SortedIndices>();
        subset.ShouldNotBeSameAs(mixed);
        subset.AllIndices.ToArray().ShouldBe([upperLatin1, lowerLatin2]);

        SortedIndices onlyLatin = new([lowerLatin2, upperLatin1]);
        IndicesContract all = onlyLatin.GetOfType(IndexType.LatinLower);

        all.ShouldBeSameAs(onlyLatin);
    }

    [Fact]
    public void GetInvertedShouldSwapUpperAndLowerPartitions()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int upperGreek2 = IndicesUtils.CreateIndex(2, IndexType.GreekLower, true);
        int lowerLatin3 = IndicesUtils.CreateIndex(3, IndexType.LatinLower, false);

        SortedIndices original = new([upperGreek2, lowerLatin3, upperLatin1]);
        IndicesContract inverted = original.GetInverted();

        int[] expectedUpper = original.LowerIndices.Select(i => i ^ IndicesUtils.UpperRawStateInt).ToArray();
        int[] expectedLower = original.UpperIndices.Select(i => i ^ IndicesUtils.UpperRawStateInt).ToArray();

        inverted.UpperIndices.ToArray().ShouldBe(expectedUpper);
        inverted.LowerIndices.ToArray().ShouldBe(expectedLower);
    }

    [Fact]
    public void ApplyIndexMappingShouldReturnSameInstanceForIdentity()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerLatin4 = IndicesUtils.CreateIndex(4, IndexType.LatinLower, false);
        SortedIndices indices = new([lowerLatin4, upperLatin1]);

        IndexMapper identity = new([], []);

        IndicesContract mapped = indices.ApplyIndexMapping(identity);

        mapped.ShouldBeSameAs(indices);
    }

    [Fact]
    public void ApplyIndexMappingShouldReturnNewSortedIndicesWhenChanged()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, false);
        int lowerGreek2 = IndicesUtils.CreateIndex(2, IndexType.GreekLower, false);

        SortedIndices indices = new([lowerGreek2, lowerLatin1, upperLatin1]);

        int fromNameWithType = IndicesUtils.GetNameWithType(upperLatin1);
        int toNameWithType = IndicesUtils.GetNameWithType(IndicesUtils.CreateIndex(9, IndexType.LatinLower, false));
        IndexMapper mapper = new([fromNameWithType], [toNameWithType]);

        IndicesContract mapped = indices.ApplyIndexMapping(mapper);

        mapped.ShouldBeOfType<SortedIndices>();
        mapped.ShouldNotBeSameAs(indices);
        mapped.AllIndices.ToArray().ShouldBe([
                IndicesUtils.CreateIndex(9, IndexType.LatinLower, true),
                IndicesUtils.CreateIndex(9, IndexType.LatinLower, false),
                lowerGreek2,
            ]);
    }

    [Fact]
    public void GetDiffIdsShouldHaveLengthMatchingSize()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerGreek3 = IndicesUtils.CreateIndex(3, IndexType.GreekLower, false);

        SortedIndices indices = new([lowerGreek3, upperLatin1]);

        short[] diffIds = indices.GetDiffIds();

        diffIds.Length.ShouldBe(indices.Size());
    }

    [Fact]
    public void TestConsistentWithExceptionShouldThrowForDuplicateIndices()
    {
        int duplicate = IndicesUtils.CreateIndex(7, IndexType.LatinLower, false);

        Exception? exception = Record.Exception(() => _ = new SortedIndices([duplicate, duplicate]));

        exception.ShouldNotBeNull();
        exception is InconsistentIndicesException || exception is TypeInitializationException.ShouldBeTrue("Unexpected exception type.");
    }
}
