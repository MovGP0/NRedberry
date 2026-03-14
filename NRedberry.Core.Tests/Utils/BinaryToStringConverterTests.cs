using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class BinaryToStringConverterTests
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(5, "101")]
    [InlineData(42, "101010")]
    public void ShouldFormatValuesAsBinary(int value, string expected)
    {
        BinaryToStringConverter converter = new();

        string actual = converter.ToString(value);

        actual.ShouldBe(expected);
    }
}
