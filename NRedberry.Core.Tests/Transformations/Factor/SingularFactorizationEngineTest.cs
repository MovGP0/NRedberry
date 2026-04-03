using System.ComponentModel;
using System.Diagnostics;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Factor;
using RandomTensorGenerator = NRedberry.Tensors.Random.RandomTensor;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class SingularFactorizationEngineTest
{
    [SkippableFact]
    public void ShouldFactorSimplePolynomial()
    {
        string singularExecutablePath = GetSingularExecutablePath();
        using SingularFactorizationEngine engine = new(singularExecutablePath);
        TensorType tensor = TensorFactory.Parse("12387623*x**134-12387623*y**6");

        TensorType factored = engine.Transform(tensor);
        bool equalAfterExpansion = TensorUtils.Equals(
            ExpandTransformation.Expand(factored),
            ExpandTransformation.Expand(tensor));

        equalAfterExpansion.ShouldBeTrue();
    }

    [SkippableFact]
    public void ShouldFactorRandomTensor()
    {
        RandomTensorGenerator random = new();
        random.ClearNamespace();
        random.AddToNamespace(
            TensorFactory.Parse("x"),
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"),
            TensorFactory.Parse("t"));

        string singularExecutablePath = GetSingularExecutablePath();
        using SingularFactorizationEngine engine = new(singularExecutablePath);
        TensorType tensor = random.NextProductTree(3, 6, 8, IndicesFactory.EmptyIndices);

        tensor = ExpandTransformation.Expand(tensor);
        TensorType factored = engine.Transform(tensor);

        TensorUtils.Equals(tensor, ExpandTransformation.Expand(factored)).ShouldBeTrue();
    }

    private static string GetSingularExecutablePath()
    {
        string[] candidates =
        [
            "Singular",
            "singular",
            "Singular.exe",
            "singular.exe",
        ];

        foreach (string candidate in candidates)
        {
            if (IsExecutableAvailable(candidate))
            {
                return candidate;
            }
        }

        Skip.If(true, "Singular executable is not available on PATH.");
        return string.Empty;
    }

    private static bool IsExecutableAvailable(string executableName)
    {
        try
        {
            using Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = executableName,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (!process.Start())
            {
                return false;
            }

            if (!process.WaitForExit(2000))
            {
                process.Kill(true);
                process.WaitForExit();
            }

            return true;
        }
        catch (Win32Exception)
        {
            return false;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
    }
}
