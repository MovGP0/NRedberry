using NRedberry.Core.Utils;

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

            reporter.Next().ShouldBeTrue();
            reporter.Next().ShouldBeTrue();
            reporter.Next().ShouldBeTrue();
            reporter.Next().ShouldBeTrue();

            string output = writer.ToString();

            output.ShouldContain("work");
            output.ShouldContain("25.00%");
            output.ShouldContain("100.00%");
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

            reporter.Next().ShouldBeTrue();
            reporter.Next().ShouldBeFalse();
        }
        finally
        {
            Console.SetOut(original);
        }
    }
}
