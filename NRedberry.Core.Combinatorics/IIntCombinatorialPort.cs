namespace NRedberry.Core.Combinatorics;

/// <summary>
/// This interface is common for all combinatorial iterators.
/// </summary>
/// <remarks>
/// src\main\java\cc\redberry\core\combinatorics\IntCombinatorialPort.java
/// </remarks>
public interface IIntCombinatorialPort : IOutputPortUnsafe<int[]>
{
    /// <summary>
    /// Resets the iteration
    /// </summary>
    void Reset();

    /// <summary>Returns the reference to the current iteration element.</summary>
    /// <returns>Reference to the current iteration element</returns>
    int[] GetReference();

    /// <summary>Calculates and returns the next combination or <value>null</value>, if no more combinations exist.</summary>
    /// <returns>The next combination or <value>null</value>, if no more combinations exist</returns>
    int[]? Take();
}