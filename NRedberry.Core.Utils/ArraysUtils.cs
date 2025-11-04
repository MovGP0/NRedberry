using System.Text;

namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    /// <summary>
    /// Returns the index of the median of the three indexed integers.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    private static int med3(int[] x, int a, int b, int c)
    {
        return x[a] < x[b]
            ? x[b] < x[c] ? b : x[a] < x[c] ? c : a
            : x[b] > x[c] ? b : x[a] > x[c] ? c : a;
    }

    /// <summary>
    /// Check that fromIndex and toIndex are in range, and throw an appropriate exception if they aren't.
    /// </summary>
    /// <param name="arrayLen"></param>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    private static void rangeCheck(int arrayLen, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex)
            throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})");
        if (fromIndex < 0)
            throw new IndexOutOfRangeException(nameof(fromIndex));
        if (toIndex > arrayLen)
            throw new IndexOutOfRangeException(nameof(toIndex));
    }

    public static string ToString<T>(T[] a, IToStringConverter<T> format)
    {
        if (a == null)
            return "null";
        int iMax = a.Length - 1;
        if (iMax == -1)
            return "[]";

        var b = new StringBuilder();
        b.Append('[');
        for (int i = 0; ; i++)
        {
            b.Append(format.ToString(a[i]));
            if (i == iMax)
                return b.Append(']').ToString();
            b.Append(", ");
        }
    }

    public static string ToString(int[] a, IToStringConverter<int> format)
    {
        if (a == null)
            return "null";
        int iMax = a.Length - 1;
        if (iMax == -1)
            return "[]";

        var b = new StringBuilder();
        b.Append('[');
        for (int i = 0; ; i++)
        {
            b.Append(format.ToString(a[i]));
            if (i == iMax)
                return b.Append(']').ToString();
            b.Append(", ");
        }
    }

    public static void TimSort(int[] target, int[] coSort)
    {
        IntTimSort.Sort(target, coSort);
    }

    private static void RangeCheck(int arrayLen, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex)
            throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})");
        if (fromIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(fromIndex), $"fromIndex({fromIndex}) must not be less than zero.");
        if (toIndex > arrayLen)
            throw new ArgumentOutOfRangeException(nameof(toIndex), $"toIndex({toIndex}) is greater than array length ({arrayLen}).");
    }

    public static void StableSort(int[] target, int[] coSort)
    {
        if (target.Length > 100)
        {
            TimSort(target, coSort);
        }
        else
        {
            InsertionSort(target, coSort);
        }
    }

    public static sbyte[] Int2byte(int[] a) {
        sbyte[] r = new sbyte[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = (sbyte)a[i];
        }

        return r;
    }

    public static short[] Int2short(int[] a) {
        short[] r = new short[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = (short)a[i];
        }

        return r;
    }

    public static int[] ShortToInt(short[] a)
    {
        int[] r = new int[a.Length];
        for (int i = 0; i < a.Length; ++i)
            r[i] = a[i];
        return r;
    }

    public static short[] IntToShort(int[] a)
    {
        short[] r = new short[a.Length];
        for (int i = 0; i < a.Length; ++i)
            r[i] = (short)a[i];
        return r;
    }

    public static int[] ByteToInt(sbyte[] a)
    {
        int[] r = new int[a.Length];
        for (int i = 0; i < a.Length; ++i)
            r[i] = a[i];
        return r;
    }

    public static short[] ByteToShort(sbyte[] a)
    {
        short[] r = new short[a.Length];
        for (int i = 0; i < a.Length; ++i)
            r[i] = a[i];
        return r;
    }

    public static byte[] IntToByte(int[] a)
    {
        byte[] r = new byte[a.Length];
        for (int i = 0; i < a.Length; ++i)
            r[i] = (byte)a[i];
        return r;
    }

    public static int Max(int[] array)
    {
        int a = -1;
        foreach (int i in array)
            a = Math.Max(a, i);
        return a;
    }

    public static int[] GetSeriesFrom0(int size)
    {
        int[] ret = new int[size];
        for (int i = size - 1; i >= 0; --i)
            ret[i] = i;
        return ret;
    }

    public static int[][] DeepClone(int[][] input)
    {
        int[][] res = new int[input.Length][];
        for (int i = res.Length - 1; i >= 0; --i)
            res[i] = (int[])input[i].Clone();
        return res;
    }

    public static int Sum(int[] array)
    {
        int s = 0;
        foreach (int i in array)
            s += i;
        return s;
    }

    public static int[]? Bijection<T>(T[] from, T[] to, IComparer<T> comparator)
    {
        if (from.Length != to.Length)
            return null;

        int length = from.Length;
        int[] bijection = new int[length];
        Array.Fill(bijection, -1);
        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < length; ++j)
            {
                if (bijection[j] == -1 && comparator.Compare(from[i], to[j]) == 0)
                {
                    bijection[j] = i;
                    break;
                }
            }

            if (Array.IndexOf(bijection, -1) == -1)
                return bijection;
        }

        return null;
    }

    public static int[]? Bijection<T>(T[] from, T[] to) where T : IComparable<T>
    {
        if (from.Length != to.Length)
            return null;

        int length = from.Length;
        int[] bijection = new int[length];
        Array.Fill(bijection, -1);
        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < length; ++j)
            {
                if (bijection[j] == -1 && from[i].CompareTo(to[j]) == 0)
                {
                    bijection[j] = i;
                    break;
                }
            }

            if (Array.IndexOf(bijection, -1) == -1)
                return bijection;
        }

        return null;
    }

    public static int[] AddAll(int[] array1, params int[] array2)
    {
        int[] r = new int[array1.Length + array2.Length];
        Array.Copy(array1, 0, r, 0, array1.Length);
        Array.Copy(array2, 0, r, array1.Length, array2.Length);
        return r;
    }

    public static int[] AddAll(params int[][] arrays)
    {
        if (arrays.Length == 0)
            return [];
        int length = 0;
        foreach (int[] array in arrays)
            length += array.Length;

        if (length == 0)
            return [];

        int[] r = new int[length];
        int pointer = 0;
        foreach (int[] array in arrays)
        {
            Array.Copy(array, 0, r, pointer, array.Length);
            pointer += array.Length;
        }

        return r;
    }

    public static T[] Remove<T>(T[] array, int i)
    {
        T[] r = new T[array.Length - 1];
        Array.Copy(array, 0, r, 0, i);
        if (i < array.Length - 1)
            Array.Copy(array, i + 1, r, i, array.Length - i - 1);
        return r;
    }

    public static T[] Remove<T>(T[] array, int[] positions)
    {
        if (array == null)
            throw new ArgumentNullException();
        int[] p = MathUtils.GetSortedDistinct(positions);
        if (p.Length == 0)
            return array;

        int size = p.Length;
        int pointer = 0;
        int s = array.Length;
        for (; pointer < size; ++pointer)
        {
            if (p[pointer] >= s)
                throw new IndexOutOfRangeException();
        }

        T[] r = new T[array.Length - p.Length];
        pointer = 0;
        int i = -1;
        for (int j = 0; j < s; ++j)
        {
            if (pointer < size - 1 && j > p[pointer])
                ++pointer;
            if (j == p[pointer])
                continue;
            r[++i] = array[j];
        }

        return r;
    }

    public static int[] Remove(int[] array, int[] positions)
    {
        if (array == null)
            throw new ArgumentNullException();
        int[] p = MathUtils.GetSortedDistinct(positions);
        if (p.Length == 0)
            return array;

        int size = p.Length;
        int pointer = 0;
        int s = array.Length;
        for (; pointer < size; ++pointer)
        {
            if (p[pointer] >= s)
                throw new IndexOutOfRangeException();
        }

        int[] r = new int[array.Length - p.Length];
        pointer = 0;
        int i = -1;
        for (int j = 0; j < s; ++j)
        {
            if (pointer < size - 1 && j > p[pointer])
                ++pointer;
            if (j == p[pointer])
            {
                continue;
            }

            r[++i] = array[j];
        }

        return r;
    }

    public static T[] Select<T>(T[] array, int[] positions)
    {
        if (array == null)
            throw new ArgumentNullException();
        int[] p = MathUtils.GetSortedDistinct(positions);
        T[] r = new T[p.Length];
        for (int i = 0; i < p.Length; ++i)
            r[i] = array[p[i]];
        return r;
    }

    public static int[] ToArray(HashSet<int> set)
    {
        int[] a = new int[set.Count];
        set.CopyTo(a);
        return a;
    }

    public static void Fill(IntArrayList list, int fromIndex, int toIndex, int value)
    {
        if (toIndex >= list.Count)
            throw new IndexOutOfRangeException();
        Array.Fill(list.Data, value, fromIndex, toIndex - fromIndex);
    }

    public static void Fill(IntArrayList list, int value)
    {
        Fill(list, 0, list.Count, value);
    }

    public static int BinarySearch(IntArrayList list, int key)
    {
        return Array.BinarySearch(list.Data, 0, list.Count, key);
    }

    public static int BinarySearch(IntArray array, int key)
    {
        return Array.BinarySearch(array.InnerArray, key);
    }

    public static int BinarySearch1(int[] a, int key)
    {
        return BinarySearch1(a, 0, a.Length, key);
    }

    public static int BinarySearch1(int[] a, int fromIndex, int toIndex, int key)
    {
        int low = fromIndex;
        int high = toIndex - 1;

        while (low <= high)
        {
            int mid = (low + high) >> 1;
            int midVal = a[mid];

            if (midVal < key)
            {
                low = mid + 1;
            }
            else if (midVal > key)
            {
                high = mid - 1;
            }
            else
            {
                while (mid > 0 && a[mid - 1] == midVal)
                    --mid;
                return mid;
            }
        }

        if (low >= a.Length)
            return low;
        while (low > 0 && a[low - 1] == a[low])
            --low;
        return low;
    }

    public static int CommutativeHashCode(params object[] objects)
    {
        if (objects == null)
            return 0;

        int hash = 0;
        foreach (object o in objects)
            hash ^= o == null ? 0 : o.GetHashCode();
        return HashFunctions.JenkinWang32shift(hash);
    }

    public static int CommutativeHashCode(object[] objects, int from, int to)
    {
        if (objects == null)
            return 0;
        int hash = 0;
        for (int i = from; i < to; ++i)
            hash ^= objects[i] == null ? 0 : objects[i].GetHashCode();
        return HashFunctions.JenkinWang32shift(hash);
    }

    private static int Med3(int[] x, int a, int b, int c)
    {
        return x[a] < x[b] ? x[b] < x[c] ? b : x[a] < x[c] ? c : a : x[b] > x[c] ? b : x[a] > x[c] ? c : a;
    }

    private static int Med3(long[] x, int a, int b, int c)
    {
        return x[a] < x[b] ? x[b] < x[c] ? b : x[a] < x[c] ? c : a : x[b] > x[c] ? b : x[a] > x[c] ? c : a;
    }

    private static int Med3<T>(T[] x, int a, int b, int c) where T : IComparable<T>
    {
        return x[a].CompareTo(x[b]) < 0 ? x[b].CompareTo(x[c]) < 0 ? b : x[a].CompareTo(x[c]) < 0 ? c : a : x[b].CompareTo(x[c]) > 0 ? b : x[a].CompareTo(x[c]) > 0 ? c : a;
    }

    public static bool Equals(int[] a, int[] b)
    {
        if (a == b)
            return true;
        if (a == null || b == null)
            return false;

        int length = a.Length;
        if (b.Length != length)
            return false;

        for (int i = 0; i < length; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }
}
