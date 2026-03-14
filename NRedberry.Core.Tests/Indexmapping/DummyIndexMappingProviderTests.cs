using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class DummyIndexMappingProviderTests
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
    public void TickShouldReturnFalseWhenProviderIsEmpty()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new NullOutputPort());

        bool result = provider.Tick();

        Assert.False(result);
    }

    [Fact]
    public void TakeShouldReturnNullAfterTickWhenProviderIsEmpty()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new NullOutputPort());
        provider.Tick();

        IIndexMappingBuffer? result = provider.Take();

        Assert.Null(result);
    }

    [Fact]
    public void TakeShouldRemainNullOnSubsequentCallsAfterTickWhenProviderIsEmpty()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new NullOutputPort());
        provider.Tick();

        IIndexMappingBuffer? firstTake = provider.Take();
        IIndexMappingBuffer? secondTake = provider.Take();

        Assert.Null(firstTake);
        Assert.Null(secondTake);
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
            .GetType("NRedberry.IndexMapping.DummyIndexMappingProvider", throwOnError: false);

        Assert.True(providerType is not null);
        return providerType;
    }

    private sealed class NullOutputPort : IOutputPort<IIndexMappingBuffer>
    {
        public IIndexMappingBuffer Take()
        {
            return null!;
        }
    }
}
