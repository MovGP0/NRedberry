namespace NRedberry.Core.Concurrent;

public class Singleton<T> : IOutputPortUnsafe<T>
    where T:class
{
    private T? _element;

    public Singleton(T element)
    {
        _element = element;
    }

    public T? Take()
    {
        var newElement = _element;
        _element = default;
        return newElement;
    }
}
