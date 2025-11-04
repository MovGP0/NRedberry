namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Port of cc.redberry.core.utils.stretces.IntObjectProvider.
/// </summary>
public interface IIntObjectProvider
{
    /// <summary>
    /// Gets an integer representation of the supplied element.
    /// </summary>
    /// <param name="element">The element to convert.</param>
    /// <returns>The integer representation.</returns>
    int Get(object element) => element.GetHashCode();

    /// <summary>
    /// Default provider that uses <see cref="object.GetHashCode"/>.
    /// </summary>
    static IIntObjectProvider HashProvider(object element) => new IntObjectProvider(element);
}

/// <summary>
/// Default provider that uses <see cref="object.GetHashCode"/>.
/// </summary>
public sealed class IntObjectProvider(object element) : IIntObjectProvider
{
    private object Element { get; } = element;

    public int Get(object element) => Element.GetHashCode();
}
