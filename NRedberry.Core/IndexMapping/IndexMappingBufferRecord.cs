using System;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferRecord.java
 */

public sealed class IndexMappingBufferRecord : ICloneable
{
    public IndexMappingBufferRecord(int from, int to)
    {
        throw new NotImplementedException();
    }

    public IndexMappingBufferRecord(byte usedStates, int indexName)
    {
        throw new NotImplementedException();
    }

    public bool TryMap(int from, int to)
    {
        throw new NotImplementedException();
    }

    public int GetIndexName()
    {
        throw new NotImplementedException();
    }

    public byte GetStates()
    {
        throw new NotImplementedException();
    }

    public int GetToRawState()
    {
        throw new NotImplementedException();
    }

    public int GetFromRawState()
    {
        throw new NotImplementedException();
    }

    public bool GetStatesBit(int bit)
    {
        throw new NotImplementedException();
    }

    public bool IsContracted()
    {
        throw new NotImplementedException();
    }

    public bool DiffStatesInitialized()
    {
        throw new NotImplementedException();
    }

    public int GetRawDiffStateBit()
    {
        throw new NotImplementedException();
    }

    public void InvertStates()
    {
        throw new NotImplementedException();
    }

    public IndexMappingBufferRecord Clone()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    object ICloneable.Clone() => Clone();
}
