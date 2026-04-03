using NRedberry.Concurrent;

namespace NRedberry.Core.Tests.Concurrent;

#pragma warning disable CS0618
public sealed class IOutputPortUnsafeTests
{
    [Fact]
    public void ShouldReturnValueFromUnsafePort()
    {
        IOutputPortUnsafe<string> port = new TestOutputPortUnsafe("payload");

        string? value = port.Take();

        value.ShouldBe("payload");
    }

    [Fact]
    public void ShouldAllowNullFromUnsafePort()
    {
        IOutputPortUnsafe<string> port = new TestOutputPortUnsafe(null);

        string? value = port.Take();

        value.ShouldBeNull();
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
