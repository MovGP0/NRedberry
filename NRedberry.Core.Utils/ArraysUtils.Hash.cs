namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static int CommutativeHashCode(params object?[]? objects)
    {
        if (objects == null)
        {
            return 0;
        }

        int hash = 0;
        foreach (object? o in objects)
        {
            hash ^= o?.GetHashCode() ?? 0;
        }

        return HashFunctions.JenkinWang32shift(hash);
    }

    public static int CommutativeHashCode(object?[]? objects, int from, int to)
    {
        if (objects == null)
        {
            return 0;
        }

        int hash = 0;
        for (int i = from; i < to; ++i)
        {
            hash ^= objects[i] == null ? 0 : objects[i]?.GetHashCode() ?? 0;
        }

        return HashFunctions.JenkinWang32shift(hash);
    }

    public static bool Equals(int[]? a, int[]? b)
    {
        if (a == b)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        int length = a.Length;
        if (b.Length != length)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }

        return true;
    }
}
