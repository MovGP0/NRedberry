using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.InfiniteLoopIterable.
/// </summary>
public sealed class InfiniteLoopIterable<T> : IEnumerable<T>
{
    private readonly IReadOnlyList<T> _items;

    public InfiniteLoopIterable(params IReadOnlyList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = items;
    }

    public IEnumerator<T> GetEnumerator() => new InfiniteLoopIterator<T>(_items.ToArray());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
