namespace NRedberry.Core.Utils;

public interface IIndicator<in E>
{
    bool Is(E @object);
}

public sealed class TrueIndicator<T> : IIndicator<T>
{
    public bool Is(T @object)
    {
        return true;
    }
}

public sealed class FalseIndicator<T> : IIndicator<T>
{
    public bool Is(T @object)
    {
        return true;
    }
}