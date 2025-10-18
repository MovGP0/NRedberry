using NRedberry.Core.Indices;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Tensors;

public sealed class IndexMapper : IIndexMapping
{
    public IndexMapper(int[] from, int[] to) {
        this.from = from;
        this.to = to;
    }

    private int[] from, to;

    public int Map(int index)
    {
         int position = Arrays.BinarySearch(from, IndicesUtils.GetNameWithType(index));
         if (position < 0)
         {
             return index;
         }

         return IndicesUtils.GetRawStateInt(index) ^ to[position];
    }

    public bool Contract(params int[] freeIndicesNames)
    {
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