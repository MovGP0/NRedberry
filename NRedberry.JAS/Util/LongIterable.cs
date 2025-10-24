using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// Iterable for Long.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.LongIterable
/// </remarks>
public class LongIterable : IEnumerable<long>
{
    private bool nonNegative = true;
    private long upperBound = long.MaxValue;

    public LongIterable()
    {
    }

    /// <summary>
    /// Set the iteration algorithm to non-negative elements.
    /// </summary>
    public void SetNonNegativeIterator()
    {
        nonNegative = true;
    }

    /// <summary>
    /// Get an iterator over Long.
    /// </summary>
    /// <returns>an iterator.</returns>
    public IEnumerator<long> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
