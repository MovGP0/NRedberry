namespace NRedberry.Core.Utils;

public static class Arrays
{
    public static int[] copyOfRange(int[] src, int start, int end)
    {
        int len = end - start;
        int[] dest = new int[len];
        for (int i = 0; i < len; ++i)
        {
            dest[i] = src[start + i]; // so 0..n = 0+x..n+x
        }

        return dest;
    }

    public static int[] copyOf(int[] original, int newLength)
    {
        return original.Take(newLength).ToArray();
    }

    public static void fill(int[] original, int number)
    {
        for (int i = 0; i < original.Length; ++i)
        {
            original[i] = number;
        }
    }

    public static int BinarySearch(int[] a, int key)
    {
        return BinarySearch0(a, 0, a.Length, key);
    }

    public static int BinarySearch(int[] a, int fromIndex, int toIndex, int key)
    {
        RangeCheck(a.Length, fromIndex, toIndex);
        return BinarySearch0(a, fromIndex, toIndex, key);
    }

    private static void RangeCheck(int arrayLength, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex)
        {
            throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})", nameof(toIndex));
        }

        if (fromIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex));
        }

        if (toIndex > arrayLength)
        {
            throw new ArgumentOutOfRangeException(nameof(toIndex));
        }
    }

    private static int BinarySearch0(int[] a, int fromIndex, int toIndex, int key)
    {
        int low = fromIndex;
        int high = toIndex - 1;

        while (low <= high)
        {
            int mid = (low + high) >>> 1;
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
                return mid; // key found
            }
        }

        return -(low + 1);  // key not found.
    }
}
