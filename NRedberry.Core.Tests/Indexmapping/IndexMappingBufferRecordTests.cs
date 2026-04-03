using NRedberry.IndexMapping;
using NRedberry.Indices;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingBufferRecordTests
{
    private const byte DefaultType = 0;
    private const int NameA = 10;
    private const int NameB = 20;

    [Fact]
    public void ConstructorShouldStoreToNameAndStateBitsWhenStatesDiffer()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, true);

        IndexMappingBufferRecord record = new(from, to);

        record.GetIndexName().ShouldBe(IndicesUtils.GetNameWithType(to));
        record.GetStates().ShouldBe((byte)0b110);
        record.DiffStatesInitialized().ShouldBeTrue();
        record.IsContracted().ShouldBeFalse();
    }

    [Fact]
    public void ConstructorShouldStoreToNameAndStateBitsWhenStatesMatch()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        record.GetIndexName().ShouldBe(IndicesUtils.GetNameWithType(to));
        record.GetStates().ShouldBe((byte)0b001);
        record.DiffStatesInitialized().ShouldBeFalse();
        record.IsContracted().ShouldBeFalse();
    }

    [Fact]
    public void TryMapShouldReturnFalseWhenTargetNameDiffers()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);
        int differentTargetName = CreateIndex(NameB, true);

        IndexMappingBufferRecord record = new(from, to);

        bool result = record.TryMap(from, differentTargetName);

        result.ShouldBeFalse();
        record.GetStates().ShouldBe((byte)0b001);
    }

    [Fact]
    public void TryMapShouldThrowWhenStateParityIsInconsistent()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        int inconsistentFrom = CreateIndex(NameA, true);
        int matchingTargetName = CreateIndex(NameA, false);

        ShouldFailWithInconsistentMapping(() => record.TryMap(inconsistentFrom, matchingTargetName));
    }

    [Fact]
    public void TryMapShouldThrowWhenRemappingSameTargetState()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        int anotherFrom = CreateIndex(NameA, true);
        int sameTargetState = CreateIndex(NameA, false);

        ShouldFailWithInconsistentMapping(() => record.TryMap(anotherFrom, sameTargetState));
    }

    [Fact]
    public void TryMapShouldUpdateContractedStateWhenSecondStateIsMapped()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        int secondFrom = CreateIndex(NameA, true);
        int secondTo = CreateIndex(NameA, true);

        bool result = record.TryMap(secondFrom, secondTo);

        result.ShouldBeTrue();
        record.GetStates().ShouldBe((byte)0b011);
        record.IsContracted().ShouldBeTrue();
    }

    [Fact]
    public void InvertStatesShouldSwapStatesWhenNotContracted()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);
        IndexMappingBufferRecord record = new(from, to);

        byte before = record.GetStates();
        int toRawBefore = record.GetToRawState();

        record.InvertStates();

        record.GetStates().ShouldBe((byte)(before ^ 0b011));
        record.GetToRawState().ShouldNotBe(toRawBefore);
        record.IsContracted().ShouldBeFalse();
    }

    [Fact]
    public void InvertStatesShouldBeNoOpWhenContracted()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);
        IndexMappingBufferRecord record = new(from, to);
        record.TryMap(CreateIndex(NameA, true), CreateIndex(NameA, true));

        byte before = record.GetStates();
        int toRawBefore = record.GetToRawState();
        int fromRawBefore = record.GetFromRawState();

        record.InvertStates();

        record.GetStates().ShouldBe(before);
        record.GetToRawState().ShouldBe(toRawBefore);
        record.GetFromRawState().ShouldBe(fromRawBefore);
        record.IsContracted().ShouldBeTrue();
    }

    [Fact]
    public void CloneEqualsAndHashCodeShouldBeConsistent()
    {
        IndexMappingBufferRecord original = new(CreateIndex(NameA, false), CreateIndex(NameA, true));

        IndexMappingBufferRecord clone = original.Clone();

        clone.ShouldNotBeSameAs(original);
        clone.ShouldBe(original);
        clone.GetHashCode().ShouldBe(original.GetHashCode());

        clone.InvertStates();

        clone.ShouldNotBe(original);
        clone.GetHashCode().ShouldNotBe(original.GetHashCode());
    }

    private static void ShouldFailWithInconsistentMapping(Action action)
    {
        Exception exception = Record.Exception(action);
        exception.ShouldNotBeNull();

        if (exception is InconsistentIndicesException)
        {
            return;
        }

        TypeInitializationException typeInitializationException = exception.ShouldBeOfType<TypeInitializationException>();
        typeInitializationException.StackTrace.ShouldContain("InconsistentIndicesException");
    }

    private static int CreateIndex(int name, bool upper)
    {
        return IndicesUtils.CreateIndex(name, DefaultType, upper);
    }
}
