using System.Collections.Generic;
using NRedberry.Concurrent;
using Xunit;

namespace NRedberry.Core.Tests.Concurrent;

#pragma warning disable CS0618
public sealed class PortEnumeratorTests
{
    [Fact]
    public void ShouldEnumerateUntilNull()
    {
        IOutputPortUnsafe<string> port = new TestOutputPortUnsafe(new[] { "a", "b" });
        using PortEnumerator<string> enumerator = new(port);

        Assert.True(enumerator.MoveNext());
        Assert.Equal("a", enumerator.Current);
        Assert.True(enumerator.MoveNext());
        Assert.Equal("b", enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void ShouldThrowOnReset()
    {
        IOutputPortUnsafe<string> port = new TestOutputPortUnsafe(Array.Empty<string>());
        using PortEnumerator<string> enumerator = new(port);

        Assert.Throws<NotSupportedException>(() => enumerator.Reset());
    }

    private sealed class TestOutputPortUnsafe : IOutputPortUnsafe<string>
    {
        private readonly Queue<string> values;

        public TestOutputPortUnsafe(IEnumerable<string> values)
        {
            this.values = new Queue<string>(values);
        }

        public string? Take()
        {
            return values.Count == 0 ? null : values.Dequeue();
        }
    }
}
#pragma warning restore CS0618
