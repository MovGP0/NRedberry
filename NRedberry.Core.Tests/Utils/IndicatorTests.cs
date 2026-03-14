using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IndicatorTests
{
    [Fact]
    public void ShouldAlwaysReturnTrueForTrueIndicator()
    {
        TrueIndicator<string> indicator = new();

        indicator.Is("value").ShouldBeTrue();
        indicator.Is(string.Empty).ShouldBeTrue();
    }

    [Fact]
    public void ShouldAlwaysReturnFalseForFalseIndicator()
    {
        FalseIndicator<string> indicator = new();

        indicator.Is("value").ShouldBeFalse();
        indicator.Is(string.Empty).ShouldBeFalse();
    }
}
