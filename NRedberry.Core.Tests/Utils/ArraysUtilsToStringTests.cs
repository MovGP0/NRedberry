using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsToStringTests
{
    [Fact]
    public void ShouldFormatNullAndEmptyArrays()
    {
        Assert.Equal("null", ArraysUtils.ToString<int>(null, ToStringConverters.Default));
        Assert.Equal("[]", ArraysUtils.ToString([], ToStringConverters.Default));
    }

    [Fact]
    public void ShouldFormatIntegerArraysWithConverter()
    {
        string actual = ArraysUtils.ToString([10, 15], ToStringConverters.Hex);

        Assert.Equal("[a, f]", actual);
    }

    [Fact]
    public void ShouldFormatGenericArraysWithCustomConverter()
    {
        string actual = ArraysUtils.ToString(["a", "b"], new WrapperConverter());

        Assert.Equal("[<a>, <b>]", actual);
    }

    private sealed class WrapperConverter : IToStringConverter<string>
    {
        public string ToString(string t)
        {
            return $"<{t}>";
        }
    }
}
