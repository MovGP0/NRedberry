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

        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() =>
            Activator.CreateInstance(
                wrapperType,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: [null],
                culture: null));

        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("provider");
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

        result.ShouldBeTrue();
        wrappedProvider.TickCallCount.ShouldBe(1);
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

        result.ShouldBeNull();
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

        result.ShouldBeSameAs(buffer);
        buffer.AddSignCallCount.ShouldBe(1);
        buffer.LastSignArgument.ShouldBeTrue();
    }

    private static object CreateWrapper(IIndexMappingProvider provider)
    {
        Type wrapperType = GetWrapperType();

        object? instance = Activator.CreateInstance(
            wrapperType,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            args: [provider],
            culture: null);

        instance.ShouldNotBeNull();
        instance.GetType().ShouldBe(wrapperType);
        return instance;
    }

    private static Type GetWrapperType()
    {
        Type? wrapperType = typeof(IIndexMappingProvider).Assembly
            .GetType("NRedberry.IndexMapping.MinusIndexMappingProviderWrapper", throwOnError: false);

        (wrapperType is not null).ShouldBeTrue();
        return wrapperType;
    }

    private static bool InvokeTick(object wrapper)
    {
        MethodInfo tickMethod = wrapper.GetType().GetMethod("Tick")!;
        object? tickResult = tickMethod.Invoke(wrapper, null);
        return tickResult.ShouldBeOfType<bool>();
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
