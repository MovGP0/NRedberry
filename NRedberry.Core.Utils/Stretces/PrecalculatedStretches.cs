using System.Collections;

namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Port of cc.redberry.core.utils.stretces.PrecalculatedStretches.
/// </summary>
public class PrecalculatedStretches : IEnumerable<Stretch>
{
    private readonly int[] _values;

    /// <summary>
    /// Initializes a new instance from raw integer values.
    /// </summary>
    /// <param name="values">Precomputed values.</param>
    public PrecalculatedStretches(params int[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = values;
    }

    /// <summary>
    /// Initializes a new instance from arbitrary elements using a provider.
    /// </summary>
    /// <param name="elements">Elements to convert.</param>
    /// <param name="provider">Converter from element to integer.</param>
    public PrecalculatedStretches(object[] elements, IIntObjectProvider provider)
    {
        ArgumentNullException.ThrowIfNull(elements);
        ArgumentNullException.ThrowIfNull(provider);
        _values = new int[elements.Length];
        for (int i = 0; i < elements.Length; ++i)
        {
            _values[i] = provider.Get(elements[i]);
        }
    }

    /// <summary>
    /// Gets the raw pre-calculated values.
    /// </summary>
    /// <returns>The raw values.</returns>
    public int[] RawValues => [.._values];

    /// <summary>
    /// Returns an enumerator over stretch instances.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<Stretch> GetEnumerator() => new StretchIteratorI(_values);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
