using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.InfiniteLoopIterable.
/// </summary>
public sealed class InfiniteLoopIterable<T> : IEnumerable<T>
{
    private readonly IReadOnlyList<T> items = [];

    public InfiniteLoopIterable(IReadOnlyList<T> items)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
