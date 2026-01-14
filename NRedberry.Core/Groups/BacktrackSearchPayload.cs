using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

/// <summary>
/// Abstract class that implements the backtracking search payload and test function logic.
/// </summary>
public abstract class BacktrackSearchPayload : IBacktrackSearchTestFunction
{
    protected Permutation[] WordReference { get; private set; } = [];

    /// <summary>
    /// Sets the word reference.
    /// </summary>
    /// <param name="wordReference">The word reference to set.</param>
    public void SetWordReference(Permutation[] wordReference)
    {
        WordReference = wordReference;
    }

    /// <summary>
    /// Executes logic before the level is incremented.
    /// </summary>
    /// <param name="level">The current level.</param>
    public abstract void BeforeLevelIncrement(int level);

    /// <summary>
    /// Executes logic after the level is incremented.
    /// </summary>
    /// <param name="level">The current level.</param>
    public abstract void AfterLevelIncrement(int level);

    /// <summary>
    /// Creates a default payload with a specified test function.
    /// </summary>
    /// <param name="test">The test function.</param>
    /// <returns>A new instance of the default payload.</returns>
    public static BacktrackSearchPayload CreateDefaultPayload(IBacktrackSearchTestFunction test)
    {
        return new DefaultBacktrackSearchPayload(test);
    }

    public abstract bool Test(Permutation permutation, int level);
}
