namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
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
}
