using System.Collections;

namespace NRedberry.Core.Utils;

[Obsolete("Consider using Enumerable.Return(element) instead.")]
public sealed class SingleIterator<T>(T element) : IEnumerator<T>
{
    private bool _ended;

    public T Current
    {
        get
        {
            if (_ended)
            {
                throw new InvalidOperationException("Enumeration already finished.");
            }

            return element;
        }
    }

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_ended)
            return false;
        _ended = true;
        return true;
    }

    public void Reset()
    {
        _ended = false;
    }

    public void Dispose()
    {
        // No resources to release
    }
}
