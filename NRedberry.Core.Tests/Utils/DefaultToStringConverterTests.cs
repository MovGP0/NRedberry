using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class DefaultToStringConverterTests
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(42, "42")]
    [InlineData(-7, "-7")]
    public void ShouldFormatValuesWithDefaultIntegerFormatting(int value, string expected)
    {
        DefaultToStringConverter converter = new();

        string actual = converter.ToString(value);

        actual.ShouldBe(expected);
    }
}
