using NRedberry.Contexts;
using NRedberry.Tensors.Functions;

namespace NRedberry.Core.Tests;

public sealed class GlobalRunListener
{
    public void TestStarted(object? description = null)
    {
        ContextManager.InitializeNew();
    }

    public void TestFailure(object failure)
    {
        ArgumentNullException.ThrowIfNull(failure);
        Console.WriteLine($"Test {failure} failed with name manager seed: {CC.NameManager.GetSeed()}");
    }

    public void TestIgnored(object description)
    {
        ArgumentNullException.ThrowIfNull(description);
        Console.WriteLine($"###IGNORED: {description}");
    }

    public void TestAssumptionFailure(object failure)
    {
        ArgumentNullException.ThrowIfNull(failure);
        Console.WriteLine($"###IGNORED: {failure}");
    }
}
