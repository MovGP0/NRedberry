namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
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
        (x[a], x[b]) = (x[b], x[a]);
    }

    private static void vecswap(int[] x, int a, int b, int n, int[] coSort)
    {
        for (int i = 0; i < n; i++, a++, b++)
            swap(x, a, b, coSort);
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

    private static void Swap(int[] x, int a, int b, int[] coSort)
    {
        Swap(x, a, b);
        Swap(coSort, a, b);
    }

    public static void Swap(int[] x, int a, int b)
    {
        (x[a], x[b]) = (x[b], x[a]);
    }

    private static void Vecswap(int[] x, int a, int b, int n, int[] coSort)
    {
        for (int i = 0; i < n; i++, a++, b++)
            Swap(x, a, b, coSort);
    }

    private static void Swap(long[] x, int a, int b, long[] coSort)
    {
        Swap(x, a, b);
        Swap(coSort, a, b);
    }

    private static void Vecswap(long[] x, int a, int b, int n, long[] coSort)
    {
        for (int i = 0; i < n; i++, a++, b++)
            Swap(x, a, b, coSort);
    }

    private static void Swap(int[] x, int a, int b, long[] coSort)
    {
        Swap(x, a, b);
        Swap(coSort, a, b);
    }

    public static void Swap(long[] x, int a, int b)
    {
        (x[a], x[b]) = (x[b], x[a]);
    }

    private static void Vecswap(int[] x, int a, int b, int n, long[] coSort)
    {
        for (int i = 0; i < n; i++, a++, b++)
            Swap(x, a, b, coSort);
    }

    private static void Swap<T>(T[] x, int a, int b, object[] coSort) where T : IComparable<T>
    {
        Swap(x, a, b);
        Swap(coSort, a, b);
    }

    public static void Swap<T>(T[] x, int a, int b)
    {
        (x[a], x[b]) = (x[b], x[a]);
    }

    private static void Vecswap<T>(T[] x, int a, int b, int n, object[] coSort) where T : IComparable<T>
    {
        for (int i = 0; i < n; i++, a++, b++)
            Swap(x, a, b, coSort);
    }

    private static void Swap<T>(T[] x, int a, int b, int[] coSort) where T : IComparable<T>
    {
        Swap(x, a, b);
        Swap(coSort, a, b);
    }

    private static void Vecswap<T>(T[] x, int a, int b, int n, int[] coSort) where T : IComparable<T>
    {
        for (int i = 0; i < n; i++, a++, b++)
            Swap(x, a, b, coSort);
    }
}
