using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsToStringTests
{
    [Fact]
    public void ShouldFormatNullAndEmptyArrays()
    {
        ArraysUtils.ToString<int>(null, ToStringConverters.Default).ShouldBe("null");
        ArraysUtils.ToString([], ToStringConverters.Default).ShouldBe("[]");
    }

    [Fact]
    public void ShouldFormatIntegerArraysWithConverter()
    {
        string actual = ArraysUtils.ToString([10, 15], ToStringConverters.Hex);

        actual.ShouldBe("[a, f]");
    }

    [Fact]
    public void ShouldFormatGenericArraysWithCustomConverter()
    {
        string actual = ArraysUtils.ToString(["a", "b"], new WrapperConverter());

        actual.ShouldBe("[<a>, <b>]");
    }

    private sealed class WrapperConverter : IToStringConverter<string>
    {
        public string ToString(string t)
        {
            return $"<{t}>";
        }
    }
}
