using System.Collections.Immutable;
using System.Text;
using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.IndexMapping;

/// <summary>
/// Represents a mapping of tensor indices.
/// </summary>
public sealed class Mapping : ITransformation
{
    private readonly int[] _fromNames;
    private readonly int[] _toData;
    private readonly bool _sign;

    public static Mapping IdentityMapping { get; } = new([], [], false, true);

    public Mapping(int[] from, int[] to)
        : this(from, to, false)
    {
    }

    public Mapping(int[] from, int[] to, bool sign)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Length != to.Length)
        {
            throw new ArgumentException("From length != to length.");
        }

        _fromNames = new int[from.Length];
        _toData = new int[from.Length];
        for (int i = 0; i < from.Length; ++i)
        {
            _fromNames[i] = IndicesUtils.GetNameWithType(from[i]);
            _toData[i] = IndicesUtils.GetRawStateInt(from[i]) ^ to[i];
        }

        ArraysUtils.QuickSort(_fromNames, _toData);
        _sign = sign;
    }

    internal Mapping(IIndexMappingBuffer buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        IDictionary<int, IndexMappingBufferRecord> map = buffer.GetMap();
        _fromNames = new int[map.Count];
        _toData = new int[map.Count];
        int i = 0;
        foreach ((int key, IndexMappingBufferRecord record) in map)
        {
            _fromNames[i] = key;
            _toData[i] = record.GetRawDiffStateBit() | record.GetIndexName();
            ++i;
        }

        ArraysUtils.QuickSort(_fromNames, _toData);
        _sign = buffer.GetSign();
    }

    private Mapping(int[] fromNames, int[] toData, bool sign, bool direct)
    {
        ArgumentNullException.ThrowIfNull(fromNames);
        ArgumentNullException.ThrowIfNull(toData);

        _fromNames = direct ? fromNames : (int[])fromNames.Clone();
        _toData = direct ? toData : (int[])toData.Clone();
        _sign = sign;
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return ApplyIndexMapping.ApplyIndexMappingAutomatically(tensor, this);
    }

    public bool IsEmpty()
    {
        return _fromNames.Length == 0;
    }

    public bool IsIdentity()
    {
        return _fromNames.Length == 0 && !_sign;
    }

    public bool GetSign()
    {
        return _sign;
    }

    public Mapping AddSign(bool sign)
    {
        return new Mapping(_fromNames, _toData, sign ^ _sign, true);
    }

    public int Size()
    {
        return _fromNames.Length;
    }

    public ImmutableArray<int> GetFromNames()
    {
        return [.._fromNames];
    }

    public ImmutableArray<int> GetToData()
    {
        return [.._toData];
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Mapping other)
        {
            return false;
        }

        return _sign == other._sign
            && _fromNames.AsSpan().SequenceEqual(other._fromNames)
            && _toData.AsSpan().SequenceEqual(other._toData);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        foreach (int fromName in _fromNames)
        {
            hashCode.Add(fromName);
        }

        foreach (int toDatum in _toData)
        {
            hashCode.Add(toDatum);
        }

        hashCode.Add(_sign);
        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        int lastIndex = _fromNames.Length - 1;
        if (lastIndex < 0)
        {
            return _sign ? "-{}" : "{}";
        }

        StringBuilder sb = new();
        if (_sign)
        {
            sb.Append('-');
        }

        sb.Append('{');
        for (int i = 0; ; ++i)
        {
            sb.Append(IndicesUtils.ToString(_fromNames[i]));
            sb.Append("->");
            sb.Append(IndicesUtils.ToString(_toData[i]));
            if (i == lastIndex)
            {
                break;
            }

            sb.Append(", ");
        }

        sb.Append('}');
        return sb.ToString();
    }

    public static Mapping ValueOf(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string text = value.Trim();
        int start = 0;
        bool sign = false;
        if (text.Length == 0)
        {
            throw new ArgumentException("Not valid syntax for mapping: " + value, nameof(value));
        }

        if (text[0] == '-')
        {
            sign = true;
            start = 1;
        }
        else if (text[0] == '+')
        {
            start = 1;
        }

        if (start >= text.Length || text[start] != '{' || text[^1] != '}')
        {
            throw new ArgumentException("Not valid syntax for mapping: " + value, nameof(value));
        }

        string content = text.Substring(start + 1, text.Length - start - 2).Trim();
        if (content.Length == 0)
        {
            return new Mapping([], [], sign);
        }

        string[] pairs = content.Split(',', StringSplitOptions.TrimEntries);
        int[] from = new int[pairs.Length];
        int[] to = new int[pairs.Length];
        for (int i = 0; i < pairs.Length; ++i)
        {
            string[] fromTo = pairs[i].Split("->", StringSplitOptions.None);
            if (fromTo.Length != 2)
            {
                throw new ArgumentException("Not valid syntax for mapping: " + value, nameof(value));
            }

            int fromIndex = IndicesUtils.ParseIndex(fromTo[0]);
            from[i] = IndicesUtils.GetNameWithType(fromIndex);
            to[i] = IndicesUtils.GetRawStateInt(fromIndex) ^ IndicesUtils.ParseIndex(fromTo[1]);
        }

        return new Mapping(from, to, sign);
    }
}
