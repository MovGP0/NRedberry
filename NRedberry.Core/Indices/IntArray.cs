using System;

namespace NRedberry.Core.Indices;

public sealed class IntArray
{
    public static IntArray EmptyArray = new IntArray(new int[0]);
    private int[] InnerArray { get; }

    public IntArray(int[] innerArray)
    {
        InnerArray = innerArray ?? throw new ArgumentNullException(nameof(innerArray));
    }

    public int this[int i] => InnerArray[i];
    public int Length => InnerArray.Length;

    public int[] Copy()
    {
        var target = new int[InnerArray.Length];
        Array.Copy(InnerArray, target, InnerArray.Length);
        return target;
    }

    public int[] Copy(int from, int to)
    {
        var target = new int[to-from];
        Array.Copy(InnerArray, from, target, 0, target.Length);
        return target;
    }

    public override bool Equals(object obj) {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is IntArray other)
        {
            return InnerArray.Equals(other.InnerArray);
        }
        return false;
    }

    public override int GetHashCode() {
        return 497 + InnerArray.GetHashCode();
    }

    public override string ToString() {
        return InnerArray.ToString();
    }
}