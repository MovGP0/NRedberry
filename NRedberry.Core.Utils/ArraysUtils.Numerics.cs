namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static int Max(int[] array)
    {
        int a = -1;
        foreach (int i in array)
        {
            a = Math.Max(a, i);
        }

        return a;
    }

    public static int[] GetSeriesFrom0(int size)
    {
        int[] ret = new int[size];
        for (int i = size - 1; i >= 0; --i)
        {
            ret[i] = i;
        }

        return ret;
    }

    public static int[][] DeepClone(int[][] input)
    {
        int[][] res = new int[input.Length][];
        for (int i = res.Length - 1; i >= 0; --i)
        {
            res[i] = (int[])input[i].Clone();
        }

        return res;
    }

    public static int Sum(int[] array)
    {
        int s = 0;
        foreach (int i in array)
        {
            s += i;
        }

        return s;
    }
}
