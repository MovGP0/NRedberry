using System.Diagnostics;
using System.Text;
using NRedberry;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Factor;

/// <summary>
/// Not functional yet.
/// </summary>
public sealed class SingularFactorizationEngine : TransformationToStringAble, IDisposable
{
    private const string Command = ""
        + "short = 0;"
        + "poly p = POLYNOMIAL;"
        + "list f = factorize(p);"
        + "for(int i=1; i<=size(f[1]); i++) { "
        + "    print(\\\"@(\\\" + string(f[1][i]) + \\\")^\\\" + string(f[2][i])); "
        + "}\n"
        + "print(\\\"DONE\\\");";

    private readonly Process process;
    private readonly System.IO.StreamReader reader;
    private readonly System.IO.StreamWriter writer;
    private bool disposed;

    public SingularFactorizationEngine(string singularBin)
    {
        ArgumentNullException.ThrowIfNull(singularBin);

        var startInfo = new ProcessStartInfo
        {
            FileName = singularBin,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start Singular process.");
        reader = process.StandardOutput;
        writer = process.StandardInput;
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        var variables = TensorUtils.GetAllDiffSimpleTensors(tensor);
        var vars = variables.Count == 0 ? new SimpleTensor[1] : new SimpleTensor[variables.Count];
        if (variables.Count > 0)
        {
            int position = 0;
            foreach (SimpleTensor variable in variables)
            {
                vars[position++] = variable;
            }
        }

        writer.WriteLine(CreateRing(vars));
        writer.Flush();
        writer.WriteLine(Command.Replace("POLYNOMIAL", tensor.ToString(OutputFormat.WolframMathematica), StringComparison.Ordinal));
        writer.Flush();

        var factors = new List<Tensor>();
        try
        {
            string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                if (line.StartsWith("//", StringComparison.Ordinal))
                {
                    continue;
                }

                if (line.Equals("DONE", StringComparison.Ordinal))
                {
                    break;
                }

                if (line.StartsWith("@", StringComparison.Ordinal))
                {
                    factors.Add(Tensors.Tensors.Parse(line.Substring(1).Replace("^", "**", StringComparison.Ordinal)));
                }
            }
        }
        catch (System.IO.IOException exception)
        {
            throw new InvalidOperationException("Error reading from Singular process.", exception);
        }

        return Tensors.Tensors.Multiply(factors);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Close()
    {
        Dispose();
    }

    private void Dispose(bool disposing)
    {
        if (!disposing || disposed)
        {
            return;
        }

        disposed = true;
        writer.Dispose();
        reader.Dispose();

        if (!process.HasExited)
        {
            process.Kill(true);
            process.WaitForExit();
        }

        process.Dispose();
    }

    private static string CreateRing(Tensor[] vars)
    {
        var sb = new StringBuilder();
        sb.Append("ring r = 0,(");
        for (int i = 0; ; i++)
        {
            sb.Append(vars[i]?.ToString() ?? "null");
            if (i == vars.Length - 1)
            {
                break;
            }

            sb.Append(',');
        }

        sb.Append("),dp;");
        return sb.ToString();
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "SingularFactorization";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
