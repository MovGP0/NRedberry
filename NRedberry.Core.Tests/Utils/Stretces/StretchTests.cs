using NRedberry.Core.Utils.Stretces;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class StretchTests
{
    [Fact]
    public void ShouldExposeValueEqualityAndFormatting()
    {
        Stretch stretch = new(3, 4);

        stretch.ShouldBe(new Stretch(3, 4));
        stretch.ToString().ShouldBe("Stretch{from=3, length=4}");
    }
}
