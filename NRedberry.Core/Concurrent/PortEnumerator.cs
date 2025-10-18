using System.Collections;

namespace NRedberry.Core.Concurrent;

[Obsolete("replace with IEnumerable<T>.")]
public sealed class PortEnumerator<T> : IEnumerator<T>
    where T : class
{
    private IOutputPortUnsafe<T> Opu { get; }

    public PortEnumerator(IOutputPortUnsafe<T> opu)
    {
        Opu = opu;
    }

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