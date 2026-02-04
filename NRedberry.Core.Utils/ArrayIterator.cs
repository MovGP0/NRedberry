namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.ArrayIterator.
/// </summary>
[Obsolete("Arrays can be enumerated directly in C#", true)]
public sealed class ArrayIterator<T> : System.Collections.Generic.IEnumerator<T>
{
    private readonly T[] _array;
    private int _index = -1;

    public ArrayIterator(T[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        _array = array;
    }

    public T Current
    {
        get
        {
            if (_index < 0 || _index >= _array.Length)
            {
                throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element.");
            }

            return _array[_index];
        }
    }

    object System.Collections.IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        if (_index < _array.Length - 1)
        {
            _index++;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _index = -1;
    }

    public void Dispose()
    {
    }
}
