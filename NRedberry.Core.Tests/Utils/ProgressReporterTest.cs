using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ProgressReporterTest
{
    [Fact(Skip = "Long-running timing-based test ignored in Java.")]
    public void ShouldAdvanceProgressReporter()
    {
        // TODO: Port test1 if timing-based behavior is needed in C#.
    }
}
