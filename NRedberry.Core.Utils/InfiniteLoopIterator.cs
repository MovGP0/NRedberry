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
        _pointer = -1;
    }

    public bool MoveNext()
    {
        if (_items.Length == 0)
        {
            return false;
        }

        if (_pointer >= _items.Length - 1)
        {
            _pointer = 0;
        }
        else
        {
            _pointer++;
        }

        return true;
    }

    public void Reset() => _pointer = -1;

    public T Current
    {
        get
        {
            if (_items.Length == 0)
            {
                throw new InvalidOperationException("Sequence is empty.");
            }

            if (_pointer < 0 || _pointer >= _items.Length)
            {
                throw new InvalidOperationException("Enumerator is positioned before the first element.");
            }

            return _items[_pointer];
        }
    }

    object IEnumerator.Current => Current!;

    public void Dispose()
    {
    }
}
