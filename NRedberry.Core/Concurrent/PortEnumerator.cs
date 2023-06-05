using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Concurrent;

[Obsolete("replace with IEnumerable<T>.")]
public sealed class PortEnumerator<T> : IEnumerator<T>
    where T : class
{
    private IOutputPortUnsave<T> Opu { get; }

    public PortEnumerator(IOutputPortUnsave<T> opu)
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