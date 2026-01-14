using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

/// <summary>
/// Holder returned by <see cref="AlgorithmsBase.Strip(IReadOnlyList{BSGSElement}, Permutation)"/>.
/// </summary>
public sealed class StripContainer
{
    public StripContainer(int terminationLevel, Permutation remainder)
    {
        TerminationLevel = terminationLevel;
        Remainder = remainder;
    }

    public int TerminationLevel { get; }

    public Permutation Remainder { get; }
}
