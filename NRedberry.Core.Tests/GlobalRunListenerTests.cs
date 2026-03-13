using System;
using System.IO;
using NRedberry.Contexts;
using NRedberry.Tensors.Functions;
using Xunit;

namespace NRedberry.Core.Tests;

public sealed class GlobalRunListenerTests
{
    [Fact]
    public void ShouldInitializeNewContextWhenTestStarts()
    {
        ContextSettings settings = new()
        {
            NameManagerSeed = 123
        };
        NRedberry.Contexts.Context original = ContextManager.InitializeNew(settings);
        GlobalRunListener listener = new();

        listener.TestStarted("core");

        Assert.NotSame(original, ContextManager.GetCurrentContext());
    }

    [Fact]
    public void ShouldWriteFailureMessageIncludingSeed()
    {
        ContextSettings settings = new()
        {
            NameManagerSeed = 321
        };
        ContextManager.InitializeNew(settings);
        GlobalRunListener listener = new();
        StringWriter writer = new();
        TextWriter original = Console.Out;

        try
        {
            Console.SetOut(writer);
            listener.TestFailure("sample.test");
        }
        finally
        {
            Console.SetOut(original);
        }

        string output = writer.ToString();
        Assert.Contains("sample.test", output, StringComparison.Ordinal);
        Assert.Contains("321", output, StringComparison.Ordinal);
    }
}
