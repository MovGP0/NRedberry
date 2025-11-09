namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static int BinarySearch(IList<int> list, int key)
    {
        return Array.BinarySearch(list.ToArray(), 0, list.Count, key);
    }

    public static int BinarySearch1(int[] a, int key)
    {
        return BinarySearch1(a, 0, a.Length, key);
    }

    public static int BinarySearch1(int[] a, int fromIndex, int toIndex, int key)
    {
        int low = fromIndex;
        int high = toIndex - 1;

        while (low <= high)
        {
            int mid = (low + high) >> 1;
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
                while (mid > 0 && a[mid - 1] == midVal)
                {
                    --mid;
                }

                return mid;
            }
        }

        if (low >= a.Length)
        {
            return low;
        }

        while (low > 0 && a[low - 1] == a[low])
        {
            --low;
        }

        return low;
    }
}
