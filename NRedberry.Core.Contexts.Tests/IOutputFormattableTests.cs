using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class IOutputFormattableTests
{
    [Fact(DisplayName = "Should format using output format")]
    public void ShouldFormatUsingOutputFormat()
    {
        IOutputFormattable formattable = new SampleFormattable();

        formattable.ToString(OutputFormat.Redberry).ShouldBe("format:Redberry");
    }

    [Fact(DisplayName = "Should format using default ToString")]
    public void ShouldFormatUsingDefaultToString()
    {
        IOutputFormattable formattable = new SampleFormattable();

        formattable.ToString().ShouldBe("default");
    }

    private sealed class SampleFormattable : IOutputFormattable
    {
        public string ToString(OutputFormat outputFormat)
        {
            return $"format:{outputFormat}";
        }

        public override string ToString()
        {
            return "default";
        }
    }
}
