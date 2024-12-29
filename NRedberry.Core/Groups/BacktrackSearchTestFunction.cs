using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Test function applied at each level of the search tree. If it is not applicable at some level, it must return true.
/// See Sec. 4.6.2 in [Holt05] for details.
/// </summary>
public interface IBacktrackSearchTestFunction
{
    /// <summary>
    /// Tests a permutation at the specified level. If the test returns false, no further permutations 
    /// with the same partial base image will be scanned in the search tree.
    /// </summary>
    /// <param name="permutation">The permutation to test.</param>
    /// <param name="level">The level in the search tree.</param>
    /// <returns>The result of the test.</returns>
    bool Test(Permutation permutation, int level);
}

/// <summary>
/// A default implementation of IBacktrackSearchTestFunction that always returns true.
/// </summary>
public sealed class TrueBacktrackSearchTestFunction : IBacktrackSearchTestFunction
{
    /// <summary>
    /// Always returns true.
    /// </summary>
    public bool Test(Permutation permutation, int level)
    {
        return true;
    }
}