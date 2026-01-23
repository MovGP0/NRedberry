using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

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
        throw new NotImplementedException();
    }

    public string Evaluate(string command)
    {
        throw new NotImplementedException();
    }

    public string EvaluateRedberryGroup(string var, IReadOnlyList<Permutation> generators)
    {
        throw new NotImplementedException();
    }

    public PermutationGroup EvaluateToPermutationGroup(string command)
    {
        throw new NotImplementedException();
    }

    public bool EvaluateToBoolean(string command)
    {
        throw new NotImplementedException();
    }

    public int EvaluateToInteger(string command)
    {
        throw new NotImplementedException();
    }

    public System.Numerics.BigInteger EvaluateToBigInteger(string command)
    {
        throw new NotImplementedException();
    }

    public Permutation[] EvaluateToGenerators(string command)
    {
        throw new NotImplementedException();
    }

    public int NrPrimitiveGroups(int degree)
    {
        throw new NotImplementedException();
    }

    public Permutation[] PrimitiveGenerators(int degree, int nr)
    {
        throw new NotImplementedException();
    }

    public PermutationGroup PrimitiveGroup(int degree, int nr)
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public static string ConvertToGapList(int[] array)
    {
        throw new NotImplementedException();
    }

    private string NextVar()
    {
        throw new NotImplementedException();
    }

    private static string StringToGapCommand(string value)
    {
        throw new NotImplementedException();
    }

    private static string StringFromGapCommand(string value)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.GapGroupsInterface.GapOutputReader.
/// </summary>
internal sealed class GapOutputReader
{
    public GapOutputReader(System.IO.Stream inputStream)
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }
}
