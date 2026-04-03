using NRedberry.Concurrent;
using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

#pragma warning disable CS0618
public sealed class IIndexMappingProviderTests
{
    [Fact]
    public void InterfaceShouldExtendIOutputPortOfIIndexMappingBuffer()
    {
        Type type = typeof(IIndexMappingProvider);

        type.GetInterfaces().ShouldContain(typeof(IOutputPort<IIndexMappingBuffer>));
    }

    [Fact]
    public void InterfaceShouldDefineTickMethodReturningBool()
    {
        Type type = typeof(IIndexMappingProvider);

        var method = type.GetMethod(nameof(IIndexMappingProvider.Tick), Type.EmptyTypes);

        method.ShouldNotBeNull();
        method.ReturnType.ShouldBe(typeof(bool));
    }

    [Fact]
    public void EmptyProviderShouldBeNonNull()
    {
        IIndexMappingProvider provider = IndexMappingProviderUtil.EmptyProvider;

        provider.ShouldNotBeNull();
    }

    [Fact]
    public void EmptyProviderTickShouldReturnFalse()
    {
        IIndexMappingProvider provider = IndexMappingProviderUtil.EmptyProvider;

        bool result = provider.Tick();

        result.ShouldBeFalse();
    }

    [Fact]
    public void EmptyProviderTakeShouldReturnNull()
    {
        IIndexMappingProvider provider = IndexMappingProviderUtil.EmptyProvider;

        IIndexMappingBuffer? result = provider.Take();

        result.ShouldBeNull();
    }

    [Fact]
    public void SingletonShouldThrowArgumentNullExceptionWhenBufferIsNull()
    {
        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
            IndexMappingProviderUtil.Singleton(buffer: null!));

        exception.ParamName.ShouldBe("buffer");
    }

    [Fact]
    public void SingletonShouldYieldProvidedBufferOnceThenReturnNull()
    {
        IIndexMappingBuffer buffer = new TestIndexMappingBuffer();

        IIndexMappingProvider provider = IndexMappingProviderUtil.Singleton(buffer);

        IIndexMappingBuffer first = provider.Take();
        IIndexMappingBuffer? second = provider.Take();

        first.ShouldBeSameAs(buffer);
        second.ShouldBeNull();
    }
}
#pragma warning restore CS0618

internal sealed class TestIndexMappingBuffer : IIndexMappingBuffer
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
        return new TestIndexMappingBuffer();
    }
}
