namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static void Fill(IList<int> list, int fromIndex, int toIndex, int value)
    {
        if (toIndex >= list.Count)
        {
            throw new IndexOutOfRangeException();
        }

        Array.Fill(list.ToArray(), value, fromIndex, toIndex - fromIndex);
    }

    public static void Fill(IList<int> list, int value)
    {
        Fill(list, 0, list.Count, value);
    }
}
