using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingProviderAbstractFTTests
{
    [Fact]
    public void ConstructorShouldThrowArgumentNullExceptionWhenFromTensorIsNull()
    {
        IOutputPort<IIndexMappingBuffer> outputPort = new SequenceOutputPort(new TestIndexMappingBuffer());
        TensorType toTensor = Complex.Zero;

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
            new TestProvider(outputPort, fromTensor: null!, toTensor));

        exception.ParamName.ShouldBe("fromTensor");
    }

    [Fact]
    public void ConstructorShouldThrowArgumentNullExceptionWhenToTensorIsNull()
    {
        IOutputPort<IIndexMappingBuffer> outputPort = new SequenceOutputPort(new TestIndexMappingBuffer());
        TensorType fromTensor = Complex.One;

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
            new TestProvider(outputPort, fromTensor, toTensor: null!));

        exception.ParamName.ShouldBe("toTensor");
    }

    [Fact]
    public void ConstructorShouldStoreFromAndToReferences()
    {
        IOutputPort<IIndexMappingBuffer> outputPort = new SequenceOutputPort(new TestIndexMappingBuffer());
        TensorType fromTensor = Complex.One;
        TensorType toTensor = Complex.Zero;

        var provider = new TestProvider(outputPort, fromTensor, toTensor);

        provider.FromTensor.ShouldBeSameAs(fromTensor);
        provider.ToTensor.ShouldBeSameAs(toTensor);
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

        firstTick.ShouldBeTrue();
        firstTake.ShouldBeSameAs(buffer);
        secondTick.ShouldBeFalse();
        secondTake.ShouldBeNull();
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
