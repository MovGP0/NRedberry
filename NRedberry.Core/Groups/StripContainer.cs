using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

/// <summary>
/// Holder returned by <see cref="AlgorithmsBase.Strip(IReadOnlyList{BSGSElement}, Permutation)"/>.
/// </summary>
public sealed class StripContainer(int terminationLevel, Permutation remainder)
{
    public int TerminationLevel { get; } = terminationLevel;

    public Permutation Remainder { get; } = remainder;
}
