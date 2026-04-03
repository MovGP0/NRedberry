using NRedberry.IndexGeneration;
using NRedberry.Indices;

namespace NRedberry.Core.Tests.IndexGeneration;

public sealed class IndexGeneratorTests
{
    [Fact]
    public void EmptyGeneratorGenerateShouldReturnRequestedTypeBits()
    {
        var generator = new IndexGenerator();

        int generated = generator.Generate(IndexType.GreekUpper);

        IndicesUtils.GetType(generated).ShouldBe(IndexType.GreekUpper.GetType_());
        IndicesUtils.GetNameWithoutType(generated).ShouldBe(0);
    }

    [Fact]
    public void SeededConstructorGenerateShouldSkipExistingNamesForType()
    {
        int seeded0 = IndicesUtils.CreateIndex(0, IndexType.LatinLower, false);
        int seeded2 = IndicesUtils.CreateIndex(2, IndexType.LatinLower, true);
        var generator = new IndexGenerator(seeded0, seeded2);

        int firstGenerated = generator.Generate(IndexType.LatinLower);
        int secondGenerated = generator.Generate(IndexType.LatinLower);

        IndicesUtils.GetNameWithoutType(firstGenerated).ShouldBe(1);
        IndicesUtils.GetNameWithoutType(secondGenerated).ShouldBe(3);
        IndicesUtils.GetType(firstGenerated).ShouldBe(IndexType.LatinLower.GetType_());
        IndicesUtils.GetType(secondGenerated).ShouldBe(IndexType.LatinLower.GetType_());
    }

    [Fact]
    public void ContainsShouldRecognizeSeededIndicesAndRejectAbsent()
    {
        int seededLatin = IndicesUtils.CreateIndex(4, IndexType.LatinUpper, false);
        int seededGreek = IndicesUtils.CreateIndex(7, IndexType.GreekLower, true);
        var generator = new IndexGenerator(seededLatin, seededGreek);

        generator.Contains(seededLatin).ShouldBeTrue();
        generator.Contains(IndicesUtils.Raise(seededLatin)).ShouldBeTrue();
        generator.Contains(seededGreek).ShouldBeTrue();
        generator.Contains(IndicesUtils.CreateIndex(9, IndexType.LatinUpper, false)).ShouldBeFalse();
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

        IndicesUtils.GetType(mergedLatin).ShouldBe(IndexType.LatinLower.GetType_());
        mergedLatin.ShouldBe(nextLatinFromOther);
        IndicesUtils.GetNameWithoutType(mergedLatin).ShouldBeGreaterThan(3);
        IndicesUtils.GetType(mergedGreekUpper).ShouldBe(IndexType.GreekUpper.GetType_());
        IndicesUtils.GetNameWithoutType(mergedGreekUpper).ShouldBe(1);
    }
}
