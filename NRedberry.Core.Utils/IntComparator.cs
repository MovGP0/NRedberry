namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.IntComparator.
/// </summary>
[Obsolete("Use IComparer<int> instead", true)]
public interface IntComparator
{
    int Compare(int a, int b);
}

/// <summary>
/// Skeleton port of cc.redberry.core.utils.IntComparator.DEFAULT.
/// </summary>
[Obsolete("Use IComparer<int> instead", true)]
public static class IntComparators
{
    public static IntComparator Default => throw new NotImplementedException();
}
