using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using NRedberry.Contexts;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups.Permutations;

public sealed class GapGroupsInterface : IDisposable
{
    private readonly Process _gapProcess;
    private readonly Thread _readThread;
    private readonly StreamWriter _gapCmd;
    private readonly GapOutputReader _gapReader;
    private int _local;

    public GapGroupsInterface(string gapExecutable)
    {
        ArgumentNullException.ThrowIfNull(gapExecutable);

        ProcessStartInfo startInfo = new()
        {
            FileName = gapExecutable,
            Arguments = "-b",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        _gapProcess = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start GAP process.");
        _gapCmd = _gapProcess.StandardInput;
        _gapReader = new GapOutputReader(_gapProcess.StandardOutput.BaseStream);
        _readThread = new Thread(_gapReader.Run)
        {
            IsBackground = true
        };
        _readThread.Start();
        Evaluate($"Reset(GlobalMersenneTwister, {CC.NameManager.GetSeed()});");
    }

    public string Evaluate(string command)
    {
        ArgumentNullException.ThrowIfNull(command);

        _gapCmd.WriteLine(NormalizeCommandForGap(command) + "Print(\"\\nEOF\");");
        _gapCmd.Flush();

        if (!_gapReader.Buffer.TryTake(out string? result, TimeSpan.FromSeconds(30)))
        {
            throw new TimeoutException("Timeout.");
        }

        return result;
    }

    public string EvaluateRedberryGroup(string var, IReadOnlyList<Permutation> generators)
    {
        ArgumentNullException.ThrowIfNull(var);
        ArgumentNullException.ThrowIfNull(generators);

        if (generators.Count == 0)
        {
            return Evaluate(var.Trim() + ":= Group(());");
        }

        StringBuilder builder = new();
        builder.Append(var.Trim()).Append(":= Group(");
        for (int i = 0; i < generators.Count; ++i)
        {
            builder.Append("PermList(").Append(ConvertToGapList(generators[i].OneLine())).Append(")");
            if (i < generators.Count - 1)
            {
                builder.Append(", ");
            }
        }

        builder.Append(");");
        return Evaluate(builder.ToString());
    }

    public PermutationGroup EvaluateToPermutationGroup(string command)
    {
        return PermutationGroup.CreatePermutationGroup(EvaluateToGenerators(NormalizeCommandForGap(command)));
    }

    public bool EvaluateToBoolean(string command)
    {
        return bool.Parse(Evaluate(NormalizeCommandForGap(command)));
    }

    public int EvaluateToInteger(string command)
    {
        return int.Parse(Evaluate(NormalizeCommandForGap(command)));
    }

    public BigInteger EvaluateToBigInteger(string command)
    {
        return BigInteger.Parse(Evaluate(NormalizeCommandForGap(command)));
    }

    public Permutation[] EvaluateToGenerators(string command)
    {
        string normalized = NormalizeCommandFromGap(command);
        if (!EvaluateToBoolean($"IsPermGroup({normalized});"))
        {
            throw new ArgumentException("Specified string does not denote any GAP permutation group.", nameof(command));
        }

        StringBuilder gapCommandBuilder = new();
        gapCommandBuilder.Append(" g:= ").Append(normalized).Append(";;");
        gapCommandBuilder.Append(" degree:= Length(MovedPoints(g));;");
        gapCommandBuilder.Append(" generators:= GeneratorsOfGroup(g);;");
        gapCommandBuilder.Append(" index:= 1;;");
        gapCommandBuilder.Append(" while(index <= Length(generators)) do ");
        gapCommandBuilder.Append("     Print(ListPerm(generators[index], degree));");
        gapCommandBuilder.Append("     if index < Length(generators) then");
        gapCommandBuilder.Append("         Print(\", \");");
        gapCommandBuilder.Append("     fi;;");
        gapCommandBuilder.Append("     index:= index + 1;;");
        gapCommandBuilder.Append(" od;;");
        string gapCommand = gapCommandBuilder.ToString();

        string generatorsString = Evaluate(gapCommand)
            .Replace("\n", string.Empty, StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal);

        string[] generatorStrings = generatorsString.Split("],[", StringSplitOptions.None);
        Permutation[] generators = new Permutation[generatorStrings.Length];
        for (int i = 0; i < generatorStrings.Length; ++i)
        {
            string generator = generatorStrings[i];
            if (generator.StartsWith("[", StringComparison.Ordinal))
            {
                generator = generator[1..];
            }

            if (generator.EndsWith("]", StringComparison.Ordinal))
            {
                generator = generator[..^1];
            }

            string[] integers = generator.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int[] permutation = new int[integers.Length];
            for (int j = 0; j < integers.Length; ++j)
            {
                permutation[j] = int.Parse(integers[j]) - 1;
            }

            generators[i] = GroupPermutations.CreatePermutation(permutation);
        }

        return generators;
    }

    public int NrPrimitiveGroups(int degree)
    {
        return EvaluateToInteger("NrPrimitiveGroups(" + degree + ");");
    }

    public Permutation[] PrimitiveGenerators(int degree, int nr)
    {
        return EvaluateToGenerators("PrimitiveGroup(" + degree + "," + (nr + 1) + ");");
    }

    public PermutationGroup PrimitiveGroup(int degree, int nr)
    {
        string variable = NextVar();
        Evaluate(variable + ":= PrimitiveGroup(" + degree + "," + (nr + 1) + ");");
        if (EvaluateToBoolean("IsNaturalSymmetricGroup(" + variable + ");"))
        {
            return PermutationGroup.SymmetricGroup(degree);
        }

        if (EvaluateToBoolean("IsNaturalAlternatingGroup(" + variable + ");"))
        {
            return PermutationGroup.AlternatingGroup(degree);
        }

        return EvaluateToPermutationGroup(variable);
    }

    public void Close()
    {
        _gapCmd.Close();
        if (!_gapProcess.HasExited)
        {
            _gapProcess.WaitForExit();
        }

        _readThread.Join();
    }

    public static string ConvertToGapList(int[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (array.Length == 0)
        {
            return "[]";
        }

        StringBuilder builder = new("[");
        for (int i = 0; i < array.Length; ++i)
        {
            builder.Append(array[i] + 1);
            if (i < array.Length - 1)
            {
                builder.Append(", ");
            }
        }

        builder.Append(']');
        return builder.ToString();
    }

    internal static string NormalizeCommandForGap(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string normalized = value.Trim();
        return normalized.EndsWith(';') ? normalized : normalized + ";";
    }

    internal static string NormalizeCommandFromGap(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string normalized = value.Trim();
        return normalized.EndsWith(';')
            ? normalized[..^1]
            : normalized;
    }

    public void Dispose()
    {
        Close();
        GC.SuppressFinalize(this);
    }

    private string NextVar()
    {
        if (_local < 0)
        {
            _local = 0;
        }

        return "var" + _local++;
    }
}

internal sealed class GapOutputReader
{
    private readonly StreamReader _reader;

    public GapOutputReader(Stream inputStream)
    {
        ArgumentNullException.ThrowIfNull(inputStream);
        _reader = new StreamReader(inputStream);
    }

    public BlockingCollection<string> Buffer { get; } = new(128);

    public void Run()
    {
        StringBuilder builder = new();
        string? line;
        while ((line = _reader.ReadLine()) is not null)
        {
            line = line.Replace("gap>", string.Empty, StringComparison.Ordinal).Trim();
            if (line.Length > 0 && line[^1] == '\\')
            {
                line = line[..^1];
            }

            if (line == "EOF")
            {
                Buffer.Add(builder.ToString());
                builder.Clear();
                continue;
            }

            builder.Append(line);
        }
    }
}
