namespace NRedberry.Concurrent;

[Obsolete("replace with IEnumerable<T>.")]
public interface IOutputPortUnsafe<out T>
    where T : class
{
    T? Take();
}
