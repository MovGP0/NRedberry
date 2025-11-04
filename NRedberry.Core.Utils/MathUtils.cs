namespace NRedberry.Core.Utils;

public static class MathUtils
{
    /// <summary>
    /// Sort array and return array with removed repetitive values.
    /// </summary>
    /// <param name="values">Input array (this method will sort this array)</param>
    /// <returns>Sorted array of distinct values</returns>
    public static int[] GetSortedDistinct(int[] values)
    {
        if (values.Length == 0)
            return values;
        Array.Sort(values);
        int shift = 0;
        int i = 0;
        while (i + shift + 1 < values.Length)
        {
            if (values[i + shift] == values[i + shift + 1])
            {
                ++shift;
            }
            else
            {
                values[i] = values[i + shift];
                ++i;
            }
        }

        values[i] = values[i + shift];
        return values[..(i + 1)];
    }

    /// <summary>
    /// Return the set difference B - A for int sets A and B.
    /// Sets A and B must be represented as two sorted int arrays.
    /// Repetitive values in A or B are not allowed.
    /// </summary>
    /// <param name="a">Sorted array of distinct values. (set A)</param>
    /// <param name="b">Sorted array of distinct values. (set B)</param>
    /// <returns>The set of elements in B but not in A</returns>
    public static int[] IntSetDifference(int[] a, int[] b)
    {
        int bPointer = 0;
        int aPointer = 0;
        int counter = 0;
        while (aPointer < a.Length && bPointer < b.Length)
        {
            if (a[aPointer] == b[bPointer])
            {
                aPointer++;
                bPointer++;
            }
            else if (a[aPointer] < b[bPointer])
            {
                aPointer++;
            }
            else
            {
                counter++;
                bPointer++;
            }
        }

        counter += b.Length - bPointer;
        int[] result = new int[counter];
        counter = 0;
        aPointer = 0;
        bPointer = 0;
        while (aPointer < a.Length && bPointer < b.Length)
        {
            if (a[aPointer] == b[bPointer])
            {
                aPointer++;
                bPointer++;
            }
            else if (a[aPointer] < b[bPointer])
            {
                aPointer++;
            }
            else
            {
                result[counter++] = b[bPointer++];
            }
        }

        Array.Copy(b, bPointer, result, counter, b.Length - bPointer);
        return result;
    }

    /// <summary>
    /// Return the union B + A for integer sets A and B.
    /// Sets A and B must be represented as two sorted integer arrays.
    /// Repetitive values in A or B are not allowed.
    /// </summary>
    /// <param name="a">Sorted array of distinct values. (set A)</param>
    /// <param name="b">Sorted array of distinct values. (set B)</param>
    /// <returns>The set of elements from B and from A</returns>
    public static int[] IntSetUnion(int[] a, int[] b)
    {
        int bPointer = 0;
        int aPointer = 0;
        int counter = 0;
        while (aPointer < a.Length && bPointer < b.Length)
        {
            if (a[aPointer] == b[bPointer])
            {
                aPointer++;
                bPointer++;
                counter++;
            }
            else if (a[aPointer] < b[bPointer])
            {
                aPointer++;
                counter++;
            }
            else
            {
                counter++;
                bPointer++;
            }
        }

        counter += (a.Length - aPointer) + (b.Length - bPointer); // Assert aPointer==a.Length || bPointer==b.Length
        int[] result = new int[counter];
        counter = 0;
        aPointer = 0;
        bPointer = 0;
        while (aPointer < a.Length && bPointer < b.Length)
        {
            if (a[aPointer] == b[bPointer])
            {
                result[counter++] = b[bPointer];
                aPointer++;
                bPointer++;
            }
            else if (a[aPointer] < b[bPointer])
            {
                result[counter++] = a[aPointer++];
            }
            else
            {
                result[counter++] = b[bPointer++];
            }
        }

        if (aPointer == a.Length)
        {
            Array.Copy(b, bPointer, result, counter, b.Length - bPointer);
        }
        else
        {
            Array.Copy(a, aPointer, result, counter, a.Length - aPointer);
        }

        return result;
    }
}
