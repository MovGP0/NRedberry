using System.Collections;

namespace NRedberry.Core.Utils;

[Obsolete("Consider using Enumerable.Return(element) instead.")]
public sealed class SingleIterator<T>(T element) : IEnumerator<T>
{
    private int _state;

    public T Current
    {
        get
        {
            if (_state != 1)
            {
                throw new InvalidOperationException("Enumeration has not started or has already finished.");
            }

            return element;
        }
    }

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_state == 0)
        {
            _state = 1;
            return true;
        }

        _state = 2;
        return false;
    }

    public void Reset()
    {
        _state = 0;
    }

    public void Dispose()
    {
        // No resources to release
    }
}
