namespace NRedberry.Core.Utils;

public static class ListExtensions
{
    public static List<int> Clone(this List<int> list) => new(list);

    public static void RemoveAfter(this List<int> list, int point)
    {
        if (point < 0 || point > list.Count)
            throw new IndexOutOfRangeException(nameof(point));

        // remove elements with index > point
        if (point < list.Count - 1)
        {
            int removeCount = list.Count - point - 1;
            list.RemoveRange(point + 1, removeCount);
        }
    }
}
