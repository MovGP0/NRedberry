using System;

namespace NRedberry.Core.Concurrent;

[Obsolete("replace with IEnumerable<T>.")]
public interface IOutputPortUnsave<out T>
    where T : class
{
    T Take();
}