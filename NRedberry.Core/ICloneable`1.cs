namespace NRedberry;

public interface ICloneable<out T> : ICloneable
{
    public new T Clone();
}
