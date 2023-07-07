using System;
using System.Linq;

namespace NRedberry.Core.Indices;

public sealed class IntArray
{
    public static IntArray EmptyArray = new(new long[0]);
    private long[] InnerArray { get; }

    public IntArray(long[] innerArray)
    {
        InnerArray = innerArray ?? throw new ArgumentNullException(nameof(innerArray));
    }

    public long this[long i] => InnerArray[i];
    public long Length => InnerArray.Length;

    public long[] Copy()
    {
        var target = new long[InnerArray.Length];
        Array.Copy(InnerArray, target, InnerArray.Length);
        return target;
    }

    public long[] Copy(int from, int to)
    {
        var target = new long[to-from];
        Array.Copy(InnerArray, from, target, 0, target.Length);
        return target;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is IntArray other)
        {
            return InnerArray.Equals(other.InnerArray);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return 497 + InnerArray.GetHashCode();
    }

    public override string ToString()
    {
        return InnerArray.ToString();
    }

    public long[] ToArray() => Copy();
}