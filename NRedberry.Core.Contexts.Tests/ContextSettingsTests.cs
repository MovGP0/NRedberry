using NRedberry;
using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ContextSettingsTests
{
    [Fact(DisplayName = "Should set defaults in parameterless constructor")]
    public void ShouldSetDefaultsInParameterlessConstructor()
    {
        ContextSettings settings = new();

        settings.DefaultOutputFormat.ShouldBe(OutputFormat.Redberry);
        settings.Kronecker.ShouldBe("g");
        settings.MetricName.ShouldBe("g");
        settings.ConverterManager.ShouldNotBeNull();
        settings.Parser.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Should set values in constructor")]
    public void ShouldSetValuesInConstructor()
    {
        ContextSettings settings = new(OutputFormat.SimpleRedberry, "delta", "metric");

        settings.DefaultOutputFormat.ShouldBe(OutputFormat.SimpleRedberry);
        settings.Kronecker.ShouldBe("delta");
        settings.MetricName.ShouldBe("metric");
    }

    [Fact(DisplayName = "Should add and remove metric types")]
    public void ShouldAddAndRemoveMetricTypes()
    {
        ContextSettings settings = new();

        settings.AddMetricIndexType(IndexType.GreekLower);
        settings.MetricTypes.ShouldContain(IndexType.GreekLower);
        settings.RemoveMetricIndexType(IndexType.GreekLower);
        settings.MetricTypes.ShouldNotContain(IndexType.GreekLower);
    }

    [Fact(DisplayName = "Should validate Kronecker name")]
    public void ShouldValidateKroneckerName()
    {
        ContextSettings settings = new();

        Should.Throw<ArgumentNullException>(() => settings.Kronecker = null!);
        Should.Throw<ArgumentException>(() => settings.Kronecker = string.Empty);
    }

    [Fact(DisplayName = "Should validate metric name")]
    public void ShouldValidateMetricName()
    {
        ContextSettings settings = new();

        Should.Throw<ArgumentNullException>(() => settings.MetricName = null!);
        Should.Throw<ArgumentException>(() => settings.MetricName = string.Empty);
    }
}
