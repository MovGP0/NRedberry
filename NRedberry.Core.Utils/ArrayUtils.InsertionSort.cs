namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static void InsertionSort(int[] target, int[] coSort)
    {
        InsertionSort(target, 0, target.Length, coSort);
    }

    public static void InsertionSort(int[] target, int fromIndex, int toIndex, int[] coSort)
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        for (int i = fromIndex + 1; i < toIndex; i++)
        {
            int key = target[i];
            int keyC = coSort[i];
            int j;
            for (j = i; j > fromIndex && target[j - 1] > key; j--)
            {
                target[j] = target[j - 1];
                coSort[j] = coSort[j - 1];
            }

            target[j] = key;
            coSort[j] = keyC;
        }
    }

    public static void InsertionSort(int[] target, long[] coSort)
    {
        InsertionSort(target, 0, target.Length, coSort);
    }

    public static void InsertionSort(int[] target, int fromIndex, int toIndex, long[] coSort)
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        for (int i = fromIndex + 1; i < toIndex; i++)
        {
            int key = target[i];
            long keyC = coSort[i];
            int j;
            for (j = i; j > fromIndex && target[j - 1] > key; j--)
            {
                target[j] = target[j - 1];
                coSort[j] = coSort[j - 1];
            }

            target[j] = key;
            coSort[j] = keyC;
        }
    }

    public static void InsertionSort<T>(T[] target, object[] coSort) where T : IComparable<T>
    {
        InsertionSort(target, 0, target.Length, coSort);
    }

    public static void InsertionSort<T>(T[] target, int fromIndex, int toIndex, object[] coSort) where T : IComparable<T>
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        for (int i = fromIndex + 1; i < toIndex; i++)
        {
            T key = target[i];
            object keyC = coSort[i];
            int j;
            for (j = i; j > fromIndex && target[j - 1].CompareTo(key) > 0; j--)
            {
                target[j] = target[j - 1];
                coSort[j] = coSort[j - 1];
            }

            target[j] = key;
            coSort[j] = keyC;
        }
    }

    public static void InsertionSort<T>(T[] target, int[] coSort) where T : IComparable<T>
    {
        InsertionSort(target, 0, target.Length, coSort);
    }

    public static void InsertionSort<T>(T[] target, int fromIndex, int toIndex, int[] coSort) where T : IComparable<T>
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        for (int i = fromIndex + 1; i < toIndex; i++)
        {
            T key = target[i];
            int keyC = coSort[i];
            int j;
            for (j = i; j > fromIndex && target[j - 1].CompareTo(key) > 0; j--)
            {
                target[j] = target[j - 1];
                coSort[j] = coSort[j - 1];
            }

            target[j] = key;
            coSort[j] = keyC;
        }
    }
}
