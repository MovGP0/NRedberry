using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IToStringConverterTests
{
    [Fact]
    public void ShouldExposeReusableConverterInstances()
    {
        IToStringConverter<int> @default = ToStringConverters.Default;
        IToStringConverter<int> hex = ToStringConverters.Hex;
        IToStringConverter<int> binary = ToStringConverters.Binary;

        Assert.Equal("10", @default.ToString(10));
        Assert.Equal("ff", hex.ToString(255));
        Assert.Equal("101", binary.ToString(5));
    }
}
