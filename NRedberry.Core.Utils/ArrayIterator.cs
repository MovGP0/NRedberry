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
        throw new NotImplementedException();
    }

    public T Current => throw new NotImplementedException();

    object System.Collections.IEnumerator.Current => throw new NotImplementedException();

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
