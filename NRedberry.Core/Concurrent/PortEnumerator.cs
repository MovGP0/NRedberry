using System.Collections;

namespace NRedberry.Concurrent;

[Obsolete("replace with IEnumerable<T>.")]
public sealed class PortEnumerator<T>(IOutputPortUnsafe<T> opu) : IEnumerator<T>
    where T : class
{
    private IOutputPortUnsafe<T> Opu { get; } = opu;

    public bool MoveNext()
    {
        Current = Opu.Take();
        return Current != null;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public T Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}
