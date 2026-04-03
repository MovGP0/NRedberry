using NRedberry.Contexts;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ContextBehaviorTests
{
    [Fact(DisplayName = "Should construct context from settings")]
    public void ShouldConstructContextFromSettings()
    {
        ContextSettings settings = new(
            OutputFormat.SimpleRedberry,
            "delta",
            "metric");

        Context context = new(settings);

        context.DefaultOutputFormat.ShouldBe(OutputFormat.SimpleRedberry);
        context.KroneckerName.ShouldBe("delta");
        context.MetricName.ShouldBe("metric");
        context.NameManager.ShouldNotBeNull();
        context.ParseManager.ShouldNotBeNull();
        context.ConverterManager.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Should allow tensor name resets")]
    public void ShouldAllowTensorNameResets()
    {
        Context context = new(new ContextSettings());

        context.ResetTensorNames();
        context.ResetTensorNames(123);
    }

    [Fact(DisplayName = "Should update metric and Kronecker names")]
    public void ShouldUpdateMetricAndKroneckerNames()
    {
        Context context = new(new ContextSettings());

        context.KroneckerName = "delta";
        context.MetricName = "metric";

        context.KroneckerName.ShouldBe("delta");
        context.MetricName.ShouldBe("metric");
    }
}
