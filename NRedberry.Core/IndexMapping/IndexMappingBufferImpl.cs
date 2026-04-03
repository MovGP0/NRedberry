using System.Text;
using NRedberry.Contexts;
using NRedberry.Indices;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferImpl.java
 */

internal class IndexMappingBufferImpl : IIndexMappingBuffer
{
    private readonly Dictionary<int, IndexMappingBufferRecord> _map;
    private bool _sign;

    public IndexMappingBufferImpl()
        : this([], false)
    {
    }

    public IndexMappingBufferImpl(bool sign)
        : this([], sign)
    {
    }

    protected IndexMappingBufferImpl(Dictionary<int, IndexMappingBufferRecord> map, bool sign)
    {
        ArgumentNullException.ThrowIfNull(map);

        _map = map;
        _sign = sign;
    }

    public bool TryMap(int from, int to)
    {
        int fromState = IndicesUtils.GetStateInt(from);
        if (fromState != IndicesUtils.GetStateInt(to) && !TensorCC.IsMetric(IndicesUtils.GetType(from)))
        {
            return false;
        }

        int fromName = IndicesUtils.GetNameWithType(from);
        if (!_map.TryGetValue(fromName, out IndexMappingBufferRecord? record))
        {
            _map[fromName] = new IndexMappingBufferRecord(from, to);
            return true;
        }

        return record.TryMap(from, to);
    }

    public void AddSign(bool sign)
    {
        _sign ^= sign;
    }

    public void RemoveContracted()
    {
        List<int> contractedKeys = [];
        foreach ((int key, IndexMappingBufferRecord value) in _map)
        {
            if (value.IsContracted())
            {
                contractedKeys.Add(key);
            }
        }

        foreach (int key in contractedKeys)
        {
            _map.Remove(key);
        }
    }

    public bool IsEmpty()
    {
        return _map.Count == 0;
    }

    public bool GetSign()
    {
        return _sign;
    }

    public object Export()
    {
        int[] from = new int[_map.Count];
        int[] to = new int[_map.Count];
        int i = 0;
        foreach ((int key, IndexMappingBufferRecord value) in _map)
        {
            from[i] = key;
            to[i] = value.GetIndexName();
            i++;
        }

        return new IndexMappingsFromToHolder(from, to, _sign);
    }

    public IDictionary<int, IndexMappingBufferRecord> GetMap()
    {
        return _map;
    }

    public IndexMappingBufferImpl Clone()
    {
        Dictionary<int, IndexMappingBufferRecord> cloned = new(_map.Count);
        foreach ((int key, IndexMappingBufferRecord value) in _map)
        {
            cloned[key] = value.Clone();
        }

        return new IndexMappingBufferImpl(cloned, _sign);
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    public string ToString(OutputFormat format)
    {
        ArgumentNullException.ThrowIfNull(format);

        StringBuilder sb = new();
        sb.Append(_sign ? '-' : '+').Append('{');
        if (_map.Count == 0)
        {
            return sb.Append('}').ToString();
        }

        foreach ((int key, IndexMappingBufferRecord value) in _map)
        {
            string from;
            string to;
            if (value.IsContracted())
            {
                from = ToStringIndex(key, format).Substring(1);
                to = ToStringIndex(value.GetIndexName(), format).Substring(1);
                sb.Append(',');
            }
            else
            {
                from = ToStringIndex(IndicesUtils.SetRawState(value.GetFromRawState(), key), format);
                to = ToStringIndex(IndicesUtils.SetRawState(value.GetToRawState(), value.GetIndexName()), format);
            }

            sb.Append(from).Append(" -> ").Append(to).Append(", ");
        }

        sb.Length -= 2;
        sb.Append('}');
        return sb.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not IndexMappingBufferImpl other)
        {
            return false;
        }

        if (_sign != other._sign || _map.Count != other._map.Count)
        {
            return false;
        }

        foreach ((int key, IndexMappingBufferRecord value) in _map)
        {
            if (!other._map.TryGetValue(key, out IndexMappingBufferRecord? otherValue) || !value.Equals(otherValue))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int mapHash = 0;
        foreach ((int key, IndexMappingBufferRecord value) in _map)
        {
            mapHash += HashCode.Combine(key, value);
        }

        return HashCode.Combine(_sign, _map.Count, mapHash);
    }

    object ICloneable.Clone()
    {
        return Clone();
    }

    private static string ToStringIndex(int index, OutputFormat format)
    {
        return (IndicesUtils.GetState(index) ? "^" : "_")
            + Context.Get().ConverterManager.GetSymbol(index, format);
    }
}

internal sealed class IndexMappingsBufferImpl : IndexMappingBufferImpl
{
    public IndexMappingsBufferImpl()
    {
    }

    public IndexMappingsBufferImpl(bool sign)
        : base(sign)
    {
    }

    private IndexMappingsBufferImpl(Dictionary<int, IndexMappingBufferRecord> map, bool sign)
        : base(map, sign)
    {
    }

    public new IndexMappingsBufferImpl Clone()
    {
        Dictionary<int, IndexMappingBufferRecord> cloned = new(GetMap().Count);
        foreach ((int key, IndexMappingBufferRecord value) in GetMap())
        {
            cloned[key] = value.Clone();
        }

        return new IndexMappingsBufferImpl(cloned, GetSign());
    }
}
