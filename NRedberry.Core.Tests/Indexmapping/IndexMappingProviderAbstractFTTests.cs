using System.Collections.Generic;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingProviderAbstractFTTests
{
    [Fact]
    public void ConstructorShouldThrowArgumentNullExceptionWhenFromTensorIsNull()
    {
        IOutputPort<IIndexMappingBuffer> outputPort = new SequenceOutputPort(new TestIndexMappingBuffer());
        TensorType toTensor = Complex.Zero;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new TestProvider(outputPort, fromTensor: null!, toTensor));

        Assert.Equal("fromTensor", exception.ParamName);
    }

    [Fact]
    public void ConstructorShouldThrowArgumentNullExceptionWhenToTensorIsNull()
    {
        IOutputPort<IIndexMappingBuffer> outputPort = new SequenceOutputPort(new TestIndexMappingBuffer());
        TensorType fromTensor = Complex.One;

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new TestProvider(outputPort, fromTensor, toTensor: null!));

        Assert.Equal("toTensor", exception.ParamName);
    }

    [Fact]
    public void ConstructorShouldStoreFromAndToReferences()
    {
        IOutputPort<IIndexMappingBuffer> outputPort = new SequenceOutputPort(new TestIndexMappingBuffer());
        TensorType fromTensor = Complex.One;
        TensorType toTensor = Complex.Zero;

        var provider = new TestProvider(outputPort, fromTensor, toTensor);

        Assert.Same(fromTensor, provider.FromTensor);
        Assert.Same(toTensor, provider.ToTensor);
    }

    [Fact]
    public void TickAndTakeShouldUseBaseProviderOutputPortBehavior()
    {
        IIndexMappingBuffer buffer = new TestIndexMappingBuffer();
        var provider = new TestProvider(new SequenceOutputPort(buffer), Complex.One, Complex.Zero);

        bool firstTick = provider.Tick();
        IIndexMappingBuffer? firstTake = provider.Take();

        bool secondTick = provider.Tick();
        IIndexMappingBuffer? secondTake = provider.Take();

        Assert.True(firstTick);
        Assert.Same(buffer, firstTake);
        Assert.False(secondTick);
        Assert.Null(secondTake);
    }

    private sealed class TestProvider : IndexMappingProviderAbstractFT<TensorType>
    {
        internal TestProvider(IOutputPort<IIndexMappingBuffer> outputPort, TensorType fromTensor, TensorType toTensor)
            : base(outputPort, fromTensor, toTensor)
        {
        }

        internal TensorType FromTensor => From;
        internal TensorType ToTensor => To;

        public override IIndexMappingBuffer? Take()
        {
            return currentBuffer;
        }
    }

    private sealed class SequenceOutputPort : IOutputPort<IIndexMappingBuffer>
    {
        private readonly Queue<IIndexMappingBuffer> _buffers;

        internal SequenceOutputPort(params IIndexMappingBuffer[] buffers)
        {
            _buffers = new Queue<IIndexMappingBuffer>(buffers);
        }

        public IIndexMappingBuffer Take()
        {
            return _buffers.Count > 0 ? _buffers.Dequeue() : null!;
        }
    }

    private sealed class TestIndexMappingBuffer : IIndexMappingBuffer
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
            return true;
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
            return new TestIndexMappingBuffer();
        }
    }
}
