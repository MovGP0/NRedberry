using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class DummyIndexMappingProviderTests
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
    public void TickShouldReturnFalseWhenProviderIsEmpty()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new NullOutputPort());

        bool result = provider.Tick();

        result.ShouldBeFalse();
    }

    [Fact]
    public void TakeShouldReturnNullAfterTickWhenProviderIsEmpty()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new NullOutputPort());
        provider.Tick();

        IIndexMappingBuffer? result = provider.Take();

        result.ShouldBeNull();
    }

    [Fact]
    public void TakeShouldRemainNullOnSubsequentCallsAfterTickWhenProviderIsEmpty()
    {
        IndexMappingProviderAbstract provider = CreateProvider(new NullOutputPort());
        provider.Tick();

        IIndexMappingBuffer? firstTake = provider.Take();
        IIndexMappingBuffer? secondTake = provider.Take();

        firstTake.ShouldBeNull();
        secondTake.ShouldBeNull();
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

        instance.ShouldNotBeNull();
        instance.ShouldBeAssignableTo<IndexMappingProviderAbstract>();
        return (IndexMappingProviderAbstract)instance;
    }

    private static Type GetProviderType()
    {
        Type? providerType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.DummyIndexMappingProvider", throwOnError: false);

        providerType.ShouldNotBeNull();
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
