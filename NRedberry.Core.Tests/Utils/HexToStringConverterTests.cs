using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class HexToStringConverterTests
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(15, "f")]
    [InlineData(255, "ff")]
    public void ShouldFormatValuesAsLowercaseHex(int value, string expected)
    {
        HexToStringConverter converter = new();

        string actual = converter.ToString(value);

        actual.ShouldBe(expected);
    }
}
