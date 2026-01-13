using System.Collections;

namespace NRedberry.Core.Combinatorics;

/// <summary>
/// Parent base class for combinatoric iterators.
/// </summary>
public abstract class IntCombinatorialGenerator : IIntCombinatorialGenerator
{
    /// <summary>
    /// Resets the iteration.
    /// </summary>
    public abstract void Reset();

    /// <summary>Returns the reference to the current iteration element.</summary>
    /// <returns>Reference to the current iteration element.</returns>
    public abstract int[] GetReference();

    public abstract bool MoveNext();

    public abstract int[] Current { get; }

    object IEnumerator.Current => Current;

    public abstract void Dispose();

    public IEnumerator<int[]> GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
