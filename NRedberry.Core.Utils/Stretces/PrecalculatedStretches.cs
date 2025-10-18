using System.Collections;

namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Port of cc.redberry.core.utils.stretces.PrecalculatedStretches.
/// </summary>
public class PrecalculatedStretches : IEnumerable<Stretch>
{
    /// <summary>
    /// Initializes a new instance from raw integer values.
    /// </summary>
    /// <param name="values">Precomputed values.</param>
    public PrecalculatedStretches(int[] values)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance from arbitrary elements using a provider.
    /// </summary>
    /// <param name="elements">Elements to convert.</param>
    /// <param name="provider">Converter from element to integer.</param>
    public PrecalculatedStretches(object[] elements, IntObjectProvider provider)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the raw pre-calculated values.
    /// </summary>
    /// <returns>The raw values.</returns>
    public int[] GetRawValues()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns an enumerator over stretch instances.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<Stretch> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
