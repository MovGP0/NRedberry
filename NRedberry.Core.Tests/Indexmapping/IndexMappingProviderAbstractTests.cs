using System;
using System.Collections.Generic;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingProviderAbstractTests
{
    [Fact]
    public void ConstructorShouldThrowWhenOutputPortIsNull()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new IndexMappingProviderAbstractTestDouble(null!));

        Assert.Equal("outputPort", exception.ParamName);
    }

    [Fact]
    public void TickShouldCallBeforeTickThenPullFromOutputPortAndReturnTrueWhenBufferExists()
    {
        List<string> events = [];
        IIndexMappingBuffer expectedBuffer = new IndexMappingBufferForIndexMappingProviderAbstractTests();
        QueueOutputPortForIndexMappingProviderAbstractTests outputPort = new([expectedBuffer], events);
        IndexMappingProviderAbstractTestDouble provider = new(outputPort, events);

        bool result = provider.Tick();

        Assert.True(result);
        Assert.Equal(1, provider.BeforeTickCallCount);
        Assert.Equal(1, outputPort.TakeCallCount);
        Assert.Equal(["BeforeTick", "Take"], events);
    }

    [Fact]
    public void TickShouldCallBeforeTickThenPullFromOutputPortAndReturnFalseWhenBufferMissing()
    {
        List<string> events = [];
        QueueOutputPortForIndexMappingProviderAbstractTests outputPort = new([null], events);
        IndexMappingProviderAbstractTestDouble provider = new(outputPort, events);

        bool result = provider.Tick();

        Assert.False(result);
        Assert.Equal(1, provider.BeforeTickCallCount);
        Assert.Equal(1, outputPort.TakeCallCount);
        Assert.Equal(["BeforeTick", "Take"], events);
    }

    [Fact]
    public void TickShouldUpdateCurrentBufferConsumedByTakeImplementation()
    {
        IIndexMappingBuffer firstBuffer = new IndexMappingBufferForIndexMappingProviderAbstractTests();
        IIndexMappingBuffer secondBuffer = new IndexMappingBufferForIndexMappingProviderAbstractTests();
        QueueOutputPortForIndexMappingProviderAbstractTests outputPort = new([firstBuffer, secondBuffer]);
        IndexMappingProviderAbstractTestDouble provider = new(outputPort);

        bool firstTick = provider.Tick();
        IIndexMappingBuffer? firstTake = provider.Take();
        IIndexMappingBuffer? secondTakeWithoutTick = provider.Take();
        bool secondTick = provider.Tick();
        IIndexMappingBuffer? secondTake = provider.Take();
        IIndexMappingBuffer? thirdTakeWithoutTick = provider.Take();

        Assert.True(firstTick);
        Assert.True(secondTick);
        Assert.Same(firstBuffer, firstTake);
        Assert.Same(secondBuffer, secondTake);
        Assert.Null(secondTakeWithoutTick);
        Assert.Null(thirdTakeWithoutTick);
    }
}

internal sealed class IndexMappingProviderAbstractTestDouble : IndexMappingProviderAbstract
{
    private readonly List<string>? _events;

    public IndexMappingProviderAbstractTestDouble(IOutputPort<IIndexMappingBuffer> outputPort, List<string>? events = null)
        : base(outputPort)
    {
        _events = events;
    }

    public int BeforeTickCallCount { get; private set; }

    public override IIndexMappingBuffer? Take()
    {
        IIndexMappingBuffer? buffer = currentBuffer;
        currentBuffer = null;
        return buffer;
    }

    protected override void BeforeTick()
    {
        BeforeTickCallCount++;
        _events?.Add("BeforeTick");
    }
}

internal sealed class QueueOutputPortForIndexMappingProviderAbstractTests : IOutputPort<IIndexMappingBuffer>
{
    private readonly Queue<IIndexMappingBuffer?> _values;
    private readonly List<string>? _events;

    public QueueOutputPortForIndexMappingProviderAbstractTests(IEnumerable<IIndexMappingBuffer?> values, List<string>? events = null)
    {
        _values = new Queue<IIndexMappingBuffer?>(values);
        _events = events;
    }

    public int TakeCallCount { get; private set; }

    public IIndexMappingBuffer Take()
    {
        TakeCallCount++;
        _events?.Add("Take");

        if (_values.Count == 0)
        {
            return null!;
        }

        return _values.Dequeue()!;
    }
}

internal sealed class IndexMappingBufferForIndexMappingProviderAbstractTests : IIndexMappingBuffer
{
    public bool TryMap(int from, int to)
    {
        throw new NotSupportedException();
    }

    public void AddSign(bool sign)
    {
        throw new NotSupportedException();
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
        throw new NotSupportedException();
    }
}
