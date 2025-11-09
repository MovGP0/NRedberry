namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static int[]? Bijection<T>(T[] from, T[] to, IComparer<T> comparator)
    {
        if (from.Length != to.Length)
        {
            return null;
        }

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
            {
                return bijection;
            }
        }

        return null;
    }

    public static int[]? Bijection<T>(T[] from, T[] to) where T : IComparable<T>
    {
        if (from.Length != to.Length)
        {
            return null;
        }

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
            {
                return bijection;
            }
        }

        return null;
    }
}
