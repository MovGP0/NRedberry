using NRedberry.IndexMapping;
using NRedberry.Indices;
using Xunit;

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

        Assert.Equal(IndicesUtils.GetNameWithType(to), record.GetIndexName());
        Assert.Equal(0b110, record.GetStates());
        Assert.True(record.DiffStatesInitialized());
        Assert.False(record.IsContracted());
    }

    [Fact]
    public void ConstructorShouldStoreToNameAndStateBitsWhenStatesMatch()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        Assert.Equal(IndicesUtils.GetNameWithType(to), record.GetIndexName());
        Assert.Equal(0b001, record.GetStates());
        Assert.False(record.DiffStatesInitialized());
        Assert.False(record.IsContracted());
    }

    [Fact]
    public void TryMapShouldReturnFalseWhenTargetNameDiffers()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);
        int differentTargetName = CreateIndex(NameB, true);

        IndexMappingBufferRecord record = new(from, to);

        bool result = record.TryMap(from, differentTargetName);

        Assert.False(result);
        Assert.Equal(0b001, record.GetStates());
    }

    [Fact]
    public void TryMapShouldThrowWhenStateParityIsInconsistent()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        int inconsistentFrom = CreateIndex(NameA, true);
        int matchingTargetName = CreateIndex(NameA, false);

        AssertInconsistentMappingFailure(() => record.TryMap(inconsistentFrom, matchingTargetName));
    }

    [Fact]
    public void TryMapShouldThrowWhenRemappingSameTargetState()
    {
        int from = CreateIndex(NameA, false);
        int to = CreateIndex(NameA, false);

        IndexMappingBufferRecord record = new(from, to);

        int anotherFrom = CreateIndex(NameA, true);
        int sameTargetState = CreateIndex(NameA, false);

        AssertInconsistentMappingFailure(() => record.TryMap(anotherFrom, sameTargetState));
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

        Assert.True(result);
        Assert.Equal(0b011, record.GetStates());
        Assert.True(record.IsContracted());
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

        Assert.Equal((byte)(before ^ 0b011), record.GetStates());
        Assert.NotEqual(toRawBefore, record.GetToRawState());
        Assert.False(record.IsContracted());
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

        Assert.Equal(before, record.GetStates());
        Assert.Equal(toRawBefore, record.GetToRawState());
        Assert.Equal(fromRawBefore, record.GetFromRawState());
        Assert.True(record.IsContracted());
    }

    [Fact]
    public void CloneEqualsAndHashCodeShouldBeConsistent()
    {
        IndexMappingBufferRecord original = new(CreateIndex(NameA, false), CreateIndex(NameA, true));

        IndexMappingBufferRecord clone = original.Clone();

        Assert.NotSame(original, clone);
        Assert.Equal(original, clone);
        Assert.Equal(original.GetHashCode(), clone.GetHashCode());

        clone.InvertStates();

        Assert.NotEqual(original, clone);
        Assert.NotEqual(original.GetHashCode(), clone.GetHashCode());
    }

    private static void AssertInconsistentMappingFailure(Action action)
    {
        Exception exception = Record.Exception(action);
        Assert.NotNull(exception);

        if (exception is InconsistentIndicesException)
        {
            return;
        }

        TypeInitializationException typeInitializationException = Assert.IsType<TypeInitializationException>(exception);
        Assert.Contains("InconsistentIndicesException", typeInitializationException.StackTrace);
    }

    private static int CreateIndex(int name, bool upper)
    {
        return IndicesUtils.CreateIndex(name, DefaultType, upper);
    }
}
