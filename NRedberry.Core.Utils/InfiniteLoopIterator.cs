using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.InfiniteLoopIterator.
/// </summary>
public sealed class InfiniteLoopIterator<T> : IEnumerator<T>
{
    private readonly T[] items = [];
    private int pointer;

    public InfiniteLoopIterator(T[] items)
    {
        throw new NotImplementedException();
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
