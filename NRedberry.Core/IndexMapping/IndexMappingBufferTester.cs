using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferTester.java
 */

internal sealed class IndexMappingsBufferTester : IIndexMappingBuffer
{
    private IndexMappingsBufferImpl _innerBuffer;
    private readonly int[] _from;
    private readonly int[] _to;
    private readonly bool _signum;

    public IndexMappingsBufferTester(int[] from, bool sign)
    {
        ArgumentNullException.ThrowIfNull(from);

        _from = IndicesUtils.GetIndicesNames(from);
        Array.Sort(_from);
        _to = _from;
        _signum = sign;
        _innerBuffer = new IndexMappingsBufferImpl();
    }

    public IndexMappingsBufferTester(Mapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        _from = mapping.GetFromNames().ToArray();
        _to = IndicesUtils.GetIndicesNames(mapping.GetToData());
        _signum = mapping.GetSign();
        _innerBuffer = new IndexMappingsBufferImpl();
    }

    public IndexMappingsBufferTester(IIndexMappingBuffer buffer)
        : this(buffer.Export())
    {
    }

    private IndexMappingsBufferTester(object holderObject)
    {
        ArgumentNullException.ThrowIfNull(holderObject);

        IndexMappingsFromToHolder holder = holderObject as IndexMappingsFromToHolder
            ?? throw new ArgumentException("Unsupported buffer export payload.", nameof(holderObject));
        _from = holder.From;
        _to = holder.To;
        _signum = holder.Sign;
        _innerBuffer = new IndexMappingsBufferImpl();
    }

    private IndexMappingsBufferTester(IndexMappingsBufferImpl innerBuffer, int[] from, int[] to, bool signum)
    {
        ArgumentNullException.ThrowIfNull(innerBuffer);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        _innerBuffer = innerBuffer;
        _from = from;
        _to = to;
        _signum = signum;
    }

    public static bool Test(IndexMappingsBufferTester tester, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(tester);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        tester.Reset();
        IIndexMappingProvider provider = IndexMappings.CreatePort(
            IndexMappingProviderUtil.Singleton(tester),
            from,
            to);
        provider.Tick();
        IIndexMappingBuffer? buffer;
        while ((buffer = provider.Take()) is not null)
        {
            if (!buffer.GetSign())
            {
                return true;
            }
        }

        return false;
    }

    public bool TryMap(int from, int to)
    {
        int fromName = IndicesUtils.GetNameWithType(from);
        int toName = IndicesUtils.GetNameWithType(to);
        int position = Array.BinarySearch(_from, fromName);
        if (position < 0)
        {
            return _innerBuffer.TryMap(from, to);
        }

        return _to[position] == toName;
    }

    public void AddSign(bool sign)
    {
        _innerBuffer.AddSign(sign);
    }

    public void RemoveContracted()
    {
        _innerBuffer.RemoveContracted();
    }

    public bool IsEmpty()
    {
        return _innerBuffer.IsEmpty();
    }

    public bool GetSign()
    {
        return _signum ^ _innerBuffer.GetSign();
    }

    public object Export()
    {
        IDictionary<int, IndexMappingBufferRecord> map = _innerBuffer.GetMap();
        int size = _from.Length + map.Count;
        int[] from = new int[size];
        int[] to = new int[size];
        Array.Copy(_from, from, _from.Length);
        Array.Copy(_to, to, _from.Length);
        int i = _from.Length;
        foreach ((int key, IndexMappingBufferRecord value) in map)
        {
            from[i] = key;
            to[i] = value.GetIndexName();
            ++i;
        }

        return new IndexMappingsFromToHolder(from, to, GetSign());
    }

    public IDictionary<int, IndexMappingBufferRecord> GetMap()
    {
        return _innerBuffer.GetMap();
    }

    public IndexMappingsBufferTester Clone()
    {
        return new IndexMappingsBufferTester(_innerBuffer.Clone(), _from, _to, _signum);
    }

    public void Reset()
    {
        _innerBuffer = new IndexMappingsBufferImpl();
    }

    public override string ToString()
    {
        return "inner: " + _innerBuffer;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}
