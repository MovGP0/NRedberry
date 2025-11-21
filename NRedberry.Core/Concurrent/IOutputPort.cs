namespace NRedberry.Concurrent;

/// <summary>
/// Interface for passive objects provider.
/// </summary>
/// <typeparam name="T"></typeparam>
[Obsolete("replace with IEnumerable<T>.")]
public interface IOutputPort<out T>
    where T : class
{
    T Take();
}
