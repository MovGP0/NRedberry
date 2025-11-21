namespace NRedberry.Concurrent;

[Obsolete("Replace with an array with only one element: [element]")]
public class Singleton<T>(T element) : IOutputPortUnsafe<T>
    where T : class
{
    private T? _element = element;

    public T? Take()
    {
        var newElement = _element;
        _element = default;
        return newElement;
    }
}
