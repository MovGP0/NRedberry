namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.IntComparator.
/// </summary>
[Obsolete("Use IComparer<int> instead")]
public interface IntComparator
{
    int Compare(int a, int b);
}

/// <summary>
/// Skeleton port of cc.redberry.core.utils.IntComparator.DEFAULT.
/// </summary>
[Obsolete("Use IComparer<int> instead")]
public static class IntComparators
{
    public static IntComparator Default { get; } = new DefaultIntComparator();
}

internal sealed class DefaultIntComparator : IntComparator
{
    public int Compare(int a, int b)
    {
        return a.CompareTo(b);
    }
}
