using NRedberry.IndexGeneration;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.IndexGeneration;

public sealed class IndexGeneratorTests
{
    [Fact]
    public void EmptyGeneratorGenerateShouldReturnRequestedTypeBits()
    {
        var generator = new IndexGenerator();

        int generated = generator.Generate(IndexType.GreekUpper);

        Assert.Equal(IndexType.GreekUpper.GetType_(), IndicesUtils.GetType(generated));
        Assert.Equal(0, IndicesUtils.GetNameWithoutType(generated));
    }

    [Fact]
    public void SeededConstructorGenerateShouldSkipExistingNamesForType()
    {
        int seeded0 = IndicesUtils.CreateIndex(0, IndexType.LatinLower, false);
        int seeded2 = IndicesUtils.CreateIndex(2, IndexType.LatinLower, true);
        var generator = new IndexGenerator(seeded0, seeded2);

        int firstGenerated = generator.Generate(IndexType.LatinLower);
        int secondGenerated = generator.Generate(IndexType.LatinLower);

        Assert.Equal(1, IndicesUtils.GetNameWithoutType(firstGenerated));
        Assert.Equal(3, IndicesUtils.GetNameWithoutType(secondGenerated));
        Assert.Equal(IndexType.LatinLower.GetType_(), IndicesUtils.GetType(firstGenerated));
        Assert.Equal(IndexType.LatinLower.GetType_(), IndicesUtils.GetType(secondGenerated));
    }

    [Fact]
    public void ContainsShouldRecognizeSeededIndicesAndRejectAbsent()
    {
        int seededLatin = IndicesUtils.CreateIndex(4, IndexType.LatinUpper, false);
        int seededGreek = IndicesUtils.CreateIndex(7, IndexType.GreekLower, true);
        var generator = new IndexGenerator(seededLatin, seededGreek);

        Assert.True(generator.Contains(seededLatin));
        Assert.True(generator.Contains(IndicesUtils.Raise(seededLatin)));
        Assert.True(generator.Contains(seededGreek));
        Assert.False(generator.Contains(IndicesUtils.CreateIndex(9, IndexType.LatinUpper, false)));
    }

    [Fact]
    public void MergeFromShouldCopyMissingTypesAndAdvanceExistingTypeSequence()
    {
        int seeded0 = IndicesUtils.CreateIndex(0, IndexType.LatinLower, false);
        int seeded2 = IndicesUtils.CreateIndex(2, IndexType.LatinLower, false);
        var generator = new IndexGenerator(seeded0, seeded2);
        var other = generator.Clone();

        other.Generate(IndexType.LatinLower);
        other.Generate(IndexType.LatinLower);
        other.Generate(IndexType.GreekUpper);

        generator.MergeFrom(other);

        int mergedLatin = generator.Generate(IndexType.LatinLower);
        int mergedGreekUpper = generator.Generate(IndexType.GreekUpper);
        int nextLatinFromOther = other.Generate(IndexType.LatinLower);

        Assert.Equal(IndexType.LatinLower.GetType_(), IndicesUtils.GetType(mergedLatin));
        Assert.Equal(nextLatinFromOther, mergedLatin);
        Assert.True(IndicesUtils.GetNameWithoutType(mergedLatin) > 3);
        Assert.Equal(IndexType.GreekUpper.GetType_(), IndicesUtils.GetType(mergedGreekUpper));
        Assert.Equal(1, IndicesUtils.GetNameWithoutType(mergedGreekUpper));
    }
}
