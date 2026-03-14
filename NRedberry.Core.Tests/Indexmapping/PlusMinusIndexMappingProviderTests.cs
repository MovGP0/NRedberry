using System;
using System.Collections.Generic;
using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class PlusMinusIndexMappingProviderTests
{
    [Fact]
    public void ConstructorShouldThrowWhenOutputPortIsNull()
    {
        Type providerType = GetProviderType();

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() =>
            Activator.CreateInstance(
                providerType,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: [null],
                culture: null));

        ArgumentNullException innerException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("outputPort", innerException.ParamName);
    }

    [Fact]
    public void TakeShouldReturnNullWithoutCurrentBuffer()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new SequenceOutputPort());

        IIndexMappingBuffer? result = provider.Take();

        Assert.Null(result);
    }

    [Fact]
    public void TakeShouldReturnCloneThenSignedOriginalThenNullAfterTickWithBuffer()
    {
        TrackingIndexMappingBuffer originalBuffer = new();
        SequenceOutputPort outputPort = new(originalBuffer);
        IndexMappingProviderAbstract provider = CreateProvider(outputPort);

        bool tickResult = provider.Tick();
        IIndexMappingBuffer? firstTake = provider.Take();

        Assert.True(tickResult);
        TrackingIndexMappingBuffer firstClone = Assert.IsType<TrackingIndexMappingBuffer>(firstTake);
        Assert.NotSame(originalBuffer, firstClone);
        Assert.Equal(1, originalBuffer.CloneCallCount);
        Assert.Equal(0, originalBuffer.AddSignCallCount);
        Assert.Equal(0, firstClone.AddSignCallCount);

        IIndexMappingBuffer? secondTake = provider.Take();

        Assert.Same(originalBuffer, secondTake);
        Assert.Equal(1, originalBuffer.AddSignCallCount);
        Assert.Equal([true], originalBuffer.AddSignArguments);

        IIndexMappingBuffer? thirdTake = provider.Take();

        Assert.Null(thirdTake);
    }

    private static IndexMappingProviderAbstract CreateProvider(IOutputPort<IIndexMappingBuffer> outputPort)
    {
        Type providerType = GetProviderType();

        object? instance = Activator.CreateInstance(
            providerType,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            args: [outputPort],
            culture: null);

        return Assert.IsAssignableFrom<IndexMappingProviderAbstract>(instance);
    }

    private static Type GetProviderType()
    {
        Type? providerType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.PlusMinusIndexMappingProvider", throwOnError: false);

        Assert.True(providerType is not null);
        return providerType;
    }

    private sealed class SequenceOutputPort : IOutputPort<IIndexMappingBuffer>
    {
        private readonly Queue<IIndexMappingBuffer?> _buffers;

        public SequenceOutputPort(params IIndexMappingBuffer?[] buffers)
        {
            _buffers = new Queue<IIndexMappingBuffer?>(buffers);
        }

        public IIndexMappingBuffer Take()
        {
            if (_buffers.Count == 0)
            {
                return null!;
            }

            return _buffers.Dequeue()!;
        }
    }

    private sealed class TrackingIndexMappingBuffer : IIndexMappingBuffer
    {
        private readonly List<bool> _addSignArguments = [];

        public int CloneCallCount { get; private set; }

        public int AddSignCallCount => _addSignArguments.Count;

        public IReadOnlyList<bool> AddSignArguments => _addSignArguments;

        public bool TryMap(int from, int to)
        {
            throw new NotSupportedException();
        }

        public void AddSign(bool sign)
        {
            _addSignArguments.Add(sign);
        }

        public void RemoveContracted()
        {
            throw new NotSupportedException();
        }

        public bool IsEmpty()
        {
            throw new NotSupportedException();
        }

        public bool GetSign()
        {
            throw new NotSupportedException();
        }

        public object Export()
        {
            throw new NotSupportedException();
        }

        public IDictionary<int, IndexMappingBufferRecord> GetMap()
        {
            throw new NotSupportedException();
        }

        public object Clone()
        {
            CloneCallCount++;
            return new TrackingIndexMappingBuffer();
        }
    }
}
