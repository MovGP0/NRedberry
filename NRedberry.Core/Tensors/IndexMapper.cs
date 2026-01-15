using NRedberry.Core.Utils;
using NRedberry.Indices;

namespace NRedberry.Tensors;

public sealed class IndexMapper : IIndexMapping
{
    private readonly int[] _from;
    private readonly int[] _to;

    public IndexMapper(int[] from, int[] to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        _from = from;
        _to = to;
    }

    public int Map(int index)
    {
        int position = Arrays.BinarySearch(_from, IndicesUtils.GetNameWithType(index));
        if (position < 0)
        {
            return index;
        }

        return IndicesUtils.GetRawStateInt(index) ^ _to[position];
    }

    public bool Contract(params int[] freeIndicesNames)
    {
        ArgumentNullException.ThrowIfNull(freeIndicesNames);

        if (freeIndicesNames.Length <= 1)
        {
            return false;
        }

        for (var i = 0; i < freeIndicesNames.Length; ++i)
        {
            freeIndicesNames[i] = 0x7FFFFFFF & Map(freeIndicesNames[i]);
        }

        Array.Sort(freeIndicesNames);
        for (var i = 1; i < freeIndicesNames.Length; ++i)
        {
            if (freeIndicesNames[i] == freeIndicesNames[i - 1])
            {
                return true;
            }
        }

        return false;
    }
}
