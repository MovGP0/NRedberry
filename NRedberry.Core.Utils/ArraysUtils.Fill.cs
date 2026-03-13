namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static void Fill(IList<int> list, int fromIndex, int toIndex, int value)
    {
        if (fromIndex < 0 || toIndex < fromIndex || toIndex > list.Count)
        {
            throw new IndexOutOfRangeException();
        }

        for (int i = fromIndex; i < toIndex; ++i)
        {
            list[i] = value;
        }
    }

    public static void Fill(IList<int> list, int value)
    {
        Fill(list, 0, list.Count, value);
    }
}
