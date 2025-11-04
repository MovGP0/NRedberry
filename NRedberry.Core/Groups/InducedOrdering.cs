using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

using System;
using System.Collections.Generic;

public class InducedOrdering : IComparer<int>
{
    private readonly int[] positions;
    private readonly int degree;

    /// <summary>
    /// Constructs an ordering induced by the specified base.
    /// </summary>
    /// <param name="baseArray">Base permutation group.</param>
    public InducedOrdering(int[] baseArray)
    {
        if (baseArray == null || baseArray.Length == 0)
            throw new ArgumentException("Base array must not be null or empty.", nameof(baseArray));

        degree = Max(baseArray) + 1;
        positions = new int[degree + 2];

        Array.Fill(positions, -1);
        for (int i = 0; i < baseArray.Length; i++)
            positions[1 + baseArray[i]] = i;

        int next = baseArray.Length;
        for (int i = 1; i < degree + 1; i++)
        {
            if (positions[i] == -1)
                positions[i] = next++;
        }

        positions[0] = int.MinValue;
        positions[degree + 1] = int.MaxValue;
    }

    /// <summary>
    /// Returns the position of the specified point in the base or <see cref="int.MaxValue"/> if the point is not a base point.
    /// </summary>
    /// <param name="a">The point.</param>
    /// <returns>The position of the point in the base.</returns>
    public int PositionOf(int a)
    {
        return positions[a + 1];
    }

    /// <inheritdoc />
    public int Compare(int a, int b)
    {
        if (a > positions.Length - 2)
            return b > positions.Length - 2 ? a.CompareTo(b) : 1;
        if (b > positions.Length - 2)
            return a > positions.Length - 2 ? a.CompareTo(b) : -1;
        return positions[a + 1].CompareTo(positions[b + 1]);
    }

    /// <summary>
    /// Returns the greatest point under this ordering.
    /// </summary>
    /// <param name="a">First point.</param>
    /// <param name="b">Second point.</param>
    /// <returns>The greatest point.</returns>
    public int Max(int a, int b) => Compare(a, b) >= 0 ? a : b;

    /// <summary>
    /// Returns the least point under this ordering.
    /// </summary>
    /// <param name="a">First point.</param>
    /// <param name="b">Second point.</param>
    /// <returns>The least point.</returns>
    public int Min(int a, int b) => Compare(a, b) >= 0 ? b : a;

    /// <summary>
    /// Returns the maximum element representative under this ordering.
    /// </summary>
    public int MaxElement() => degree;

    /// <summary>
    /// Returns the minimum element representative under this ordering.
    /// </summary>
    public int MinElement() => -1;

    /// <summary>
    /// Returns the least point under this ordering in the specified array.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns>The least point.</returns>
    public int Min(int[] array) => array.Min();

    /// <summary>
    /// Returns the least point under this ordering in the specified <see cref="IntArrayList"/>.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns>The least point.</returns>
    public int Min(IntArrayList array) => array.Min();

    /// <summary>
    /// Returns the greatest point under this ordering in the specified array.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns>The greatest point.</returns>
    public int Max(int[] array) => array.Max();

    /// <summary>
    /// Returns the greatest point under this ordering in the specified <see cref="IntArrayList"/>.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns>The greatest point.</returns>
    public int Max(IntArrayList array) => array.Max();
}
