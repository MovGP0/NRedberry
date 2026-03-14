using Xunit;

namespace NRedberry.Groovy.Tests;

public sealed class GroovyGlobalRunListenerTests
{
    [Fact]
    public void ShouldResetTensorNamesWithoutThrowing()
    {
        GroovyGlobalRunListener listener = new();

        listener.TestStarted("groovy");
    }

    [Fact]
    public void ShouldWriteFailureMessageIncludingSeed()
    {
        GroovyGlobalRunListener listener = new();
        StringWriter writer = new();
        TextWriter original = Console.Out;

        try
        {
            Console.SetOut(writer);
            listener.TestFailure("groovy.failure");
        }
        finally
        {
            Console.SetOut(original);
        }

        string output = writer.ToString();
        Assert.Contains("groovy.failure", output, StringComparison.Ordinal);
        Assert.Contains("name manager seed", output, StringComparison.OrdinalIgnoreCase);
    }
}
