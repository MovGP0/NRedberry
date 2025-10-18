namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Port of cc.redberry.core.utils.stretces.StretchIterator.
/// </summary>
public class StretchIterator
{
    /// <summary>
    /// Initializes a new iterator over the provided elements.
    /// </summary>
    /// <param name="elements">Source elements.</param>
    /// <param name="provider">Converter to integer stretch values.</param>
    public StretchIterator(object[] elements, IntObjectProvider provider)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns whether another stretch is available.
    /// </summary>
    /// <returns><c>true</c> if another stretch is available.</returns>
    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the next stretch.
    /// </summary>
    /// <returns>The next stretch.</returns>
    public Stretch Next()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Unsupported removal operation.
    /// </summary>
    public void Remove()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates an iterable serving hash-based stretches.
    /// </summary>
    /// <param name="elements">Elements to iterate.</param>
    /// <returns>An enumerable that produces stretches.</returns>
    public static IEnumerable<Stretch> GoHash(object[] elements)
    {
        throw new NotImplementedException();
    }
}
