using System;
using System.Text;

namespace NRedberry.Core.Utils;

public static class ArraysUtils
{
    public static void quickSort(int[] target, int[] coSort)
    {
        quickSort(target, 0, target.Length, coSort);
    }

    public static void quickSort(int[] target, int fromIndex, int toIndex, int[] coSort)
    {
        throw new NotImplementedException();
    }

    public static void quickSort1(int[] target, int fromIndex, int length, int[] coSort)
    {
        throw new NotImplementedException();
    }

    private static void quickSort2(int[] target, int fromIndex, int length, int[] coSort)
    {
        throw new NotImplementedException();
    }

    private static void swap(int[] x, int a, int b, int[] coSort)
    {
        swap(x, a, b);
        swap(coSort, a, b);
    }

    /// <summary>
    /// Swaps x[a] with x[b].
    /// </summary>
    /// <param name="x"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private static void swap(int[] x, int a, int b)
    {
        int t = x[a];
        x[a] = x[b];
        x[b] = t;
    }

    private static void vecswap(int[] x, int a, int b, int n, int[] coSort)
    {
        for (int i = 0; i < n; i++, a++, b++)
            swap(x, a, b, coSort);
    }

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
        return (x[a] < x[b]
            ? (x[b] < x[c] ? b : x[a] < x[c] ? c : a)
            : (x[b] > x[c] ? b : x[a] > x[c] ? c : a));
    }

    public static void quickSort(long[] target, long[] coSort)
    {
        quickSort1(target, 0, target.Length, coSort);
    }

    public static void quickSort(long[] target, int fromIndex, int toIndex, long[] coSort)
    {
        rangeCheck(target.Length, fromIndex, toIndex);
        rangeCheck(coSort.Length, fromIndex, toIndex);
        quickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    public static void quickSort1(long[] target, int fromIndex, int length, long[] coSort)
    {
        throw new NotImplementedException();
    }

    private static void swap(int[] x, int a, int b, long[] coSort)
    {
        swap(x, a, b);
        swap(coSort, a, b);
    }

    /**
         * Swaps x[a] with x[b].
         */
    private static void swap(long[] x, int a, int b)
    {
        (x[a], x[b]) = (x[b], x[a]);
    }

    private static void vecswap(int[] x, int a, int b, int n, long[] coSort)
    {
        for (int i = 0; i < n; i++, a++, b++)
            swap(x, a, b, coSort);
    }

    public static void quickSort<T>(T[] target, object[] coSort)
        where T : IComparable<T>
    {
        quickSort(target, 0, target.Length, coSort);
    }

    public static void quickSort<T>(T[] target, int fromIndex, int toIndex, object[] coSort)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
        //if (ReferenceEquals(target, coSort)) throw new ArgumentException();

        //rangeCheck(target.Length, fromIndex, toIndex);
        //rangeCheck(coSort.Length, fromIndex, toIndex);
        //quickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    /// <summary>
    /// Check that fromIndex and toIndex are in range, and throw an appropriate exception if they aren't.
    /// </summary>
    /// <param name="arrayLen"></param>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    private static void rangeCheck(int arrayLen, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex) throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})");
        if (fromIndex < 0) throw new IndexOutOfRangeException(nameof(fromIndex));
        if (toIndex > arrayLen) throw new IndexOutOfRangeException(nameof(toIndex));
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

    public static void InsertionSort(int[] target, int[] coSort)
    {
        InsertionSort(target, 0, target.Length, coSort);
    }

    public static void InsertionSort(int[] target, int fromIndex, int toIndex, int[] coSort)
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);

        int i, key, j, keyC;
        for (i = fromIndex + 1; i < toIndex; i++)
        {
            key = target[i];
            keyC = coSort[i];
            for (j = i; j > fromIndex && target[j - 1] > key; j--)
            {
                target[j] = target[j - 1];
                coSort[j] = coSort[j - 1];
            }
            target[j] = key;
            coSort[j] = keyC;
        }
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
            TimSort(target, coSort);
        else
            InsertionSort(target, coSort);
    }
}