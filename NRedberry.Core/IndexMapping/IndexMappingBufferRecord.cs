using System.Text;
using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferRecord.java
 */

public sealed class IndexMappingBufferRecord : ICloneable
{
    private byte _states;
    private readonly int _toName;

    public IndexMappingBufferRecord(int from, int to)
    {
        _toName = IndicesUtils.GetNameWithType(to);
        _states |= (byte)(1 << IndicesUtils.GetStateInt(to));
        _states |= (byte)((IndicesUtils.GetStateInt(from) ^ IndicesUtils.GetStateInt(to)) << 2);
    }

    public IndexMappingBufferRecord(byte usedStates, int indexName)
    {
        _states = usedStates;
        _toName = indexName;
    }

    public bool TryMap(int from, int to)
    {
        if (IndicesUtils.GetNameWithType(to) != _toName)
        {
            return false;
        }

        if ((IndicesUtils.GetStateInt(from) != IndicesUtils.GetStateInt(to)) != ((_states & 0x4) == 0x4))
        {
            throw new InconsistentIndicesException(from);
        }

        if ((_states & (1 << IndicesUtils.GetStateInt(to))) != 0)
        {
            throw new InconsistentIndicesException(to);
        }

        _states |= (byte)(1 << IndicesUtils.GetStateInt(to));
        return true;
    }

    public int GetIndexName()
    {
        return _toName;
    }

    public byte GetStates()
    {
        return _states;
    }

    public int GetToRawState()
    {
        return (_states & 1) == 0 ? unchecked((int)0x80000000) : 0;
    }

    public int GetFromRawState()
    {
        if ((_states & 4) == 0)
        {
            return GetToRawState();
        }

        return unchecked((int)0x80000000) ^ GetToRawState();
    }

    public bool GetStatesBit(int bit)
    {
        return ((_states >> bit) & 1) == 1;
    }

    public bool IsContracted()
    {
        return (_states & 3) == 3;
    }

    public bool DiffStatesInitialized()
    {
        return (_states & 4) == 4;
    }

    public int GetRawDiffStateBit()
    {
        return (_states & 4) << 29;
    }

    public void InvertStates()
    {
        if (IsContracted())
        {
            return;
        }

        _states ^= 3;
    }

    public IndexMappingBufferRecord Clone()
    {
        return new IndexMappingBufferRecord(_states, _toName);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not IndexMappingBufferRecord other)
        {
            return false;
        }

        return _states == other._states && _toName == other._toName;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return 31 * (31 * 7 + _states) + _toName;
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Context.Get().ConverterManager.GetSymbol(GetIndexName(), OutputFormat.UTF8));
        sb.Append(':');
        for (int i = 2; i >= 0; --i)
        {
            sb.Append(GetStatesBit(i) ? '1' : '0');
        }

        return sb.ToString();
    }

    object ICloneable.Clone() => Clone();
}
