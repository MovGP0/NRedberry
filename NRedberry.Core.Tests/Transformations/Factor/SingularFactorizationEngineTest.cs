using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Factor;
using Shouldly;
using Xunit;
using Xunit.Sdk;
using RandomTensorGenerator = NRedberry.Tensors.Random.RandomTensor;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class SingularFactorizationEngineTest
{
    [Fact]
    public void ShouldFactorSimplePolynomial()
    {
        using SingularFactorizationEngine engine = new(GetSingularExecutablePath());
        TensorType tensor = TensorFactory.Parse("12387623*x**134-12387623*y**6");

        TensorType factored = engine.Transform(tensor);
        bool equalAfterExpansion = TensorUtils.Equals(
            ExpandTransformation.Expand(factored),
            ExpandTransformation.Expand(tensor));

        equalAfterExpansion.ShouldBeTrue();
    }

    [Fact]
    public void ShouldFactorRandomTensor()
    {
        RandomTensorGenerator random = new();
        random.ClearNamespace();
        random.AddToNamespace(
            TensorFactory.Parse("x"),
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"),
            TensorFactory.Parse("t"));

        using SingularFactorizationEngine engine = new(GetSingularExecutablePath());
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

        throw SkipException.ForSkip("Singular executable is not available on PATH.");
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
