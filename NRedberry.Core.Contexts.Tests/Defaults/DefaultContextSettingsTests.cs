using NRedberry.Contexts.Defaults;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class DefaultContextSettingsTests
{
    [Fact(DisplayName = "Should configure default metric index types")]
    public void ShouldConfigureDefaultMetricIndexTypes()
    {
        // Act
        var settings = DefaultContextSettings.Create();

        // Assert
        settings.MetricTypes.ShouldContain(IndexType.LatinLower);
        settings.MetricTypes.ShouldContain(IndexType.GreekLower);
        settings.MetricTypes.ShouldContain(IndexType.LatinUpper);
        settings.MetricTypes.ShouldContain(IndexType.GreekUpper);
    }

    [Fact(DisplayName = "Should read seed from environment variable when valid")]
    public void ShouldReadSeedFromEnvironmentWhenValid()
    {
        // Arrange
        string? original = Environment.GetEnvironmentVariable("redberry.nmseed");
        Environment.SetEnvironmentVariable("redberry.nmseed", "123");
        try
        {
            // Act
            var settings = DefaultContextSettings.Create();

            // Assert
            settings.NameManagerSeed.ShouldBe(123);
        }
        finally
        {
            Environment.SetEnvironmentVariable("redberry.nmseed", original);
        }
    }

    [Fact(DisplayName = "Should default seed to 10 when environment is invalid")]
    public void ShouldDefaultSeedToTenWhenEnvironmentIsInvalid()
    {
        // Arrange
        string? original = Environment.GetEnvironmentVariable("redberry.nmseed");
        Environment.SetEnvironmentVariable("redberry.nmseed", "not-a-number");
        try
        {
            // Act
            var settings = DefaultContextSettings.Create();

            // Assert
            settings.NameManagerSeed.ShouldBe(10);
        }
        finally
        {
            Environment.SetEnvironmentVariable("redberry.nmseed", original);
        }
    }
}
