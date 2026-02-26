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

        Assert.Equal([upperLatin1, upperGreek1, lowerLatin5, lowerGreek2], indices.AllIndices.ToArray());
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

        Assert.Equal(4, indices.Size(IndexType.LatinLower));
        Assert.Equal(1, indices.Size(IndexType.GreekLower));
        Assert.Equal(0, indices.Size(IndexType.Matrix4));

        Assert.Equal(upperLatin1, indices[IndexType.LatinLower, 0]);
        Assert.Equal(upperLatin2, indices[IndexType.LatinLower, 1]);
        Assert.Equal(lowerLatin1, indices[IndexType.LatinLower, 2]);
        Assert.Equal(lowerLatin5, indices[IndexType.LatinLower, 3]);
        Assert.Throws<IndexOutOfRangeException>(() => _ = indices[IndexType.LatinLower, 4]);

        Assert.Equal(lowerGreek1, indices[IndexType.GreekLower, 0]);
        Assert.Throws<IndexOutOfRangeException>(() => _ = indices[IndexType.GreekLower, 1]);
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

        Assert.Same(IndicesFactory.EmptyIndices, empty);
        Assert.IsType<SortedIndices>(subset);
        Assert.NotSame(mixed, subset);
        Assert.Equal([upperLatin1, lowerLatin2], subset.AllIndices.ToArray());

        SortedIndices onlyLatin = new([lowerLatin2, upperLatin1]);
        IndicesContract all = onlyLatin.GetOfType(IndexType.LatinLower);

        Assert.Same(onlyLatin, all);
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

        Assert.Equal(expectedUpper, inverted.UpperIndices.ToArray());
        Assert.Equal(expectedLower, inverted.LowerIndices.ToArray());
    }

    [Fact]
    public void ApplyIndexMappingShouldReturnSameInstanceForIdentity()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerLatin4 = IndicesUtils.CreateIndex(4, IndexType.LatinLower, false);
        SortedIndices indices = new([lowerLatin4, upperLatin1]);

        IndexMapper identity = new([], []);

        IndicesContract mapped = indices.ApplyIndexMapping(identity);

        Assert.Same(indices, mapped);
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

        Assert.IsType<SortedIndices>(mapped);
        Assert.NotSame(indices, mapped);
        Assert.Equal(
            [
                IndicesUtils.CreateIndex(9, IndexType.LatinLower, true),
                IndicesUtils.CreateIndex(9, IndexType.LatinLower, false),
                lowerGreek2,
            ],
            mapped.AllIndices.ToArray());
    }

    [Fact]
    public void GetDiffIdsShouldHaveLengthMatchingSize()
    {
        int upperLatin1 = IndicesUtils.CreateIndex(1, IndexType.LatinLower, true);
        int lowerGreek3 = IndicesUtils.CreateIndex(3, IndexType.GreekLower, false);

        SortedIndices indices = new([lowerGreek3, upperLatin1]);

        short[] diffIds = indices.GetDiffIds();

        Assert.Equal(indices.Size(), diffIds.Length);
    }

    [Fact]
    public void TestConsistentWithExceptionShouldThrowForDuplicateIndices()
    {
        int duplicate = IndicesUtils.CreateIndex(7, IndexType.LatinLower, false);

        Exception? exception = Record.Exception(() => _ = new SortedIndices([duplicate, duplicate]));

        Assert.NotNull(exception);
        Assert.True(
            exception is InconsistentIndicesException || exception is TypeInitializationException,
            "Unexpected exception type.");
    }
}
