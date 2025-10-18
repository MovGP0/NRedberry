using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.IteratorWithProgress.
/// </summary>
public class IteratorWithProgress<T> : IEnumerator<T>
{
    protected readonly IEnumerator<T> innerIterator;
    protected readonly long totalCount;
    protected readonly Consumer output = null!;
    protected int previousPercent = -1;
    protected long currentPosition;

    public IteratorWithProgress(IEnumerator<T> innerIterator, long totalCount, Consumer output)
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

    public delegate void Consumer(int value);
}
