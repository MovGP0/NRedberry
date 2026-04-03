using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class PlusMinusIndexMappingProviderTests
{
    [Fact]
    public void ConstructorShouldThrowWhenOutputPortIsNull()
    {
        Type providerType = GetProviderType();

        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() =>
            Activator.CreateInstance(
                providerType,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: [null],
                culture: null));

        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("outputPort");
    }

    [Fact]
    public void TakeShouldReturnNullWithoutCurrentBuffer()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new SequenceOutputPort());

        IIndexMappingBuffer? result = provider.Take();

        result.ShouldBeNull();
    }

    [Fact]
    public void TakeShouldReturnCloneThenSignedOriginalThenNullAfterTickWithBuffer()
    {
        TrackingIndexMappingBuffer originalBuffer = new();
        SequenceOutputPort outputPort = new(originalBuffer);
        IndexMappingProviderAbstract provider = CreateProvider(outputPort);

        bool tickResult = provider.Tick();
        IIndexMappingBuffer? firstTake = provider.Take();

        tickResult.ShouldBeTrue();
        TrackingIndexMappingBuffer firstClone = firstTake.ShouldBeOfType<TrackingIndexMappingBuffer>();
        firstClone.ShouldNotBeSameAs(originalBuffer);
        originalBuffer.CloneCallCount.ShouldBe(1);
        originalBuffer.AddSignCallCount.ShouldBe(0);
        firstClone.AddSignCallCount.ShouldBe(0);

        IIndexMappingBuffer? secondTake = provider.Take();

        secondTake.ShouldBeSameAs(originalBuffer);
        originalBuffer.AddSignCallCount.ShouldBe(1);
        originalBuffer.AddSignArguments.ShouldBe([true]);

        IIndexMappingBuffer? thirdTake = provider.Take();

        thirdTake.ShouldBeNull();
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

        return instance.ShouldBeAssignableTo<IndexMappingProviderAbstract>();
    }

    private static Type GetProviderType()
    {
        Type? providerType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.PlusMinusIndexMappingProvider", throwOnError: false);

        (providerType is not null).ShouldBeTrue();
        return providerType;
    }

    private sealed class SequenceOutputPort(params IIndexMappingBuffer?[] buffers) : IOutputPort<IIndexMappingBuffer>
    {
        private readonly Queue<IIndexMappingBuffer?> _buffers = new(buffers);

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
