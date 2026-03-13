using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IndicatorTests
{
    [Fact]
    public void ShouldAlwaysReturnTrueForTrueIndicator()
    {
        TrueIndicator<string> indicator = new();

        Assert.True(indicator.Is("value"));
        Assert.True(indicator.Is(string.Empty));
    }

    [Fact]
    public void ShouldAlwaysReturnFalseForFalseIndicator()
    {
        FalseIndicator<string> indicator = new();

        Assert.False(indicator.Is("value"));
        Assert.False(indicator.Is(string.Empty));
    }
}
