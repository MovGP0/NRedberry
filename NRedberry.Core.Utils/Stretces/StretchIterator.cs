namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Port of cc.redberry.core.utils.stretces.StretchIterator.
/// </summary>
public class StretchIterator
{
    private readonly object[] _elements;
    private readonly IIntObjectProvider _provider;
    private int _pointer;

    /// <summary>
    /// Initializes a new iterator over the provided elements.
    /// </summary>
    /// <param name="elements">Source elements.</param>
    /// <param name="provider">Converter to integer stretch values.</param>
    public StretchIterator(object[] elements, IIntObjectProvider provider)
    {
        ArgumentNullException.ThrowIfNull(elements);
        ArgumentNullException.ThrowIfNull(provider);
        _elements = elements;
        _provider = provider;
    }

    /// <summary>
    /// Returns whether another stretch is available.
    /// </summary>
    /// <returns><c>true</c> if another stretch is available.</returns>
    public bool HasNext()
    {
        return _pointer < _elements.Length;
    }

    /// <summary>
    /// Returns the next stretch.
    /// </summary>
    /// <returns>The next stretch.</returns>
    public Stretch Next()
    {
        int i = _pointer;
        int value = _provider.Get(_elements[i]);
        int begin = i;
        while (++i < _elements.Length && _provider.Get(_elements[i]) == value)
        {
        }

        _pointer = i;
        return new Stretch(begin, i - begin);
    }

    /// <summary>
    /// Unsupported removal operation.
    /// </summary>
    public void Remove()
    {
        throw new NotSupportedException("Remove is not supported.");
    }

    /// <summary>
    /// Creates an iterable serving hash-based stretches.
    /// </summary>
    /// <param name="elements">Elements to iterate.</param>
    /// <returns>An enumerable that produces stretches.</returns>
    public static IEnumerable<Stretch> GoHash(object[] elements)
    {
        return Enumerate();

        IEnumerable<Stretch> Enumerate()
        {
            var iterator = new StretchIterator(elements, IIntObjectProvider.HashProvider);
            while (iterator.HasNext())
            {
                yield return iterator.Next();
            }
        }
    }
}
