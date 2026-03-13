using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchTests
{
    [Fact]
    public void ShouldExposeValueEqualityAndFormatting()
    {
        Stretch stretch = new(3, 4);

        Assert.Equal(new Stretch(3, 4), stretch);
        Assert.Equal("Stretch{from=3, length=4}", stretch.ToString());
    }
}
