using System;

namespace NRedberry.Core.Concurrent;

[Obsolete("replace with IEnumerable<T>.")]
public sealed class OutputPortUnsaveSingleton<T> : IOutputPortUnsave<T>
    where T:class
{
    private T _element;

    public OutputPortUnsaveSingleton(T element)
    {
        _element = element;
    }

    public T Take()
    {
        var newElement = _element;
        _element = null;
        return newElement;
    }
}