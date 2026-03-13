using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ProgressReporterTest
{
    [Fact]
    public void ShouldPrintProgressAtConfiguredSteps()
    {
        StringWriter writer = new();
        TextWriter original = Console.Out;

        try
        {
            Console.SetOut(writer);
            ProgressReporter reporter = new("work", 4, 25);

            Assert.True(reporter.Next());
            Assert.True(reporter.Next());
            Assert.True(reporter.Next());
            Assert.True(reporter.Next());

            string output = writer.ToString();

            Assert.Contains("work", output);
            Assert.Contains("25.00%", output);
            Assert.Contains("100.00%", output);
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    [Fact]
    public void ShouldReturnFalseAfterLimitIsExceeded()
    {
        StringWriter writer = new();
        TextWriter original = Console.Out;

        try
        {
            Console.SetOut(writer);
            ProgressReporter reporter = new("done", 1, 100);

            Assert.True(reporter.Next());
            Assert.False(reporter.Next());
        }
        finally
        {
            Console.SetOut(original);
        }
    }
}
