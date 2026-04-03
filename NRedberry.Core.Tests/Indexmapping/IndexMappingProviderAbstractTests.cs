using NRedberry.Concurrent;
using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingProviderAbstractTests
{
    [Fact]
    public void ConstructorShouldThrowWhenOutputPortIsNull()
    {
        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
            new IndexMappingProviderAbstractTestDouble(null!));

        exception.ParamName.ShouldBe("outputPort");
    }

    [Fact]
    public void TickShouldCallBeforeTickThenPullFromOutputPortAndReturnTrueWhenBufferExists()
    {
        List<string> events = [];
        IIndexMappingBuffer expectedBuffer = new IndexMappingBufferForIndexMappingProviderAbstractTests();
        QueueOutputPortForIndexMappingProviderAbstractTests outputPort = new([expectedBuffer], events);
        IndexMappingProviderAbstractTestDouble provider = new(outputPort, events);

        bool result = provider.Tick();

        result.ShouldBeTrue();
        provider.BeforeTickCallCount.ShouldBe(1);
        outputPort.TakeCallCount.ShouldBe(1);
        events.ShouldBe(["BeforeTick", "Take"]);
    }

    [Fact]
    public void TickShouldCallBeforeTickThenPullFromOutputPortAndReturnFalseWhenBufferMissing()
    {
        List<string> events = [];
        QueueOutputPortForIndexMappingProviderAbstractTests outputPort = new([null], events);
        IndexMappingProviderAbstractTestDouble provider = new(outputPort, events);

        bool result = provider.Tick();

        result.ShouldBeFalse();
        provider.BeforeTickCallCount.ShouldBe(1);
        outputPort.TakeCallCount.ShouldBe(1);
        events.ShouldBe(["BeforeTick", "Take"]);
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

        firstTick.ShouldBeTrue();
        secondTick.ShouldBeTrue();
        firstTake.ShouldBeSameAs(firstBuffer);
        secondTake.ShouldBeSameAs(secondBuffer);
        secondTakeWithoutTick.ShouldBeNull();
        thirdTakeWithoutTick.ShouldBeNull();
    }
}

internal sealed class IndexMappingProviderAbstractTestDouble(
    IOutputPort<IIndexMappingBuffer> outputPort,
    List<string>? events = null)
    : IndexMappingProviderAbstract(outputPort)
{
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
        events?.Add("BeforeTick");
    }
}

internal sealed class QueueOutputPortForIndexMappingProviderAbstractTests(
    IEnumerable<IIndexMappingBuffer?> values,
    List<string>? events = null)
    : IOutputPort<IIndexMappingBuffer>
{
    private readonly Queue<IIndexMappingBuffer?> _values = new(values);

    public int TakeCallCount { get; private set; }

    public IIndexMappingBuffer Take()
    {
        TakeCallCount++;
        events?.Add("Take");

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
