using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.EmptyIterator.
/// </summary>
[Obsolete("Use Enumerable.Empty<T>() instead", true)]
public sealed class EmptyIterator<T> : IEnumerator<T>
{
    public static EmptyIterator<T> Instance { get; } = new();

    private EmptyIterator()
    {
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public T Current => throw new NotImplementedException();

    object IEnumerator.Current => Current!;

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
