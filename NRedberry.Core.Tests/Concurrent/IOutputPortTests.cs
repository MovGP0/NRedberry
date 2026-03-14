using NRedberry.Concurrent;
using Xunit;

namespace NRedberry.Core.Tests.Concurrent;

#pragma warning disable CS0618
public sealed class IOutputPortTests
{
    [Fact]
    public void ShouldReturnValueFromPort()
    {
        IOutputPort<string> port = new TestOutputPort("payload");

        string value = port.Take();

        Assert.Equal("payload", value);
    }

    private sealed class TestOutputPort(string value) : IOutputPort<string>
    {
        public string Take()
        {
            return value;
        }
    }
}
#pragma warning restore CS0618
