using System.Collections;

namespace NRedberry.Core.Utils;

public sealed class IntArray : IEnumerable<int>
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
        return Equals(InnerArray, array);
    }

    public IEnumerator<int> GetEnumerator()
    {
        foreach (int i in InnerArray)
        {
            yield return i;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;

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
        return string.Join(", ", InnerArray);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
