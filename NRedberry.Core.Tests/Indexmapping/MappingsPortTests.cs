using System;
using System.Collections.Generic;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

#pragma warning disable CS0618
public sealed class MappingsPortTests
{
    [Fact]
    public void ConstructorShouldThrowWhenInnerPortIsNull()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MappingsPort(null!));

        Assert.Equal("innerPort", exception.ParamName);
    }

    [Fact]
    public void TakeShouldReturnNullWhenInnerPortReturnsNull()
    {
        var mappingsPort = new MappingsPort(new MappingsPortTestsNullOutputPortDouble());

        Mapping? result = mappingsPort.Take();

        Assert.Null(result);
    }

    [Fact]
    public void TakeShouldThrowNotImplementedExceptionWhenInnerPortReturnsBuffer()
    {
        var mappingsPort = new MappingsPort(new MappingsPortTestsSingleBufferOutputPortDouble(new MappingsPortTestsBufferDouble()));

        Assert.Throws<NotImplementedException>(() => mappingsPort.Take());
    }

    private sealed class MappingsPortTestsNullOutputPortDouble : IOutputPort<IIndexMappingBuffer>
    {
        public IIndexMappingBuffer Take()
        {
            return null!;
        }
    }

    private sealed class MappingsPortTestsSingleBufferOutputPortDouble : IOutputPort<IIndexMappingBuffer>
    {
        private IIndexMappingBuffer? _buffer;

        public MappingsPortTestsSingleBufferOutputPortDouble(IIndexMappingBuffer buffer)
        {
            ArgumentNullException.ThrowIfNull(buffer);

            _buffer = buffer;
        }

        public IIndexMappingBuffer Take()
        {
            IIndexMappingBuffer? current = _buffer;
            _buffer = null;
            return current!;
        }
    }

    private sealed class MappingsPortTestsBufferDouble : IIndexMappingBuffer
    {
        public bool TryMap(int from, int to)
        {
            return true;
        }

        public void AddSign(bool sign)
        {
        }

        public void RemoveContracted()
        {
        }

        public bool IsEmpty()
        {
            return true;
        }

        public bool GetSign()
        {
            return false;
        }

        public object Export()
        {
            return new object();
        }

        public IDictionary<int, IndexMappingBufferRecord> GetMap()
        {
            return new Dictionary<int, IndexMappingBufferRecord>();
        }

        public object Clone()
        {
            return new MappingsPortTestsBufferDouble();
        }
    }
}
#pragma warning restore CS0618
