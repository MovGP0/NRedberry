namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static void QuickSort(long[] target, long[] coSort)
    {
        QuickSort1(target, 0, target.Length, coSort);
    }

    public static void QuickSort(long[] target, int fromIndex, int toIndex, long[] coSort)
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        QuickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    public static void QuickSort(int[] target, int[] coSort)
    {
        QuickSort(target, 0, target.Length, coSort);
    }

    public static void QuickSort(int[] target, int fromIndex, int toIndex, int[] coSort)
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        QuickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    public static void QuickSort(int[] target, long[] coSort)
    {
        QuickSort1(target, 0, target.Length, coSort);
    }

    public static void QuickSort(int[] target, int fromIndex, int toIndex, long[] coSort)
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        QuickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    public static void QuickSort<T>(T[] target, object[] coSort) where T : IComparable<T>
    {
        QuickSort(target, 0, target.Length, coSort);
    }

    public static void QuickSort<T>(T[] target, int fromIndex, int toIndex, object[] coSort) where T : IComparable<T>
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        QuickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    public static void QuickSort<T>(T[] target, int[] coSort) where T : IComparable<T>
    {
        QuickSort(target, 0, target.Length, coSort);
    }

    public static void QuickSort<T>(T[] target, int fromIndex, int toIndex, int[] coSort) where T : IComparable<T>
    {
        RangeCheck(target.Length, fromIndex, toIndex);
        RangeCheck(coSort.Length, fromIndex, toIndex);
        QuickSort1(target, fromIndex, toIndex - fromIndex, coSort);
    }

    private static void QuickSort1(long[] target, int fromIndex, int length, long[] coSort)
    {
        if (length < 7)
        {
            for (int i = fromIndex; i < length + fromIndex; i++)
            {
                for (int j = i; j > fromIndex && target[j - 1] > target[j]; j--)
                    Swap(target, j, j - 1, coSort);
            }

            return;
        }

        int m = fromIndex + (length >> 1);
        if (length > 7)
        {
            int l = fromIndex;
            int n1 = fromIndex + length - 1;
            if (length > 40)
            {
                int s1 = length / 8;
                l = Med3(target, l, l + s1, l + 2 * s1);
                m = Med3(target, m - s1, m, m + s1);
                n1 = Med3(target, n1 - 2 * s1, n1 - s1, n1);
            }

            m = Med3(target, l, m, n1);
        }

        long v = target[m];

        int a = fromIndex;
        int b = a;
        int c = fromIndex + length - 1;
        int d = c;
        while (true)
        {
            while (b <= c && target[b] <= v)
            {
                if (target[b] == v)
                    Swap(target, a++, b, coSort);
                b++;
            }

            while (c >= b && target[c] >= v)
            {
                if (target[c] == v)
                    Swap(target, c, d--, coSort);
                c--;
            }

            if (b > c)
                break;
            Swap(target, b++, c--, coSort);
        }

        int s;
        int n = fromIndex + length;
        s = Math.Min(a - fromIndex, b - a);
        Vecswap(target, fromIndex, b - s, s, coSort);
        s = Math.Min(d - c, n - d - 1);
        Vecswap(target, b, n - s, s, coSort);

        if ((s = b - a) > 1)
            QuickSort1(target, fromIndex, s, coSort);
        if ((s = d - c) > 1)
            QuickSort1(target, n - s, s, coSort);
    }

    private static void QuickSort1(int[] target, int fromIndex, int length, int[] coSort)
    {
        if (target == coSort)
            throw new ArgumentException("Target reference == coSort reference.");
        QuickSort2(target, fromIndex, length, coSort);
    }

    private static void QuickSort2(int[] target, int fromIndex, int length, int[] coSort)
    {
        if (length < 7)
        {
            for (int i = fromIndex; i < length + fromIndex; i++)
            {
                for (int j = i; j > fromIndex && target[j - 1] > target[j]; j--)
                    Swap(target, j, j - 1, coSort);
            }

            return;
        }

        int m = fromIndex + (length >> 1);
        if (length > 7)
        {
            int l = fromIndex;
            int n = fromIndex + length - 1;
            if (length > 40)
            {
                int s = length / 8;
                l = Med3(target, l, l + s, l + 2 * s);
                m = Med3(target, m - s, m, m + s);
                n = Med3(target, n - 2 * s, n - s, n);
            }

            m = Med3(target, l, m, n);
        }

        int v = target[m];

        int a = fromIndex;
        int b = a;
        int c = fromIndex + length - 1;
        int d = c;
        while (true)
        {
            while (b <= c && target[b] <= v)
            {
                if (target[b] == v)
                    Swap(target, a++, b, coSort);
                b++;
            }

            while (c >= b && target[c] >= v)
            {
                if (target[c] == v)
                    Swap(target, c, d--, coSort);
                c--;
            }

            if (b > c)
                break;
            Swap(target, b++, c--, coSort);
        }

        int n2 = fromIndex + length;
        var s2 = Math.Min(a - fromIndex, b - a);
        Vecswap(target, fromIndex, b - s2, s2, coSort);
        s2 = Math.Min(d - c, n2 - d - 1);
        Vecswap(target, b, n2 - s2, s2, coSort);

        if ((s2 = b - a) > 1)
            QuickSort2(target, fromIndex, s2, coSort);
        if ((s2 = d - c) > 1)
            QuickSort2(target, n2 - s2, s2, coSort);
    }

    private static void QuickSort1(int[] target, int fromIndex, int length, long[] coSort)
    {
        if (length < 7)
        {
            for (int i = fromIndex; i < length + fromIndex; i++)
            {
                for (int j = i; j > fromIndex && target[j - 1] > target[j]; j--)
                    Swap(target, j, j - 1, coSort);
            }

            return;
        }

        int m = fromIndex + (length >> 1);
        if (length > 7)
        {
            int l = fromIndex;
            int n = fromIndex + length - 1;
            if (length > 40)
            {
                int s = length / 8;
                l = Med3(target, l, l + s, l + 2 * s);
                m = Med3(target, m - s, m, m + s);
                n = Med3(target, n - 2 * s, n - s, n);
            }

            m = Med3(target, l, m, n);
        }

        int v = target[m];

        int a = fromIndex;
        int b = a;
        int c = fromIndex + length - 1;
        int d = c;
        while (true)
        {
            while (b <= c && target[b] <= v)
            {
                if (target[b] == v)
                    Swap(target, a++, b, coSort);
                b++;
            }

            while (c >= b && target[c] >= v)
            {
                if (target[c] == v)
                    Swap(target, c, d--, coSort);
                c--;
            }

            if (b > c)
                break;
            Swap(target, b++, c--, coSort);
        }

        int n1 = fromIndex + length;
        var s1 = Math.Min(a - fromIndex, b - a);
        Vecswap(target, fromIndex, b - s1, s1, coSort);
        s1 = Math.Min(d - c, n1 - d - 1);
        Vecswap(target, b, n1 - s1, s1, coSort);

        if ((s1 = b - a) > 1)
            QuickSort1(target, fromIndex, s1, coSort);
        if ((s1 = d - c) > 1)
            QuickSort1(target, n1 - s1, s1, coSort);
    }

    private static void QuickSort1<T>(T[] target, int fromIndex, int length, object[] coSort) where T : IComparable<T>
    {
        if (length < 7)
        {
            for (int i = fromIndex; i < length + fromIndex; i++)
            {
                for (int j = i; j > fromIndex && target[j - 1].CompareTo(target[j]) > 0; j--)
                    Swap(target, j, j - 1, coSort);
            }

            return;
        }

        int m = fromIndex + (length >> 1);
        if (length > 7)
        {
            int l = fromIndex;
            int n = fromIndex + length - 1;
            if (length > 40)
            {
                int s = length / 8;
                l = Med3(target, l, l + s, l + 2 * s);
                m = Med3(target, m - s, m, m + s);
                n = Med3(target, n - 2 * s, n - s, n);
            }

            m = Med3(target, l, m, n);
        }

        T v = target[m];

        int a = fromIndex;
        int b = a;
        int c = fromIndex + length - 1;
        int d = c;
        while (true)
        {
            while (b <= c && target[b].CompareTo(v) <= 0)
            {
                if (target[b].CompareTo(v) == 0)
                    Swap(target, a++, b, coSort);
                b++;
            }

            while (c >= b && target[c].CompareTo(v) >= 0)
            {
                if (target[c].CompareTo(v) == 0)
                    Swap(target, c, d--, coSort);
                c--;
            }

            if (b > c)
                break;
            Swap(target, b++, c--, coSort);
        }

        var n1 = fromIndex + length;
        var s1 = Math.Min(a - fromIndex, b - a);
        Vecswap(target, fromIndex, b - s1, s1, coSort);
        s1 = Math.Min(d - c, n1 - d - 1);
        Vecswap(target, b, n1 - s1, s1, coSort);

        if ((s1 = b - a) > 1)
            QuickSort1(target, fromIndex, s1, coSort);
        if ((s1 = d - c) > 1)
            QuickSort1(target, n1 - s1, s1, coSort);
    }

    private static void QuickSort1<T>(T[] target, int fromIndex, int length, int[] coSort) where T : IComparable<T>
    {
        if (length < 7)
        {
            for (int i = fromIndex; i < length + fromIndex; i++)
            {
                for (int j = i; j > fromIndex && target[j - 1].CompareTo(target[j]) > 0; j--)
                    Swap(target, j, j - 1, coSort);
            }

            return;
        }

        int m = fromIndex + (length >> 1);
        if (length > 7)
        {
            int l = fromIndex;
            int n = fromIndex + length - 1;
            if (length > 40)
            {
                int s = length / 8;
                l = Med3(target, l, l + s, l + 2 * s);
                m = Med3(target, m - s, m, m + s);
                n = Med3(target, n - 2 * s, n - s, n);
            }

            m = Med3(target, l, m, n);
        }

        T v = target[m];

        int a = fromIndex;
        int b = a;
        int c = fromIndex + length - 1;
        int d = c;
        while (true)
        {
            while (b <= c && target[b].CompareTo(v) <= 0)
            {
                if (target[b].CompareTo(v) == 0)
                    Swap(target, a++, b, coSort);
                b++;
            }

            while (c >= b && target[c].CompareTo(v) >= 0)
            {
                if (target[c].CompareTo(v) == 0)
                    Swap(target, c, d--, coSort);
                c--;
            }

            if (b > c)
                break;
            Swap(target, b++, c--, coSort);
        }

        var n1 = fromIndex + length;
        var s1 = Math.Min(a - fromIndex, b - a);
        Vecswap(target, fromIndex, b - s1, s1, coSort);
        s1 = Math.Min(d - c, n1 - d - 1);
        Vecswap(target, b, n1 - s1, s1, coSort);

        if ((s1 = b - a) > 1)
            QuickSort1(target, fromIndex, s1, coSort);
        if ((s1 = d - c) > 1)
            QuickSort1(target, n1 - s1, s1, coSort);
    }

    public static int[] QuickSortP(int[] target)
    {
        int[] permutation = new int[target.Length];
        for (int i = 1; i < target.Length; ++i)
            permutation[i] = i;
        QuickSort(target, 0, target.Length, permutation);
        return permutation;
    }
}
