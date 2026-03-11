using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit.Sdk;

namespace NRedberry.Core.Tests.Groups.Permutations;

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.GapGroupsInterface.
/// </summary>
public sealed class GapGroupsInterface
{
    private readonly object _gapProcess;
    private readonly object _readThread;
    private readonly object _gapCmd;
    private readonly GapOutputReader _gapReader;
    private int _local;

    public GapGroupsInterface(string gapExecutable)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public string Evaluate(string command)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public string EvaluateRedberryGroup(string var, IReadOnlyList<Permutation> generators)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public PermutationGroup EvaluateToPermutationGroup(string command)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public bool EvaluateToBoolean(string command)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public int EvaluateToInteger(string command)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public System.Numerics.BigInteger EvaluateToBigInteger(string command)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public Permutation[] EvaluateToGenerators(string command)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public int NrPrimitiveGroups(int degree)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public Permutation[] PrimitiveGenerators(int degree, int nr)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public PermutationGroup PrimitiveGroup(int degree, int nr)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public void Close()
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public static string ConvertToGapList(int[] array)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    private string NextVar()
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    private static string StringToGapCommand(string value)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    private static string StringFromGapCommand(string value)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }
}

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.GapGroupsInterface.GapOutputReader.
/// </summary>
internal sealed class GapOutputReader
{
    public GapOutputReader(System.IO.Stream inputStream)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public void Run()
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }
}
