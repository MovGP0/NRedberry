namespace NRedberry.Core.Combinatorics;

/// <summary>
/// Parent interface for combinatoric iterators
/// </summary>
public interface IIntCombinatorialGenerator : IEnumerable<int[]>, IEnumerator<int[]>
{
    /// <summary>
    /// Resets the iteration.
    /// </summary>
    new void Reset();

    /// <summary>Returns the reference to the current iteration element.</summary>
    /// <returns>Reference to the current iteration element.</returns>
    int[] GetReference();
}
