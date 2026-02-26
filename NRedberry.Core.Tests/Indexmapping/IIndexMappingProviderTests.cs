using System;
using System.Collections.Generic;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

#pragma warning disable CS0618
public sealed class IIndexMappingProviderTests
{
    [Fact]
    public void InterfaceShouldExtendIOutputPortOfIIndexMappingBuffer()
    {
        Type type = typeof(IIndexMappingProvider);

        Assert.Contains(typeof(IOutputPort<IIndexMappingBuffer>), type.GetInterfaces());
    }

    [Fact]
    public void InterfaceShouldDefineTickMethodReturningBool()
    {
        Type type = typeof(IIndexMappingProvider);

        var method = type.GetMethod(nameof(IIndexMappingProvider.Tick), Type.EmptyTypes);

        Assert.NotNull(method);
        Assert.Equal(typeof(bool), method.ReturnType);
    }

    [Fact]
    public void EmptyProviderShouldBeNonNull()
    {
        IIndexMappingProvider provider = IndexMappingProviderUtil.EmptyProvider;

        Assert.NotNull(provider);
    }

    [Fact]
    public void EmptyProviderTickShouldReturnFalse()
    {
        IIndexMappingProvider provider = IndexMappingProviderUtil.EmptyProvider;

        bool result = provider.Tick();

        Assert.False(result);
    }

    [Fact]
    public void EmptyProviderTakeShouldReturnNull()
    {
        IIndexMappingProvider provider = IndexMappingProviderUtil.EmptyProvider;

        IIndexMappingBuffer? result = provider.Take();

        Assert.Null(result);
    }

    [Fact]
    public void SingletonShouldThrowArgumentNullExceptionWhenBufferIsNull()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            IndexMappingProviderUtil.Singleton(buffer: null!));

        Assert.Equal("buffer", exception.ParamName);
    }

    [Fact]
    public void SingletonShouldYieldProvidedBufferOnceThenReturnNull()
    {
        IIndexMappingBuffer buffer = new TestIndexMappingBuffer();

        IIndexMappingProvider provider = IndexMappingProviderUtil.Singleton(buffer);

        IIndexMappingBuffer first = provider.Take();
        IIndexMappingBuffer? second = provider.Take();

        Assert.Same(buffer, first);
        Assert.Null(second);
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
