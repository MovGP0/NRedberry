namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.EmptyIterator.
/// </summary>
[Obsolete("Use Enumerable.Empty<T>() instead", true)]
public sealed class EmptyIterator<T> : System.Collections.Generic.IEnumerator<T>
{
    public static EmptyIterator<T> Instance => throw new NotImplementedException();

    private EmptyIterator()
    {
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
