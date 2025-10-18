using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.ArrayIterator.
/// </summary>
public sealed class ArrayIterator<T> : IEnumerator<T>
{
    private readonly T[] array = [];
    private int index = -1;

    public ArrayIterator(T[] array)
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
    }
}
