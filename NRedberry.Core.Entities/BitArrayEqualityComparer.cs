using System.Collections;

namespace NRedberry;

public class BitArrayEqualityComparer : IEqualityComparer<BitArray>
{
    public bool Equals(BitArray? x, BitArray? y)
    {
        if (x == null || y == null)
            return false;
        return x.Cast<bool>().SequenceEqual(y.Cast<bool>());
    }

    public int GetHashCode(BitArray obj)
    {
        return obj.Cast<bool>().Aggregate(0, (current, bit) => unchecked(current * 23 + bit.GetHashCode()));
    }
}
