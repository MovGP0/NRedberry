using NRedberry.Tensors;

namespace NRedberry.Groovy.Tests;

public sealed class GroovyGlobalRunListener
{
    public void TestStarted(string? description = null)
    {
        CC.ResetTensorNames();
    }

    public void TestFailure(string? message = null)
    {
        Console.WriteLine($"Test failed with name manager seed: {CC.NameManager.GetSeed()}");
        if (!string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine(message);
        }
    }
}
