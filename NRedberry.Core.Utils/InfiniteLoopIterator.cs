using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.InfiniteLoopIterator.
/// </summary>
public sealed class InfiniteLoopIterator<T> : IEnumerator<T>
{
    private readonly T[] _items;
    private int _pointer;

    public InfiniteLoopIterator(params T[] items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = items;
        _pointer = 0;
    }

    public bool MoveNext()
    {
        if (_items.Length == 0)
        {
            return false;
        }

        if (_pointer == _items.Length)
        {
            _pointer = 0;
        }

        return true;
    }

    public void Reset() => _pointer = 0;

    public T Current
    {
        get
        {
            if (_items.Length == 0)
            {
                throw new InvalidOperationException("Sequence is empty.");
            }

            return _items[_pointer++];
        }
    }

    object IEnumerator.Current => Current!;

    public void Dispose()
    {
    }
}
