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

    /// <summary>
    /// Set the iteration algorithm to non-negative elements.
    /// </summary>
    public void SetNonNegativeIterator() => nonNegative = true;

    /// <summary>
    /// Set the iteration algorithm to include negative and positive elements.
    /// </summary>
    public void SetAllIterator() => nonNegative = false;

    /// <summary>
    /// Set an upper bound (inclusive) for the iterator.
    /// </summary>
    /// <param name="bound">maximum value to iterate to.</param>
    public void SetUpperBound(long bound) => upperBound = bound;

    /// <summary>
    /// Get an iterator over <see cref="long"/>.
    /// </summary>
    /// <returns>an iterator.</returns>
    public IEnumerator<long> GetEnumerator() => new LongEnumerator(nonNegative, upperBound);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class LongEnumerator(bool nonNegative, long upperBound) : IEnumerator<long>
    {
        private long nextValue;
        private long current;
        private bool hasNext = true;

        public long Current => current;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (!hasNext)
            {
                return false;
            }

            current = nextValue;

            long upcoming;
            if (nonNegative)
            {
                upcoming = nextValue + 1;
            }
            else if (nextValue > 0L)
            {
                upcoming = -nextValue;
            }
            else
            {
                upcoming = -nextValue + 1;
            }

            if (upcoming > upperBound)
            {
                hasNext = false;
            }
            else
            {
                nextValue = upcoming;
            }

            return true;
        }

        public void Reset() => throw new NotSupportedException("Reset is not supported.");

        public void Dispose()
        {
        }
    }
}
