using System.Reflection;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class MinusIndexMappingProviderWrapperTests
{
    [Fact]
    public void ConstructorShouldThrowWhenProviderIsNull()
    {
        Type wrapperType = GetWrapperType();

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() =>
            Activator.CreateInstance(
                wrapperType,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: new object?[] { null },
                culture: null));

        ArgumentNullException innerException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("provider", innerException.ParamName);
    }

    [Fact]
    public void TickShouldDelegateToWrappedProvider()
    {
        StubProvider wrappedProvider = new()
        {
            TickResult = true
        };

        object wrapper = CreateWrapper(wrappedProvider);

        bool result = InvokeTick(wrapper);

        Assert.True(result);
        Assert.Equal(1, wrappedProvider.TickCallCount);
    }

    [Fact]
    public void TakeShouldReturnNullWhenWrappedProviderReturnsNull()
    {
        StubProvider wrappedProvider = new()
        {
            TakeResult = null
        };

        object wrapper = CreateWrapper(wrappedProvider);

        IIndexMappingBuffer? result = InvokeTake(wrapper);

        Assert.Null(result);
    }

    [Fact]
    public void TakeShouldAddNegativeSignAndReturnSameBufferWhenWrappedProviderReturnsBuffer()
    {
        StubBuffer buffer = new();
        StubProvider wrappedProvider = new()
        {
            TakeResult = buffer
        };

        object wrapper = CreateWrapper(wrappedProvider);

        IIndexMappingBuffer? result = InvokeTake(wrapper);

        Assert.Same(buffer, result);
        Assert.Equal(1, buffer.AddSignCallCount);
        Assert.True(buffer.LastSignArgument);
    }

    private static object CreateWrapper(IIndexMappingProvider provider)
    {
        Type wrapperType = GetWrapperType();

        object? instance = Activator.CreateInstance(
            wrapperType,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            args: new object[] { provider },
            culture: null);

        Assert.NotNull(instance);
        Assert.Equal(wrapperType, instance.GetType());
        return instance;
    }

    private static Type GetWrapperType()
    {
        Type? wrapperType = typeof(IIndexMappingProvider).Assembly
            .GetType("NRedberry.IndexMapping.MinusIndexMappingProviderWrapper", throwOnError: false);

        Assert.True(wrapperType is not null);
        return wrapperType;
    }

    private static bool InvokeTick(object wrapper)
    {
        MethodInfo tickMethod = wrapper.GetType().GetMethod("Tick")!;
        object? tickResult = tickMethod.Invoke(wrapper, null);
        return Assert.IsType<bool>(tickResult);
    }

    private static IIndexMappingBuffer? InvokeTake(object wrapper)
    {
        MethodInfo takeMethod = wrapper.GetType().GetMethod("Take")!;
        return (IIndexMappingBuffer?)takeMethod.Invoke(wrapper, null);
    }

    private sealed class StubProvider : IIndexMappingProvider
    {
        public bool TickResult { get; set; }

        public IIndexMappingBuffer? TakeResult { get; set; }

        public int TickCallCount { get; private set; }

        public bool Tick()
        {
            TickCallCount++;
            return TickResult;
        }

        public IIndexMappingBuffer Take()
        {
            return TakeResult!;
        }
    }

    private sealed class StubBuffer : IIndexMappingBuffer
    {
        public int AddSignCallCount { get; private set; }

        public bool LastSignArgument { get; private set; }

        public bool TryMap(int from, int to)
        {
            throw new NotSupportedException();
        }

        public void AddSign(bool sign)
        {
            AddSignCallCount++;
            LastSignArgument = sign;
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
}
