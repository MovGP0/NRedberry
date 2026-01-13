using System.Collections;

namespace NRedberry.Core.Utils;

[Obsolete("use ImmutableArray<int> instead", true)]
public sealed class IntArray : IEnumerable<int>, IEquatable<IntArray>
{
    public static readonly IntArray EmptyArray = new([]);
    public readonly int[] InnerArray;

    /// <summary>
    /// Generic integer array to be wrapped.
    /// </summary>
    /// <param name="innerArray">The inner array.</param>
    public IntArray(int[] innerArray)
    {
        InnerArray = innerArray;
    }

    /// <summary>
    /// Returns the integer at the specified position in this IntArray.
    /// </summary>
    /// <param name="i">Position of the integer to return.</param>
    /// <returns>The integer at the specified position in this IntArray.</returns>
    public int Get(int i)
    {
        return InnerArray[i];
    }

    /// <summary>
    /// Returns number of elements in this IntArray.
    /// </summary>
    /// <returns>Number of elements in this array.</returns>
    public int Length()
    {
        return InnerArray.Length;
    }

    /// <summary>
    /// Returns a new integer array with a copy of this IntArray data.
    /// </summary>
    /// <returns>Integer array with a copy of this IntArray data.</returns>
    public int[] Copy()
    {
        return (int[])InnerArray.Clone();
    }

    /// <summary>
    /// Returns a new integer array with a copy of a range of this IntArray data.
    /// </summary>
    /// <param name="from">The start index (inclusive).</param>
    /// <param name="to">The end index (exclusive).</param>
    /// <returns>Integer array with a copy of this IntArray data.</returns>
    public int[] Copy(int from, int to)
    {
        int length = to - from;
        int[] result = new int[length];
        Array.Copy(InnerArray, from, result, 0, length);
        return result;
    }

    /// <summary>
    /// Compares this IntArray with a specified array for equality.
    /// </summary>
    /// <param name="array">The array to compare with.</param>
    /// <returns>True if arrays are equal; otherwise, false.</returns>
    public bool EqualsToArray(int[] array)
    {
        return AreArraysEqual(InnerArray, array);
    }

    public bool Equals(IntArray? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AreArraysEqual(InnerArray, other.InnerArray);
    }

    public override bool Equals(object? obj)
    {
        return obj is IntArray other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        foreach (int value in InnerArray)
        {
            hashCode.Add(value);
        }

        return 497 + hashCode.ToHashCode();
    }

    public static bool operator ==(IntArray? left, IntArray? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(IntArray? left, IntArray? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", InnerArray)}]";
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<int> GetEnumerator()
    {
        foreach (int i in InnerArray)
        {
            yield return i;
        }
    }

    private static bool AreArraysEqual(int[] left, int[] right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left.Length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }

        return true;
    }
}
