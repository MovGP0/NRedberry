namespace NRedberry.Core.Utils;

public interface IIndicator<in E>
{
    bool Is(E @object);
}