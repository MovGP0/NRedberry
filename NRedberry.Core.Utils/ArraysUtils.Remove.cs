namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static T[] Remove<T>(T[] array, int i)
    {
        T[] r = new T[array.Length - 1];
        Array.Copy(array, 0, r, 0, i);
        if (i < array.Length - 1)
        {
            Array.Copy(array, i + 1, r, i, array.Length - i - 1);
        }

        return r;
    }

    public static T[] Remove<T>(T[] array, int[] positions)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }

        int[] p = MathUtils.GetSortedDistinct(positions);
        if (p.Length == 0)
        {
            return array;
        }

        int size = p.Length;
        int pointer = 0;
        int s = array.Length;
        for (; pointer < size; ++pointer)
        {
            if (p[pointer] >= s)
            {
                throw new IndexOutOfRangeException();
            }
        }

        T[] r = new T[array.Length - p.Length];
        pointer = 0;
        int i = -1;
        for (int j = 0; j < s; ++j)
        {
            if (pointer < size - 1 && j > p[pointer])
            {
                ++pointer;
            }

            if (j == p[pointer])
            {
                continue;
            }

            r[++i] = array[j];
        }

        return r;
    }

    public static int[] Remove(int[] array, int[] positions)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }

        int[] p = MathUtils.GetSortedDistinct(positions);
        if (p.Length == 0)
        {
            return array;
        }

        int size = p.Length;
        int pointer = 0;
        int s = array.Length;
        for (; pointer < size; ++pointer)
        {
            if (p[pointer] >= s)
            {
                throw new IndexOutOfRangeException();
            }
        }

        int[] r = new int[array.Length - p.Length];
        pointer = 0;
        int i = -1;
        for (int j = 0; j < s; ++j)
        {
            if (pointer < size - 1 && j > p[pointer])
            {
                ++pointer;
            }

            if (j == p[pointer])
            {
                continue;
            }

            r[++i] = array[j];
        }

        return r;
    }
}
