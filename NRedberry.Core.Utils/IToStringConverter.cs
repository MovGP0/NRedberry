namespace NRedberry.Core.Utils;

/// <summary>
/// Base interface for type to string converter.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <see cref="DefaultToStringConverter"/>
/// <see cref="HexToStringConverter"/>
/// <see cref="BinaryToStringConverter"/>
public interface IToStringConverter<in T>
{
    string ToString(T t);
}
