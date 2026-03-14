using NRedberry.Concurrent;
using Xunit;

namespace NRedberry.Core.Tests.Concurrent;

#pragma warning disable CS0618
public sealed class IOutputPortUnsafeTests
{
    [Fact]
    public void ShouldReturnValueFromUnsafePort()
    {
        IOutputPortUnsafe<string> port = new TestOutputPortUnsafe("payload");

        string? value = port.Take();

        Assert.Equal("payload", value);
    }

    [Fact]
    public void ShouldAllowNullFromUnsafePort()
    {
        IOutputPortUnsafe<string> port = new TestOutputPortUnsafe(null);

        string? value = port.Take();

        Assert.Null(value);
    }

    private sealed class TestOutputPortUnsafe(string? value) : IOutputPortUnsafe<string>
    {
        public string? Take()
        {
            return value;
        }
    }
}
#pragma warning restore CS0618
