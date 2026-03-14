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

        @default.ToString(10).ShouldBe("10");
        hex.ToString(255).ShouldBe("ff");
        binary.ToString(5).ShouldBe("101");
    }
}
