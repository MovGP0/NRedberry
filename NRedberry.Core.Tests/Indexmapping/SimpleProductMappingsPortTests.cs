using System;
using System.Collections.Generic;
using System.Reflection;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using Xunit;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class SimpleProductMappingsPortTests
{
    private static readonly Type s_simpleProductMappingsPortType =
        typeof(IndexMappings).Assembly.GetType("NRedberry.IndexMapping.SimpleProductMappingsPort", throwOnError: true)!;

    [Fact]
    public void FirstConstructorShouldThrowArgumentNullExceptionWhenProvidersIsNull()
    {
        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(() =>
            CreateWithProviders(providers: null!));

        Assert.Equal("providers", exception.ParamName);
    }

    [Fact]
    public void SecondConstructorShouldThrowArgumentNullExceptionWhenProviderIsNull()
    {
        TensorType[] from = [Complex.One];
        TensorType[] to = [Complex.Zero];

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(() =>
            CreateWithProviderAndTensors(provider: null!, from, to));

        Assert.Equal("provider", exception.ParamName);
    }

    [Fact]
    public void SecondConstructorShouldThrowArgumentNullExceptionWhenFromIsNull()
    {
        IIndexMappingProvider provider = new SequenceIndexMappingProvider([], []);
        TensorType[] to = [Complex.Zero];

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(() =>
            CreateWithProviderAndTensors(provider, from: null!, to));

        Assert.Equal("from", exception.ParamName);
    }

    [Fact]
    public void SecondConstructorShouldThrowArgumentNullExceptionWhenToIsNull()
    {
        IIndexMappingProvider provider = new SequenceIndexMappingProvider([], []);
        TensorType[] from = [Complex.One];

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(() =>
            CreateWithProviderAndTensors(provider, from, to: null!));

        Assert.Equal("to", exception.ParamName);
    }

    [Fact]
    public void FirstConstructorTakeShouldInitializeProvidersOnceAndReturnLastProviderBuffersThenNull()
    {
        SequenceIndexMappingProvider first = new([false], []);
        MappingBufferStub firstBuffer = new();
        MappingBufferStub secondBuffer = new();
        SequenceIndexMappingProvider last = new([false], [firstBuffer, secondBuffer, null]);

        object port = CreateWithProviders([first, last]);

        IIndexMappingBuffer? take1 = InvokeTake(port);
        IIndexMappingBuffer? take2 = InvokeTake(port);

        Assert.Equal(1, first.TickCallCount);
        Assert.Equal(1, last.TickCallCount);
        Assert.Same(firstBuffer, take1);
        Assert.Same(secondBuffer, take2);

        IIndexMappingBuffer? take3 = InvokeTake(port);

        Assert.Null(take3);
        Assert.Equal(2, first.TickCallCount);
        Assert.Equal(2, last.TickCallCount);
    }

    [Fact]
    public void SecondConstructorShouldThrowNotImplementedExceptionWithMinimalTensorArrays()
    {
        IIndexMappingProvider provider = new SequenceIndexMappingProvider([], []);
        TensorType[] from = [Complex.One];
        TensorType[] to = [Complex.Zero];

        NotImplementedException exception = AssertInnerException<NotImplementedException>(() =>
            CreateWithProviderAndTensors(provider, from, to));

        Assert.NotNull(exception);
    }

    private static object CreateWithProviders(IIndexMappingProvider[] providers)
    {
        ConstructorInfo? constructor = s_simpleProductMappingsPortType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            [typeof(IIndexMappingProvider[])],
            modifiers: null);

        Assert.NotNull(constructor);
        return constructor.Invoke([providers]);
    }

    private static object CreateWithProviderAndTensors(IIndexMappingProvider provider, TensorType[] from, TensorType[] to)
    {
        ConstructorInfo? constructor = s_simpleProductMappingsPortType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            [typeof(IIndexMappingProvider), typeof(TensorType[]), typeof(TensorType[])],
            modifiers: null);

        Assert.NotNull(constructor);
        return constructor.Invoke([provider, from, to]);
    }

    private static IIndexMappingBuffer? InvokeTake(object port)
    {
        MethodInfo? take = s_simpleProductMappingsPortType.GetMethod(
            nameof(IIndexMappingProvider.Take),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        Assert.NotNull(take);
        return (IIndexMappingBuffer?)take.Invoke(port, null);
    }

    private static TException AssertInnerException<TException>(Action action)
        where TException : Exception
    {
        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(action);

        return Assert.IsType<TException>(exception.InnerException);
    }

    private sealed class SequenceIndexMappingProvider : IIndexMappingProvider
    {
        private readonly Queue<bool> _ticks;
        private readonly Queue<IIndexMappingBuffer?> _takes;

        public SequenceIndexMappingProvider(IEnumerable<bool> ticks, IEnumerable<IIndexMappingBuffer?> takes)
        {
            _ticks = new Queue<bool>(ticks);
            _takes = new Queue<IIndexMappingBuffer?>(takes);
        }

        public int TickCallCount { get; private set; }

        public bool Tick()
        {
            TickCallCount++;

            if (_ticks.Count == 0)
            {
                return false;
            }

            return _ticks.Dequeue();
        }

        public IIndexMappingBuffer Take()
        {
            if (_takes.Count == 0)
            {
                return null!;
            }

            return _takes.Dequeue()!;
        }
    }

    private sealed class MappingBufferStub : IIndexMappingBuffer
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
            return new MappingBufferStub();
        }
    }
}
