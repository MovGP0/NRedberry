namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.EmptyIterator.
/// </summary>
[Obsolete("Use Enumerable.Empty<T>() instead", true)]
public sealed class EmptyIterator<T> : System.Collections.Generic.IEnumerator<T>
{
    public static EmptyIterator<T> Instance { get; } = new();

    private EmptyIterator()
    {
    }

    public T Current => throw new InvalidOperationException("Empty iterator has no current element.");

    object System.Collections.IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        return false;
    }

    public void Reset()
    {
    }

    public void Dispose()
    {
    }
}
