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

public static class ToStringConverters
{
    public static readonly IToStringConverter<int> Default = new DefaultToStringConverter();
    public static readonly IToStringConverter<int> Hex = new HexToStringConverter();
    public static readonly IToStringConverter<int> Binary = new BinaryToStringConverter();
}
